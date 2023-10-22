using McProtoNet.MultiVersion.Events;
using McProtoNet.Geometry;
using Org.BouncyCastle.Crypto.Digests;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Threading;
using Microsoft.IO;
using System.Runtime.InteropServices.JavaScript;
using Stateless;
using McProtoNet.MultiVersion.Client;
using System.Runtime.CompilerServices;
using System.IO.Pipelines;
using Serilog;

namespace McProtoNet.MultiVersion
{


	public partial class MinecraftClient : IDisposable
	{
		private ILogger _logger;

		private enum Trigger
		{
			Starting,
			Stop,
			Restart
		}
		public event EventHandler<StateChangedEventArgs> StateChanged;

		public ClientState State
		{
			get => state;
			private set
			{
				var old = this.state;
				this.state = value;
				StateChanged?.Invoke(this, new(old, value));
			}
		}
		public ClientConfig Config { get; set; } = new();

		private MinecraftVersion _protocol;
		private volatile MinecraftClientCore _core;
		//	Pipe pipe;


		public MinecraftClient()
		{
			//	pipe = new Pipe(new PipeOptions(useSynchronizationContext: false));
			CreateEvents();

		}

		private static IPacketPallete _cache754 = new PacketPalette_1_16();

		private IPacketPallete CreatePallete()
		{
			IPacketPallete? packetPallete = null;
			if (Config.Version <= MinecraftVersion.MC_1_12_2_Version)
				packetPallete = new PacketPalette_1_12_2();
			else if (Config.Version < MinecraftVersion.MC_1_14_Version)
				packetPallete = new PacketPalette_1_13();
			else if (Config.Version <= MinecraftVersion.MC_1_15_Version)
				packetPallete = new PacketPalette_1_14();
			else if (Config.Version <= MinecraftVersion.MC_1_15_2_Version)
				packetPallete = new PacketPalette_1_15();
			else if (Config.Version <= MinecraftVersion.MC_1_16_1_Version)
			{
				packetPallete = _cache754;
			}
			else if (Config.Version <= MinecraftVersion.MC_1_16_5_Version)
				packetPallete = new PacketPalette_1_16_2();
			else if (Config.Version <= MinecraftVersion.MC_1_17_1_Version)
				packetPallete = new PacketPalette_1_17();
			else if (Config.Version <= MinecraftVersion.MC_1_18_2_Version)
				packetPallete = new PacketPalette_1_18();
			// else if (protocol <= MC_1_19_Version)
			//     packetPallete = new PacketPalette_1_19();
			//   else if (protocol <= MC_1_19_2_Version)
			//      packetPallete = new PacketPalette_1192();
			//else
			//    packetPallete = new PacketPalette1193();
			return packetPallete;
		}
		private void CreateNewCore()
		{
			RemoveCore();
			//pipe.Reset();
			//	throw new Exception("CTOR");
			_core = new MinecraftClientCore(
				Config.Version,
				Config.Username,
				Config.Host,
				Config.Port,
				Config.Proxy,
				CreatePallete(),
				//	this.pipe,
				this._logger);

		}
		private async void RemoveCore()
		{
			Interlocked.Exchange(ref _core, null)?.DisposeAsync();
		}
		private void ValidateConfig()
		{
			if (string.IsNullOrWhiteSpace(Config.Username))
			{
				throw new ValidationException("Username is empty");
			}
			if (string.IsNullOrWhiteSpace(Config.Host))
			{
				throw new ValidationException("Host is empty");
			}
			if (Config.Proxy is { })
			{

				//ToDO login
			}
		}
		private Task? workTask;
		public TaskAwaiter GetAwaiter()
		{
			if (workTask is null)
				return default;
			return workTask.GetAwaiter();
		}

		public async Task Login(Serilog.ILogger logger)
		{
			_logger = logger;

			_startDisconnect = false;
			ValidateConfig();
			CreateNewCore();
			_protocol = Config.Version;
			try
			{
				State = ClientState.Connecting;
				await _core.Connect();

				State = ClientState.HandShake;
				await _core.HandShake();

				State = ClientState.Login;
				workTask = await _core.Login(OnPacket);
				State = ClientState.Play;
			}
			catch (Exception e) when (e is not OperationCanceledException)
			{
				_logger.Error(e, "Во время запуска клиента произошла ошибка");
				throw e;
			}


		}
		private bool _startDisconnect = false;
		public async void Disconnect()
		{
			if (_startDisconnect)
				return;
			_startDisconnect = true;
			if (_core is not null)
				await _core.DisposeAsync();
		}



		~MinecraftClient()
		{
			Dispose();
		}

		private bool _disposed = false;
		private ClientState state;

		public void Dispose()
		{

			if (_disposed) return;



			if (_core is { })
			{
				_core.Dispose();
			}

			//	if (pipe is { })
			{

			}
			//pipe = null;

			_disposed = true;

			GC.SuppressFinalize(this);
		}
		public ValueTask DisposeAsync()
		{

			return ValueTask.CompletedTask;
		}





	}
}