using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using DynamicData;
using Fody;
using HolyClient.Abstractions.StressTest;
using HolyClient.Common;
using HolyClient.Core.Infrastructure;
using HolyClient.Proxy;
using McProtoNet.Abstractions;
using McProtoNet.Client;
using McProtoNet.Utils;
using MessagePack;
using QuickProxyNet;
using QuickProxyNet.ProxyChecker;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;

namespace HolyClient.StressTest;

public class ExceptionCounter
{
    private volatile int _x = 1;

    public int Count => Volatile.Read(ref _x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment()
    {
        Interlocked.Increment(ref _x);
    }
}

[MessagePackObject(true)]
public class StressTestProfile : ReactiveObject, IStressTestProfile
{
    private readonly object _currentInfoLock = new();

    private volatile int _botsConnectionCounter;
    private volatile int _botsHandshakeCounter;
    private volatile int _botsLoginCounter;
    private volatile int _botsPlayCounter;


    private IDisposable? _cleanUp;

    private volatile int _cpsCounter;


    private readonly Subject<StressTestMetrik> _dataPerSecond = new();
    private StressTestMetrik currentInfo;


    [ConfigureAwait(false)]
    public async Task Start(ILogger logger)
    {
        ExceptionCounter.Clear();
        CurrentState = StressTestServiceState.Init;
        _botsConnectionCounter = 0;
        _botsHandshakeCounter = 0;
        _botsLoginCounter = 0;
        _botsPlayCounter = 0;
        _cpsCounter = 0;
        try
        {
            CompositeDisposable disposables = new();

            CancellationTokenSource cancellationTokenSource = new();

            Disposable.Create(() =>
            {
                if (!cancellationTokenSource.IsCancellationRequested) cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }).DisposeWith(disposables);


            if (ParallelCountCheckingCalculateAuto) ProxyCheckerOptions.ParallelCount = NumberOfBots * 10;

            var host = Server;

            var hostPort = host.Split(':');
            ushort port = 25565;
            if (hostPort.Length == 2)
            {
                host = hostPort[0];
                port = ushort.Parse(hostPort[1]);
            }

            if (port == 25565)
            {
                logger.Information($"[STRESS TEST] Поиск srv для {Server}");
                try
                {
                    IServerResolver resolver = new ServerResolver();

                    SrvRecord result = await resolver.ResolveAsync(host, cancellationTokenSource.Token);
                    host = result.Host;
                    port = result.Port;
                }
                catch
                {
                    logger.Error($"[STRESS TEST] Ошибка поиска srv для {Server}");
                }
            }

            logger.Information($"[STRESS TEST] Поиск DNS для {Server}");

            var srv_host = host;
            var srv_port = port;

            if (OptimizeDNS)
                try
                {
                    var result = await Dns
                        .GetHostAddressesAsync(host, AddressFamily.InterNetwork, cancellationTokenSource.Token)
                        .ConfigureAwait(false);

                    host = result[0].ToString();
                    logger.Information($"[STRESS TEST] DNS IP for {Server} - {host}");
                }
                catch
                {
                    logger.Error($"[STRESS TEST] Ошибка поиска DNS для {Server}");
                }


            ProxyProvider? proxyProvider = null;

            if (UseProxy)
            {
                var proxies = (await LoadProxy(logger)).Cast<ProxyRecord>();
                if (proxies.Count() > 0)
                {
                    IProxyChecker proxyChecker = ProxyChecker.CreateChunked(proxies, new ProxyCheckerChunkedOptions()
                    {
                        ChunkSize = NumberOfBots * 10,
                        ConnectTimeout = 10_000,
                        IsSingleConsumer = false,
                        SendAlive = true,
                        TargetHost = host,
                        TargetPort = port
                    });
                    proxyChecker.Start();
                    proxyChecker.DisposeWith(disposables);
                    disposables.Add();
                    logger.Information("Запущен прокси-чекер");
                }
                else
                {
                    logger.Information("Не удалось загрузить прокси");
                }
            }


            var bots = await RunBots(
                logger,
                cancellationTokenSource,
                proxyProvider,
                disposables,
                host,
                port,
                srv_host,
                srv_port);

            _cleanUp = disposables;

            logger.Information("Запуск поведения");
            if (Behavior is not null) await Behavior.Activate(disposables, bots, logger, cancellationTokenSource.Token);
            logger.Information("Поведение запущено");


            var t = Task.Run(async () =>
            {
                try
                {
                    Stopwatch stopwatch = new();

                    while (!cancellationTokenSource.IsCancellationRequested)
                    {
                        stopwatch.Start();
                        var cps = Interlocked.Exchange(ref _cpsCounter, 0);

                        var botsOnline = Volatile.Read(ref _botsPlayCounter);

                        _dataPerSecond.OnNext(new StressTestMetrik(cps, botsOnline));


                        stopwatch.Stop();

                        if (stopwatch.Elapsed.Microseconds < 1000)
                            await Task.Delay(1000 - stopwatch.Elapsed.Microseconds);
                        stopwatch.Reset();
                    }
                }
                catch
                {
                }
            });
            logger.Information("Запущены потоки чтения метрик");


            CurrentState = StressTestServiceState.Running;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Не удалось запустить стресс тест");

            CurrentState = StressTestServiceState.None;
        }
    }


