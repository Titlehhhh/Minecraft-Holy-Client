using McProtoNet;
using McProtoNet.Utils;
using Serilog;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace HolyClient.Core.Models.BotManager
{
	public class MinecraftBot : IDisposable, IAsyncDisposable
	{
		private ILogger _logger;
		private ISubject<bool> _activating = new Subject<bool>();
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

		private Subject<BotState> _stateObservable = new Subject<BotState>();
		public IObservable<BotState> StateObservable => _stateObservable;

		private BotState _currentState;
		public BotState CurrentState
		{
			get => _currentState;
			private set
			{
				_currentState = value;
				_stateObservable.OnNext(value);
			}
		}

		private List<IBotPlugin> _plugins = new();
		public IEnumerable<IBotPlugin> Plugins => _plugins;

		public void AddPlugin(IBotPlugin plugin)
		{
			_plugins.Add(plugin);
			plugin.Client = this.minecraftClient;
		}
		public void RemovePlugin(IBotPlugin plugin)
		{
			_plugins.Remove(plugin);
			plugin.Client = null;
		}

		public bool CheckSrv { get; set; } = true;

		public MinecraftVersion Version { get; set; }

		private MinecraftClient minecraftClient;
		private bool isActivate;

		public MinecraftClient Client => minecraftClient;

		private volatile IDisposable? _cleanUpPlugins;

		public async Task Run(ILogger logger, CancellationToken cancellation)
		{
			_logger = logger;
			ThrowIfDispose();
			if (IsActivate)
			{
				return;
			}
			IsActivate = true;
			string host = this.Host;
			ushort port = this.Port;
			if (port == 25565 && this.CheckSrv)
			{
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
			}

			minecraftClient.Config = new ClientConfig
			{
				Host = host,
				Port = port,
				Username = this.Nickname,
				Version = this.Version
			};
			CompositeDisposable disposables = new();
			foreach (var plugin in this.Plugins)
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




		public MinecraftBot()
		{
			this.minecraftClient = new MinecraftClient();
		}
		private bool _disposed;


		private void ThrowIfDispose()
		{
			if (_disposed)
				throw new ObjectDisposedException(GetType().Name);
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

		public async ValueTask DisposeAsync()
		{

		}
	}
}
