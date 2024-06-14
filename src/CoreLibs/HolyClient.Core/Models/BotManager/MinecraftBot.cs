using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Serilog;

namespace HolyClient.Core.Models.BotManager;

public class MinecraftBot : IDisposable, IAsyncDisposable
{
    private readonly ISubject<bool> _activating = new Subject<bool>();

    private volatile IDisposable? _cleanUpPlugins;

    private BotState _currentState;
    private bool _disposed;
    private ILogger _logger;

    private readonly List<IBotPlugin> _plugins = new();

    private Subject<BotState> _stateObservable = new();
    private bool isActivate;

    private MinecraftClient minecraftClient;


    public MinecraftBot()
    {
        minecraftClient = new MinecraftClient();
    }

    public IObservable<bool> Activating => _activating;

    public bool IsActivate
    {
        get => isActivate;
        private set
        {
            isActivate = value;
            _activating.OnNext(value);
        }
    }

    public string Nickname { get; set; }
    public string Host { get; set; }
    public ushort Port { get; set; }
    public IObservable<BotState> StateObservable => _stateObservable;

    public BotState CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            _stateObservable.OnNext(value);
        }
    }

    public IEnumerable<IBotPlugin> Plugins => _plugins;

    public bool CheckSrv { get; set; } = true;

    public MinecraftVersion Version { get; set; }

    public MinecraftClient Client => minecraftClient;

    public async ValueTask DisposeAsync()
    {
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        Interlocked.Exchange(ref minecraftClient, null)?.Dispose();
        Interlocked.Exchange(ref _stateObservable, null)?.Dispose();

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public void AddPlugin(IBotPlugin plugin)
    {
        _plugins.Add(plugin);
        plugin.Client = minecraftClient;
    }

    public void RemovePlugin(IBotPlugin plugin)
    {
        _plugins.Remove(plugin);
        plugin.Client = null;
    }

    public async Task Run(ILogger logger, CancellationToken cancellation)
    {
        _logger = logger;
        ThrowIfDispose();
        if (IsActivate) return;
        IsActivate = true;
        var host = Host;
        var port = Port;
        if (port == 25565 && CheckSrv)
            try
            {
                _logger.Information("SRV Lookup");
                IServerResolver resolver = new ServerResolver();
                var result = await resolver.ResolveAsync(host, cancellation);
                host = result.Host;
                port = result.Port;
            }
            catch
            {
                _logger.Information("SRV не найден");
            }

        minecraftClient.Config = new ClientConfig
        {
            Host = host,
            Port = port,
            Username = Nickname,
            Version = Version
        };
        CompositeDisposable disposables = new();
        foreach (var plugin in Plugins)
        {
            CompositeDisposable activatePlugin = new();
            plugin.Logger = _logger;
            plugin.Activate(activatePlugin);
            disposables.Add(activatePlugin);
        }

        Interlocked.Exchange(ref _cleanUpPlugins, disposables);

        minecraftClient.Start(logger);
    }

    public Task Stop()
    {
        ThrowIfDispose();
        if (IsActivate)
        {
            IsActivate = false;
            minecraftClient.Disconnect();
        }

        Interlocked.Exchange(ref _cleanUpPlugins, null)?.Dispose();

        return Task.CompletedTask;
    }


    private void ThrowIfDispose()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }
}