    public Task Stop()
    {
        Interlocked.Exchange(ref _cleanUp, null)?.Dispose();

        CurrentState = StressTestServiceState.None;
        return Task.CompletedTask;
    }

    public Task Initialization(IPluginProvider pluginProvider)
    {
        var plugin =
            pluginProvider
                .AvailableStressTestPlugins
                .Lookup(BehaviorRef);

        if (plugin.HasValue) SetBehavior(plugin.Value);

        return Task.CompletedTask;
    }

    public void SetBehavior(IPluginSource pluginSource)
    {
        if (CurrentState != StressTestServiceState.None)
            throw new InvalidOperationException("you cannot change the plugin while running or loading a stress test");

        if (pluginSource is null)
            throw new ArgumentException("parameter is null", nameof(pluginSource));

        if (Behavior is not null)
            if (pluginSource.Reference.Equals(BehaviorRef))
                return;

        var behavior = pluginSource.CreateInstance<IStressTestBehavior>();
        Behavior = behavior;
        BehaviorRef = pluginSource.Reference;
    }

    public void DeleteBehavior()
    {
        if (CurrentState != StressTestServiceState.None)
            throw new InvalidOperationException("you cannot delete the plugin while running or loading a stress test");

        Behavior = null;
        BehaviorRef = default;
    }

    private async Task<List<IStressTestBot>> RunBots(
        ILogger logger,
        CancellationTokenSource cancellationTokenSource,
        IProxyProvider? proxyProvider,
        CompositeDisposable disposables,
        string host,
        ushort port,
        string? srv_host,
        ushort? srv_port)
    {
        var stressTestBots = new List<IStressTestBot>();

        var nickProvider = new NickProvider(BotsNickname);


        for (var i = 0; i < NumberOfBots; i++)
        {
            if (cancellationTokenSource.IsCancellationRequested)
                break;

            MinecraftClient bot = new MinecraftClient();

            bot.StateChanged += BotOnStateChanged;

            disposables.Add(Disposable.Create(() => { bot.StateChanged -= BotOnStateChanged; }));

            bot.Host = srv_host;
            bot.Port = (ushort)srv_port;
            bot.Version = Version;
            bot.Username = nickProvider.GetNextNick();


            var b = new StressTestBot(
                bot,
                nickProvider,
                proxyProvider,
                logger,
                i,
                cancellationTokenSource.Token);


            stressTestBots.Add(b);
            b.DisposeWith(disposables);
        }

        logger.Information($"[STRESS TEST] Запущен стресс тест на {NumberOfBots} ботов на сервер {host}:{port}");


        return stressTestBots;
    }

