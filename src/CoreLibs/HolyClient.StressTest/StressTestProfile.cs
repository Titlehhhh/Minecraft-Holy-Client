﻿using DynamicData;
using DynamicData.Kernel;
using Fody;
using HolyClient.Abstractions.StressTest;
using HolyClient.Common;
using HolyClient.Core.Infrastructure;
using McProtoNet;
using McProtoNet.Utils;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace HolyClient.StressTest
{
	public class ExceptionCounter
	{
		private volatile int _x = 1;

		public int Count => Volatile.Read(ref this._x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Increment()
		{
			Interlocked.Increment(ref _x);
		}

	}

	[MessagePackObject(keyAsPropertyName: true)]
	public class StressTestProfile : ReactiveObject, IStressTestProfile
	{
		#region Properties
		#region Serializable

		public Guid Id { get; set; } = Guid.NewGuid();

		[Reactive]
		public string Name { get; set; }


		[Reactive]
		public string Server { get; set; }
		[Reactive]
		public string BotsNickname { get; set; }
		[Reactive]
		public int NumberOfBots { get; set; }

		[Reactive]
		public MinecraftVersion Version { get; set; } = MinecraftVersion.MC_1_16_5_Version;
		public IEnumerable<IProxySource> ProxiesState
		{
			get => Proxies.Items.ToArray();
			set => Proxies.AddOrUpdate(value);
		}
		[Reactive]
		[MessagePack.MessagePackFormatter(typeof(PluginTypeRefFormatter))]
		public PluginTypeReference BehaviorRef { get; set; }
		[Reactive]
		public bool UseProxy { get; set; } = true;
		#endregion

		#region NonSerializable




		[IgnoreMember]
		public ISourceCache<IProxySource, Guid> Proxies { get; } = new SourceCache<IProxySource, Guid>(x => x.Id);



		[IgnoreMember]
		public IObservable<StressTestMetrik> Metrics => _dataPerSecond;


		[IgnoreMember]
		[Reactive]
		public IStressTestBehavior Behavior
		{
			get;
			private set;
		}



		[Reactive]
		[IgnoreMember]
		public StressTestServiceState CurrentState { get; private set; }

		[IgnoreMember]
		public ISourceCache<ExceptionThrowCount, Type> Exceptions { get; } = new SourceCache<ExceptionThrowCount, Type>(x => x.TypeException);

		[IgnoreMember]
		public ConcurrentDictionary<Type, ExceptionCounter> ExceptionCounter { get; private set; } = new();


		#endregion


		#endregion


		private Subject<StressTestMetrik> _dataPerSecond = new();
		private readonly object _currentInfoLock = new();
		private StressTestMetrik currentInfo;

		private volatile int _botsOnlineCounter = 0;
		private volatile int _cpsCounter = 0;


		private IDisposable? _cleanUp;

		public StressTestProfile()
		{

		}




		[ConfigureAwait(false)]
		public async Task Start(Serilog.ILogger logger)
		{

			CurrentState = StressTestServiceState.Init;
			_botsOnlineCounter = 0;
			_cpsCounter = 0;
			try
			{
				CompositeDisposable _disposables = new();

				CancellationTokenSource cancellationTokenSource = new();

				Disposable.Create(() =>
				{
					if (!cancellationTokenSource.IsCancellationRequested)
					{
						cancellationTokenSource.Cancel();
					}
					cancellationTokenSource.Dispose();
				}).DisposeWith(_disposables);


				var proxyProvider = await LoadProxy(logger);

				var bots = new List<MinecraftClient>();

				string host = this.Server;

				string[] hostPort = host.Split(':');
				ushort port = 25565;
				if (hostPort.Length == 2)
				{
					host = hostPort[0];
					port = ushort.Parse(hostPort[1]);
				}

				if (port == 25565)
				{

					logger.Information($"[STRESS TEST] Поиск srv для {this.Server}");
					try
					{
						IServerResolver resolver = new ServerResolver();

						var result = await resolver.ResolveAsync(host, cancellationTokenSource.Token);
						host = result.Host;
						port = result.Port;
					}
					catch
					{
						logger.Error($"[STRESS TEST] Ошибка поиска srv для {this.Server}");
					}
				}

				logger.Information($"[STRESS TEST] Запущен стресс тест на {this.NumberOfBots} ботов на сервер {host}:{port}");

				var stressTestBots = new List<IStressTestBot>();

				var nickProvider = new NickProvider(this.BotsNickname);







				for (int i = 0; i < this.NumberOfBots; i++)
				{
					if (cancellationTokenSource.IsCancellationRequested)
						break;

					MinecraftClient bot = new MinecraftClient();
					bot.Config = new ClientConfig
					{
						Host = host,
						Port = port,
						Version = this.Version,
						Username = nickProvider.GetNextNick()

					};


					bot.StateChanged += Bot_StateChanged;

					_disposables.Add(Disposable.Create(() =>
					{
						bot.StateChanged -= Bot_StateChanged;
						bot.Dispose();
					}));


					var b = new StressTestBot(
							bot, nickProvider, proxyProvider,
							logger,
							i,
							cancellationTokenSource.Token);

					b.OnError.Subscribe(ex =>
					{
						var key = ex.GetType();

						if (ExceptionCounter.TryGetValue(key, out var counter))
						{
							counter.Increment();
						}
						else
						{
							ExceptionCounter[key] = new ExceptionCounter();
						}

					}).DisposeWith(_disposables);

					stressTestBots.Add(b);

					

				}



				var metricsThread = new Thread(() =>
				{
					try
					{
						Stopwatch stopwatch = new();
						while (!cancellationTokenSource.IsCancellationRequested)
						{
							stopwatch.Start();
							var cps = Interlocked.Exchange(ref _cpsCounter, 0);

							var botsOnline = Volatile.Read(ref _botsOnlineCounter);

							_dataPerSecond.OnNext(new StressTestMetrik(cps, botsOnline));


							stopwatch.Stop();

							if (stopwatch.Elapsed.Microseconds < 1000)
							{
								Thread.Sleep(1000 - stopwatch.Elapsed.Microseconds);
							}
							stopwatch.Reset();
						}
					}
					catch
					{

					}
				})
				{
					Name = "Stress test counter",

					IsBackground = true
				};

				

				CompositeDisposable disposables = new();
				_disposables.Add(disposables);


				_cleanUp = _disposables;

				logger.Information("Запуск поведения");
				if (Behavior is not null)
				{
					await Behavior.Activate(disposables, stressTestBots, cancellationTokenSource.Token);
				}
				logger.Information("Поведение запущено");

				metricsThread.Start();				

				logger.Information("Запущены потоки чтения метрик");

				CurrentState = StressTestServiceState.Running;
			}
			catch
			{
				CurrentState = StressTestServiceState.None;
			}
		}

		private void Bot_StateChanged(object? sender, McProtoNet.StateChangedEventArgs e)
		{
			//Console.WriteLine(e.NewState);
			if (e.NewState == ClientState.Play)
			{
				//Console.WriteLine("Play");
				Interlocked.Increment(ref _botsOnlineCounter);
				Interlocked.Increment(ref _cpsCounter);
			}
			else if (e.NewState == ClientState.Failed)
			{
				if (e.OldState == ClientState.Play)
				{
					Interlocked.Decrement(ref _botsOnlineCounter);
				}
			}
		}


		private async Task<IProxyProvider?> LoadProxy(Serilog.ILogger logger)
		{
			if (!UseProxy)
			{
				logger.Information("Прокси не используются в стресс-тесте");
				return null;
			}

			logger.Information("Загрузка прокси");
			var sources = this.Proxies.Items.ToList();

			if (sources.Count() == 0)
			{
				sources.Add(new UrlProxySource(QuickProxyNet.ProxyType.HTTP, "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/http.txt"));
				sources.Add(new UrlProxySource(QuickProxyNet.ProxyType.SOCKS4, "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/socks4.txt"));
				sources.Add(new UrlProxySource(QuickProxyNet.ProxyType.SOCKS5, "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/socks5.txt"));
			}
			List<Task<IEnumerable<ProxyInfo>>> tasks = new();

			foreach (var s in sources)
			{
				tasks.Add(s.GetProxiesAsync());
			}

			var result = await Task.WhenAll(tasks);

			var proxies = result.SelectMany(x => x).ToList();

			var provider = new ProxyProvider(proxies);

			var group = proxies.GroupBy(x => x.Type).Select(x => $"{x.Key} - {x.Count()}");

			logger.Information($"Загружено {proxies.Count} прокси. {string.Join(", ", group)}");

			return provider;

		}



		public Task Stop()
		{
			Interlocked.Exchange(ref _cleanUp, null)?.Dispose();

			CurrentState = StressTestServiceState.None;
			return Task.CompletedTask;
		}

		public Task Initialization(IPluginProvider pluginProvider)
		{


			Optional<IPluginSource> plugin =
				pluginProvider
				.AvailableStressTestPlugins
				.Lookup(this.BehaviorRef);

			if (plugin.HasValue)
			{
				this.SetBehavior(plugin.Value);
			}
			else
			{

			}
			return Task.CompletedTask;
		}

		public void SetBehavior(IPluginSource pluginSource)
		{
			if (CurrentState != StressTestServiceState.None)
			{
				throw new InvalidOperationException("you cannot change the plugin while running or loading a stress test");
			}

			if (pluginSource is null)
				throw new ArgumentException("parameter is null", nameof(pluginSource));

			if (this.Behavior is not null)
				if (pluginSource.Reference.Equals(this.BehaviorRef))
					return;

			var behavior = pluginSource.CreateInstance<IStressTestBehavior>();
			this.Behavior = behavior;
			this.BehaviorRef = pluginSource.Reference;

		}

		public void DeleteBehavior()
		{
			if (CurrentState != StressTestServiceState.None)
			{
				throw new InvalidOperationException("you cannot delete the plugin while running or loading a stress test");
			}

			this.Behavior = null;
			this.BehaviorRef = default;
		}
	}


}
