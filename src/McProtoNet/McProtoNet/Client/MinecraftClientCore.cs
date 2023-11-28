using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;
using McProtoNet.Utils;
using Microsoft.IO;
using QuickProxyNet;
using Serilog;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;

namespace McProtoNet
{
	internal delegate void OnPacketReceived(MinecraftPrimitiveReader reader, PacketIn id, CancellationToken cancellation);

	internal class MinecraftClientCore : IDisposable, IAsyncDisposable
	{
		#region ReadOnlyFields


		private MinecraftVersion _protocol;
		private string _nick;
		private string _host;
		private ushort _port;

		private IProxyClient? _proxy;
		private Pipe pipe;
		private IPacketPallete _packetPallete;
		private ILogger _logger;

		private CancellationTokenSource CTS = new();

		public MinecraftClientCore(MinecraftVersion protocol, string nick, string host, ushort port, IProxyClient? proxy, IPacketPallete packetPallete, ILogger logger)
		{
			_protocol = protocol;
			_nick = nick;
			_host = host;
			_port = port;
			_proxy = proxy;
			_packetPallete = packetPallete;
			//this.pipe = pipe;
			_logger = logger;
		}


		private int threshold;
		private SubProtocol _subProtocol;



		#endregion

		#region StateFields
		Stream tcp;
		private MinecraftStream minecraftStream;
		internal IMinecraftPacketReader PacketReader;
		internal IMinecraftPacketSender PacketSender;
		#endregion



		internal async Task Connect()
		{


			tcp = await CreateTcp(CTS.Token);

			minecraftStream = new MinecraftStream(tcp);
			PacketSender = new MinecraftPacketSender(minecraftStream, true);


		}
		internal async Task HandShake()
		{
			_subProtocol = SubProtocol.HandShake;
			_logger.Information("Рукопожатие");
			await PacketSender.SendPacketAsync(
					 new HandShakePacket(
						 HandShakeIntent.LOGIN,
						 (int)_protocol,
						 _host,
						 _port),
					 0x00, CTS.Token);


		}
		internal async Task<Task> Login(OnPacketReceived packetReceived)
		{
			CTS.Token.ThrowIfCancellationRequested();

			_subProtocol = SubProtocol.Login;
			_logger.Information("Логинизация");
			await this.SendPacket(w =>
			{
				w.WriteString(this._nick);
			}, 0x00);


			await using (PacketReader = new MinecraftPacketReader(minecraftStream, false))
			{
				await LoginCore(CTS.Token);
			}
			//Task fill = FillPipeAsync(CTS.Token);
			Task read = ReadPipeAsync(CTS.Token, packetReceived);



			return read;
		}
		private async ValueTask LoginCore(CancellationToken cancellation)
		{
			bool ok = false;
			do
			{
				using (Packet readData = await PacketReader.ReadNextPacketAsync(cancellation))
				{
					var reader = Performance.Readers.Get();
					try
					{
						reader.BaseStream = readData.Data;


						ok = await HandleLogin(reader, readData.Id);
					}
					finally
					{
						Performance.Readers.Return(reader);
					}
				}

			} while (!ok);

		}
		private async Task<bool> HandleLogin(MinecraftPrimitiveReader reader, int id)
		{

			if (id == 0x02)
			{
				_subProtocol = SubProtocol.Game;
				_logger.Information("Переход в Play режим");
				return true;
			}
			else if (id == 0x03)
			{
				threshold = reader.ReadVarInt();
				_logger.Information($"Включаем сжатие: {threshold}");
				PacketReader.SwitchCompression(threshold);
				PacketSender.SwitchCompression(threshold);
			}
			else if (id == 0x01)
			{
				reader.ReadString();
				byte[] publicKey = reader.ReadByteArray();
				byte[] verifyToken = reader.ReadByteArray();
				var RSAService = CryptoHandler.DecodeRSAPublicKey(publicKey);
				byte[] secretKey = CryptoHandler.GenerateAESPrivateKey();

				byte[] g = RSAService.Encrypt(secretKey, false);
				byte[] token = RSAService.Encrypt(verifyToken, false);
				_logger.Information("Включаем шифрование");
				await SendEncrypt(g, token);
				minecraftStream.SwitchEncryption(secretKey);

			}
			else if (id == 0x00)
			{
				var r = reader.ReadString();

				throw new LoginRejectedException(r);
			}
			else
			{
				throw new Exception("Unkown packet: " + id);
			}

			return false;
		}


