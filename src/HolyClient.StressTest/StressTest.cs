using DynamicData;
using Fody;
using HolyClient.Abstractions.StressTest;
using HolyClient.Common;
using McProtoNet;
using McProtoNet.Utils;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using HolyClient.Core.Infrastructure;
using DynamicData.Kernel;

namespace HolyClient.StressTest
{
	[MessagePackObject(keyAsPropertyName: true)]
	public class StressTest : ReactiveObject, IStressTest
	{
		#region Properties
		#region Serializable



		[Reactive]
		public string Server { get; set; }
		[Reactive]
		public string BotsNickname { get; set; }
		[Reactive]
		public int NumberOfBots { get; set; }

		[Reactive]
		public MinecraftVersion Version { get; set; } = MinecraftVersion.MC_1_16_5_Version;
		public IEnumerable<ProxyInfo> ProxiesState
		{
			get => Proxies.Items.ToArray();
			set => Proxies.AddRange(value);
		}
		[Reactive]
		[MessagePack.MessagePackFormatter(typeof(PluginTypeRefFormatter))]
		public PluginTypeReference BehaviorRef { get; set; }
		[Reactive]
		public bool UseProxy { get; set; } = true;
		#endregion

		#region NonSerializable




		[IgnoreMember]
		public ISourceList<ProxyInfo> Proxies { get; } = new SourceList<ProxyInfo>();



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




		#endregion


		#endregion


		private Subject<StressTestMetrik> _dataPerSecond = new();
		private readonly object _currentInfoLock = new();
		private StressTestMetrik currentInfo;

		private int _botsOnlineCounter = 0;
		private int _cpsCounter = 0;


		private IDisposable? _cleanUp;

		public StressTest()
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

				IProxyProvider? proxyProvider =
					UseProxy ? new ProxyProvider(this.ProxiesState) : null;


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




					stressTestBots.Add(
						new StressTestBot(
							bot, nickProvider, proxyProvider,
							logger,
							i,
							cancellationTokenSource.Token));

				}

				new Thread(() =>
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
				}.Start();

				CompositeDisposable disposables = new();
				_disposables.Add(disposables);


				_cleanUp = _disposables;


				if (Behavior is not null)
				{
					logger.Information("Загружено поведение: " + Behavior.GetType().FullName);
					await Behavior.Activate(disposables, stressTestBots, cancellationTokenSource.Token);
				}
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
	class NickProvider : INickProvider
	{
		private readonly string _baseNick;

		public NickProvider(string baseNick)
		{
			if (baseNick.Length > 16)
				throw new ArgumentException("Nick long");
			_baseNick = baseNick;

		}
		private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		public string GetNextNick()
		{
			var stringChars = new char[15 - _baseNick.Length];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);
			return _baseNick + finalString;
		}
	}

}