    private void BotOnStateChanged(object? sender, StateEventArgs e)
    {
        if (e.State == MinecraftClientState.Play)
        {
            Interlocked.Increment(ref _cpsCounter);
            Interlocked.Increment(ref _botsPlayCounter);
        }
        else if (e.State == MinecraftClientState.Errored)
        {
            if (e.OldState == MinecraftClientState.Play)
            {
                Interlocked.Decrement(ref _botsPlayCounter);
            }

            Exception exc = e.Error;
            var key = Tuple.Create(exc.GetType().FullName, exc.Message);

            if (ExceptionCounter.TryGetValue(key, out var counter))
                counter.Increment();
            else
                ExceptionCounter[key] = new ExceptionCounter();
        }
    }

    private async Task<IEnumerable<ProxyInfo>> LoadProxy(ILogger logger)
    {
        if (!UseProxy)
        {
            logger.Information("Прокси не используются в стресс-тесте");
            return null;
        }


        var sources = Proxies.Items.ToList();
        logger.Information("Загрузка прокси");
        if (sources.Count() == 0)
        {
            sources.Add(new UrlProxySource(type: null,
                "https://raw.githubusercontent.com/proxifly/free-proxy-list/refs/heads/main/proxies/all/data.txt"));

            sources.Add(new UrlProxySource(ProxyType.HTTP,
                "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/http.txt"));
            sources.Add(new UrlProxySource(ProxyType.SOCKS4,
                "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/socks4.txt"));
            sources.Add(new UrlProxySource(ProxyType.SOCKS5,
                "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/socks5.txt"));
        }

        List<Task<IEnumerable<ProxyInfo>>> tasks = new();

        foreach (var s in sources) tasks.Add(s.GetProxiesAsync());

        var result = await Task.WhenAll(tasks);

        var proxies = result.SelectMany(x => x).ToList();


        var group = proxies.GroupBy(x => x.Type).Select(x => $"{x.Key} - {x.Count()}");

        logger.Information($"Загружено {proxies.Count} прокси. {string.Join(", ", group)}");

        return proxies;
    }

    #region Properties

    #region Serializable

    public Guid Id { get; set; } = Guid.NewGuid();

    [Reactive] public string Name { get; set; }


    [Reactive] public string Server { get; set; }

    [Reactive] public string BotsNickname { get; set; }

    [Reactive] public int NumberOfBots { get; set; }

    [Reactive] public int Version { get; set; } = 754;

    public IEnumerable<IProxySource> ProxiesState
    {
        get => Proxies.Items.ToArray();
        set => Proxies.AddOrUpdate(value);
    }

    [Reactive]
    [MessagePackFormatter(typeof(PluginTypeRefFormatter))]
    public PluginTypeReference BehaviorRef { get; set; }

    [Reactive] public bool UseProxy { get; set; } = true;

    [Reactive] public bool OptimizeDNS { get; set; } = false;


    #region ProxyChecker

    public ProxyCheckerOptions ProxyCheckerOptions { get; set; } = new();

    [Reactive] public bool ParallelCountCheckingCalculateAuto { get; set; } = true;

    #endregion

    #endregion

    #region NonSerializable

    [IgnoreMember]
    public ISourceCache<IProxySource, Guid> Proxies { get; } = new SourceCache<IProxySource, Guid>(x => x.Id);


    [IgnoreMember] public IObservable<StressTestMetrik> Metrics => _dataPerSecond;


    [IgnoreMember] [Reactive] public IStressTestBehavior Behavior { get; private set; }


    [Reactive] [IgnoreMember] public StressTestServiceState CurrentState { get; private set; }


    [IgnoreMember]
    public ConcurrentDictionary<Tuple<string, string>, ExceptionCounter> ExceptionCounter { get; } = new();

    #endregion

    #endregion
}