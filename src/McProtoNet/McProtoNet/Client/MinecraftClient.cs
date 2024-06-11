using DotNext;
using LibDeflate;
using McProtoNet.Protocol;
using QuickProxyNet;
using System.IO.Pipelines;
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

		private MinecraftPacketReader reader;
		private MinecraftPacketSender sender;

		//private IMinecraftPrimitiveWriter primitiveWriter;
		//private IMinecraftPrimitiveReader primitiveReader;


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

			CTS = new CancellationTokenSource();



			await ConnectAsync(CTS.Token);

			if (reader is null || sender is null)
			{
				reader = new MinecraftPacketReader();
				sender = new MinecraftPacketSender();
			}
			reader.BaseStream = mainStream;
			sender.BaseStream = mainStream;
		}


		private async ValueTask ConnectAsync(CancellationToken cancellationToken)
		{
			if (Proxy is null)
			{
				TcpClient tcpClient = new TcpClient();

				await tcpClient.ConnectAsync(Host, Port, cancellationToken);

				mainStream = tcpClient.GetStream();
			}
			else
			{
				mainStream = await Proxy.ConnectAsync(Host, Port, cancellationToken);
			}
		}

		private async ValueTask HandshakeAsync()
		{

		}

		public async Task Stop()
		{

		}
	}
}
