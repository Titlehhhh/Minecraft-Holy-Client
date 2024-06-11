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


	public sealed partial class MinecraftClient : Disposable
	{
		#region Events
		public event PacketHandler PacketReceived;
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

		private readonly TcpClient tcpClient;


		private Task mainTask;
		#endregion



		public MinecraftClient()
		{
			pipePair = new DuplexPipePair();


			transportHandler = new TransportHandler(pipePair.Transport);

			packetPipeHandler = new PacketPipeHandler(
				pipePair.Application,
				compressor,
				decompressor);

			packetPipeHandler.PacketReceived = this.OnPacket;

			mainTask = Task.CompletedTask;

			tcpClient = new TcpClient();
			tcpClient.LingerState = new LingerOption(true, 0);

		}
		private void OnPacket(object sender, InputPacket packet)
		{
			this.PacketReceived?.Invoke(this, packet);
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
			Validate();

			if (CTS is not null)
			{
				CTS.Dispose();
			}
			CTS = new CancellationTokenSource();



			await mainTask;



			await ConnectAsync(CTS.Token);


			MinecraftLogin minecraftLogin = new MinecraftLogin();


			var loginOptions = new LoginOptions(Host, Port, Version, Username);

			var result = await minecraftLogin.Login(mainStream, loginOptions, CTS.Token);

			mainTask = MainLoop(result, CTS.Token);
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
			catch (OperationCanceledException)
			{
				Console.WriteLine("Operation Canceeeeeeeel");
			}
			finally
			{
				Debug.WriteLine("Start Complete pipe");
				transportHandler.Complete();
				packetPipeHandler.Complete();
				Debug.WriteLine("Stop Complete pipe");

				pipePair.Reset();
			}

		}


		private async ValueTask ConnectAsync(CancellationToken cancellationToken)
		{



			if (Proxy is not null)
			{
				await tcpClient.ConnectAsync(Proxy.ProxyHost, Proxy.ProxyPort, cancellationToken);
				mainStream = await Proxy.ConnectAsync(tcpClient.GetStream(), Host, Port, cancellationToken);
			}
			else
			{
				await tcpClient.ConnectAsync(Host, Port, cancellationToken);
				mainStream = tcpClient.GetStream();
			}


		}


		public async Task Stop()
		{
			CTS.Cancel();
			tcpClient.Client.Shutdown(SocketShutdown.Both);
			await tcpClient.Client.DisconnectAsync(true);
		}

		protected override void Dispose(bool disposing)
		{
			GC.SuppressFinalize(this);

			CTS.Dispose();
			tcpClient.Dispose();
			compressor.Dispose();
			decompressor.Dispose();

			base.Dispose(disposing);
		}

		public async Task SendTest()
		{
			Debug.WriteLine("TestChat");
			await this.packetPipeHandler.SendPacketAsync(CreateChatPacket(), CTS.Token);
		}

		public ReadOnlyMemory<byte> CreateChatPacket()
		{
			MinecraftPrimitiveWriterSlim writer = new MinecraftPrimitiveWriterSlim();

			writer.WriteVarInt(0x03);
			writer.WriteString("ASdasdasd");

			using var owner = writer.GetWrittenMemory();

			return owner.Memory.ToArray();
		}
	}
}
