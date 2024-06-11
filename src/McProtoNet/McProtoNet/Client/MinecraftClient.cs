using DotNext;
using DotNext.IO.Pipelines;
using LibDeflate;
using McProtoNet.Protocol;
using McProtoNet.Serialization;
using QuickProxyNet;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;

namespace McProtoNet.Client
{

	public enum MinecraftClientState
	{
		Stopped,
		Connect,
		Errored,
		Handshaking,
		Login,
		Play
	}
	public sealed class StateEventArgs : EventArgs
	{
		public MinecraftClientState State { get; }
		public MinecraftClientState OldState { get; }

		public Exception? Error { get; }

		public StateEventArgs(MinecraftClientState newState, MinecraftClientState oldState)
		{
			State = newState;
			OldState = oldState;
		}

		public StateEventArgs(Exception ex, MinecraftClientState newState, MinecraftClientState oldState)
		{
			Error = ex;
			State = newState;
			OldState = oldState;
		}
		public StateEventArgs(Exception ex, MinecraftClientState oldState)
		{
			Error = ex;
			State = MinecraftClientState.Errored;
			OldState = oldState;

		}
	}

	public sealed partial class MinecraftClient : Disposable
	{
		#region Events
		public event PacketHandler OnPacket;
		public event EventHandler<StateEventArgs> StateChanged;

		#endregion

		#region Properties		
		public string Host { get; set; }
		public ushort Port { get; set; } = 25565;

		public string Username { get; set; }
		public int Version { get; set; }
		public IProxyClient? Proxy { get; set; }
		#endregion


		#region Constans

		public const int MinVersionSupport = 754;
		public const int MaxVersionSupport = 765;

		#endregion

		#region Fields
		private Stream mainStream;
		private CancellationTokenSource CTS;


		private readonly DuplexPipePair pipePair;

		private readonly ZlibCompressor compressor = new ZlibCompressor(4);
		private readonly ZlibDecompressor decompressor = new ZlibDecompressor();

		private readonly TransportHandler transportHandler;
		private readonly PacketPipeHandler packetPipeHandler;

		MinecraftClientLogin minecraftLogin = new MinecraftClientLogin();

		private TcpClient tcpClient;


		private Task mainTask;

		private MinecraftClientState _state;
		#endregion



		public MinecraftClient()
		{
			pipePair = new DuplexPipePair();


			transportHandler = new TransportHandler(pipePair.Transport);

			packetPipeHandler = new PacketPipeHandler(
				pipePair.Application,
				compressor,
				decompressor);

			packetPipeHandler.PacketReceived = this.PacketReceived;

			mainTask = Task.CompletedTask;

			minecraftLogin.StateChanged += MinecraftLogin_StateChanged;



		}

		private void MinecraftLogin_StateChanged(MinecraftClientState state)
		{
			StateChange(state);
		}

		private void PacketReceived(object sender, InputPacket packet)
		{
			this.OnPacket?.Invoke(this, packet);
		}
		private void Validate()
		{
			if (string.IsNullOrWhiteSpace(Host))
				throw new ArgumentException("Host is empty");
			if (string.IsNullOrEmpty(Username))
				throw new ArgumentException("Username is empty");

			if (!(Version >= MinVersionSupport && Version <= MaxVersionSupport))
			{
				throw new ArgumentException("Version not supported");
			}
		}

		public async Task Start()
		{
			try
			{
				Validate();


				if (CTS is not null)
				{
					CTS.Dispose();
				}
				CTS = new CancellationTokenSource();



				await mainTask;


				StateChange(MinecraftClientState.Connect);
				await ConnectAsync(CTS.Token);






				var loginOptions = new LoginOptions(Host, Port, Version, Username);



				var result = await minecraftLogin.Login(mainStream, loginOptions, CTS.Token);

				mainTask = MainLoop(result, CTS.Token);
			}
			catch (OperationCanceledException ex)
			{
				if (!CTS.IsCancellationRequested)
				{
					OnException(ex);
				}
			}
			catch (Exception ex)
			{
				OnException(ex);
				throw;
			}
		}
		private void StateChange(MinecraftClientState state)
		{
			var old = _state;
			_state = state;
			StateChanged?.Invoke(this, new StateEventArgs(old, state));
		}


		private void OnException(Exception ex)
		{
			CleanUp();
			StateChanged?.Invoke(this, new StateEventArgs(ex, _state));
		}
		private async Task MainLoop(LoginizationResult loginizationResult, CancellationToken cancellationToken)
		{
			try
			{
				transportHandler.BaseStream = loginizationResult.Stream;

				packetPipeHandler.CompressionThreshold = loginizationResult.CompressionThreshold;


				Task transport = transportHandler.StartAsync(cancellationToken);
				Task packets = packetPipeHandler.StartAsync(cancellationToken);
				await Task.WhenAll(transport, packets);

			}
			catch (OperationCanceledException ex)
			{
				if (!CTS.IsCancellationRequested)
				{
					OnException(ex);
				}
			}
			catch (Exception ex)
			{
				OnException(ex);
			}
			finally
			{
				transportHandler.Complete();
				packetPipeHandler.Complete();
				pipePair.Reset();
			}

		}


		private async ValueTask ConnectAsync(CancellationToken cancellationToken)
		{


			var newTcp = new TcpClient();

			Interlocked.Exchange(ref tcpClient, newTcp)?.Dispose();

			newTcp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			newTcp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
			newTcp.Client.NoDelay = true;
			newTcp.LingerState = new LingerOption(false, 0);


			if (Proxy is not null)
			{
				await newTcp.ConnectAsync(Proxy.ProxyHost, Proxy.ProxyPort, cancellationToken);
				mainStream = await Proxy.ConnectAsync(newTcp.GetStream(), Host, Port, cancellationToken);
			}
			else
			{
				try
				{
					await newTcp.Client.DisconnectAsync(true, cancellationToken);
				}
				catch { }
				await newTcp.ConnectAsync(Host, Port, cancellationToken);
				mainStream = newTcp.GetStream();
			}



		}


		private void CleanUp()
		{
			minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;
			if (CTS is not null)
			{
				CTS.Cancel();
				CTS.Dispose();
				CTS = null;
			}
			
			Interlocked.Exchange(ref tcpClient, null)?.Dispose();
			Interlocked.Exchange(ref mainStream, null)?.Dispose();
		}

		public void Stop()
		{
			CleanUp();

			StateChanged?.Invoke(this, new StateEventArgs(MinecraftClientState.Stopped, _state));
		}

		protected override void Dispose(bool disposing)
		{
			GC.SuppressFinalize(this);
			minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;

			CTS.Dispose();
			mainStream.Dispose();
			tcpClient.Dispose();			
			compressor.Dispose();
			decompressor.Dispose();

			base.Dispose(disposing);
		}
	}
}
