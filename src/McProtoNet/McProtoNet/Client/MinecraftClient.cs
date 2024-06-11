using DotNext;
using DotNext.IO.Pipelines;
using LibDeflate;
using McProtoNet.Protocol;
using QuickProxyNet;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;

namespace McProtoNet.Client
{

	internal delegate void MinecraftPacketHandler(InputPacket packet);
	public sealed class ClientContext
	{
		public ZlibCompressor Compressor { get; }
		public ZlibDecompressor Decompressor { get; }

		public IDuplexPipe Pipe { get; }

		public int CompressionThreshold { get; set; }
		public int ProtocolVersion { get; set; }
	}

	internal class DuplexPipe : IDuplexPipe
	{
		public PipeReader Input { get; private set; }

		public PipeWriter Output { get; private set; }

		public DuplexPipe(PipeReader input, PipeWriter output)
		{
			Input = input;
			Output = output;
		}
	}

	internal sealed class DuplexPipePair
	{
		public IDuplexPipe Transport { get; }
		public IDuplexPipe Application { get; }

		private readonly Pipe pipe1;
		private readonly Pipe pipe2;

		public DuplexPipePair()
		{
			pipe1 = new Pipe();
			pipe2 = new Pipe();

			Transport = new DuplexPipe(pipe1.Reader, pipe2.Writer);
			Application = new DuplexPipe(pipe2.Reader, pipe1.Writer);
		}

		public void Reset()
		{
			pipe1.Reset();
			pipe2.Reset();
		}
	}

	public sealed partial class MinecraftClient : Disposable
	{

		public string Host { get; set; }
		public ushort Port { get; set; } = 25565;

		public string Username { get; set; }
		public int Version { get; set; }
		public IProxyClient? Proxy { get; set; }



		public const int MinVersionSupport = 754;
		public const int MaxVersionSupport = 765;


		private Stream mainStream;
		private CancellationTokenSource CTS;


		private readonly DuplexPipePair pipePair;


		private readonly TransportHandler transportHandler;
		private readonly PacketPipeHandler packetPipeHandler;

		private readonly TcpClient tcpClient;


		private Task mainTask;

		public MinecraftClient()
		{
			pipePair = new DuplexPipePair();
			transportHandler = new TransportHandler(pipePair.Transport);
			packetPipeHandler = new PacketPipeHandler(pipePair.Application);

			mainTask = Task.CompletedTask;
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


			await mainTask.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);



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
				//packetPipeHandler.CompressionThreshold = loginizationResult.CompressionThreshold;
				Task transport = transportHandler.StartAsync(cancellationToken);
				Task packets = packetPipeHandler.StartAsync(cancellationToken);
				await Task.WhenAll(transport, packets);

			}
			finally
			{

			}

		}


		private async ValueTask ConnectAsync(CancellationToken cancellationToken)
		{

			if (tcpClient.Connected)
			{
				tcpClient.Client.Shutdown(SocketShutdown.Both);
				await tcpClient.Client.DisconnectAsync(true, cancellationToken);
			}

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



			if (Proxy is null)
			{
				await tcpClient.ConnectAsync(Host, Port, cancellationToken);
				mainStream = tcpClient.GetStream();
			}
			else
			{
				mainStream = await Proxy.ConnectAsync(Host, Port, cancellationToken);
			}
		}


		public void Stop()
		{
			CTS.Cancel();
			tcpClient.Client.Shutdown(SocketShutdown.Both);
			tcpClient.Client.Disconnect(true);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