		private async Task ReadPipeAsync(CancellationToken cancellationToken, OnPacketReceived packetReceived)
		{

			PacketReader = new MinecraftPacketReader(minecraftStream, false);
			PacketReader.SwitchCompression(threshold);

			while (!cancellationToken.IsCancellationRequested)
			{
				using (var packet = await PacketReader.ReadNextPacketAsync(cancellationToken))
				{
					if (_packetPallete.TryGetIn(packet.Id, out var packetIn))
					{
						packet.Data.Position = 0;

						var reader = Performance.Readers.Get();

						try
						{



							reader.BaseStream = packet.Data;

							packetReceived.Invoke(reader, packetIn, cancellationToken);
						}
						finally
						{
							Performance.Readers.Return(reader);
						}
					}
				}
			}

		}
		private async Task FillPipeAsync(CancellationToken cancellationToken)
		{
			return;
			const int minimumBufferSize = 512;
			while (!cancellationToken.IsCancellationRequested)
			{
				Memory<byte> memory = pipe.Writer.GetMemory(minimumBufferSize);
				int bytesRead = await minecraftStream.ReadAsync(memory, cancellationToken);


				pipe.Writer.Advance(bytesRead);
				FlushResult result = await pipe.Writer.FlushAsync();
				if (result.IsCompleted)
				{
					break;
				}
			}
		}




		private async Task<Stream> CreateTcp(CancellationToken cancellation)
		{
			cancellation.ThrowIfCancellationRequested();
			if (_proxy is null)
			{

				TcpClient tcp = new TcpClient();

				_logger.Information("Подключение");
				await tcp.ConnectAsync(_host, _port, cancellation);
				return tcp.GetStream();
			}
			_logger.Information($"Подключение к {_proxy.Type} прокси {_proxy.ProxyHost}:{_proxy.ProxyPort}");
			return await _proxy.ConnectAsync(_host, _port, 10000, cancellation);
		}



		private ValueTask SendEncrypt(byte[] key, byte[] token)
		{
			return SendPacket(w =>
			{
				w.WriteByteArray(key);
				w.WriteByteArray(token);
			}, 0x01);
		}
		public ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, PacketOut id)
		{
			int _id = _packetPallete.GetOut(id);
			return SendPacket(action, _id);
		}
		private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
		static RecyclableMemoryStreamManager streamManager = new();


		public async ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, int id)
		{
			await semaphore.WaitAsync();
			try
			{
				using (MemoryStream ms = streamManager.GetStream())
				{
					var writer = Performance.Writers.Get();
					try
					{


						writer.BaseStream = ms;
						action(writer);
						ms.Position = 0;
					}
					finally
					{
						Performance.Writers.Return(writer);
					}
					await PacketSender.SendPacketAsync(new(id, ms), CTS.Token);

				}
			}
			catch { }
			finally
			{
				semaphore.Release();
			}
		}
		internal async ValueTask SendPacketAsync(IOutputPacket packet, int id)
		{
			await semaphore.WaitAsync();
			try
			{
				using (MemoryStream ms = streamManager.GetStream())
				{
					var writer = Performance.Writers.Get();
					try
					{

						writer.BaseStream = ms;

						packet.Write(writer);

					}
					finally
					{
						Performance.Writers.Return(writer);
					}
					ms.Position = 0;
					await PacketSender.SendPacketAsync(new(id, ms), CTS.Token);
				}
			}
			catch { }
			finally
			{
				semaphore.Release();
			}
		}
		bool _disposed;

		public void Dispose()
		{
			if (_disposed) return;

			if (pipe is { })
			{
				pipe = null;
			}
			if (PacketSender is { })
			{
				PacketSender.Dispose();
			}
			PacketSender = null;
			if (PacketReader is { })
			{
				PacketReader.Dispose();
			}
			PacketReader = null;

			_proxy = null;
			_packetPallete = null;
			_disposed = true;

			_logger = null;

			if (CTS is { })
			{
				if (!CTS.IsCancellationRequested)
				{
					CTS.Cancel();
				}
				CTS.Dispose();
			}
			CTS = null;

			GC.SuppressFinalize(this);

		}
		public async ValueTask DisposeAsync()
		{
			if (_disposed) return;

			//if (pipe is { })
			{
				//	pipe = null;
			}
			if (PacketSender is { })
			{
				await PacketSender.DisposeAsync();
			}
			PacketSender = null;
			if (PacketReader is { })
			{
				await PacketReader.DisposeAsync();
			}
			PacketReader = null;
			_proxy = null;
			_packetPallete = null;
			_disposed = true;

			_logger = null;


			if (!CTS.IsCancellationRequested)
			{
				await CTS.CancelAsync();
			}
			CTS.Dispose();



			GC.SuppressFinalize(this);
		}
	}
}