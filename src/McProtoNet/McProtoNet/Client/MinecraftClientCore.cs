using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;
using McProtoNet.Utils;
using Microsoft.IO;
using QuickProxyNet;
using Serilog;
using System.IO.Pipelines;
using System.Net.Sockets;

namespace McProtoNet
{
	public delegate void OnPacketReceived(MinecraftPrimitiveReader reader, PacketIn id, CancellationToken cancellation);

	public class MinecraftClientCore : IDisposable, IAsyncDisposable
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

		public MinecraftClientCore(MinecraftVersion protocol, string nick, string host, ushort port, IProxyClient? proxy, IPacketPallete packetPallete, Pipe pipe, ILogger logger)
		{
			_protocol = protocol;
			_nick = nick;
			_host = host;
			_port = port;
			_proxy = proxy;
			_packetPallete = packetPallete;
			this.pipe = pipe;
			_logger = logger;
		}


		private int threshold;
		private SubProtocol _subProtocol;



		#endregion

		#region StateFields
		Stream mainStream;
		private MinecraftStream minecraftStream;
		public IMinecraftPacketReader PacketReader;
		public IMinecraftPacketSender PacketSender;
		#endregion

		public void Reset()
		{

		}

		public async Task Connect()
		{


			mainStream = await CreateTcp(CTS.Token);



			minecraftStream = new MinecraftStream(mainStream);
			PacketSender = new MinecraftPacketSender(minecraftStream, true);


		}
		public async Task HandShake()
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
		public async Task<Task> Login(OnPacketReceived packetReceived)
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

			Task fill = null;

			//if (Pipelines)
			{
				var readStream = pipe.Reader.AsStream();


				fill = FillPipeAsync(minecraftStream, CTS.Token);

				PacketReader = new MinecraftPacketReader(readStream, false);
				PacketReader.SwitchCompression(threshold);



			}
			//else
			//{



			//	fill = Task.CompletedTask;

			//	PacketReader = new MinecraftPacketReader(minecraftStream, false);
			//	PacketReader.SwitchCompression(threshold);
			//}
			var read = ReadPacketLoop(CTS.Token, packetReceived);

			return Task.WhenAll(read, fill);
		}

		//public static bool Pipelines { get; set; } = true;

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


		private async Task ReadPacketLoop(CancellationToken cancellationToken, OnPacketReceived packetReceived)
		{

			try
			{



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
			finally
			{
				await pipe.Reader.CompleteAsync();
			}

		}
		private async ValueTask FillPipeAsync(Stream stream, CancellationToken cancellationToken)
		{
			try
			{
				const int minimumBufferSize = 128;
				while (!cancellationToken.IsCancellationRequested)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(minimumBufferSize);
					int bytesRead = await stream.ReadAsync(memory, cancellationToken);




					pipe.Writer.Advance(bytesRead);


					FlushResult result = await pipe.Writer.FlushAsync();
					if (result.IsCompleted)
					{
						break;
					}
					if (bytesRead <= 0)
					{
						throw new EndOfStreamException();
					}
				}
			}
			finally
			{
				await pipe.Writer.CompleteAsync();
			}
		}



		private async Task<Stream> CreateTcp(CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			if (_proxy is null)
			{

				TcpClient tcp = new TcpClient();

				_logger.Information("Подключение");
				await tcp.ConnectAsync(_host, _port, token);
				return tcp.GetStream();
			}
			_logger.Information($"Подключение к {_proxy.Type} прокси {_proxy.ProxyHost}:{_proxy.ProxyPort}");




			return await _proxy.ConnectAsync(_host, _port, 5000, token);


		}




		bool _disposed;



		#region Send



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
		//private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);



		public async ValueTask SendPacket(Action<IMinecraftPrimitiveWriter> action, int id)
		{
			//await semaphore.WaitAsync();

			using (MemoryStream ms = StaticResources.MSmanager.GetStream())
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


			//semaphore.Release();

		}
		public async ValueTask SendPacketAsync(IOutputPacket packet, int id)
		{
			//await semaphore.WaitAsync();
			
				using (MemoryStream ms = StaticResources.MSmanager.GetStream())
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
		#endregion

		#region Dispose


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


			pipe = null;

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

		#endregion
	}


	public class ReadWriteStream : Stream
	{
		private readonly Stream _write;

		private readonly Stream _read;
		public ReadWriteStream(Stream write, Stream read)
		{
			_write = write;
			_read = read;
		}


		public override bool CanRead => throw new NotImplementedException();

		public override bool CanSeek => throw new NotImplementedException();

		public override bool CanWrite => throw new NotImplementedException();

		public override long Length => throw new NotImplementedException();

		public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void Flush()
		{
			_write.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _read.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_write.Write(buffer, offset, count);
		}

		public override void Write(ReadOnlySpan<byte> buffer)
		{
			_write.Write(buffer);
		}
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return _write.WriteAsync(buffer, offset, count, cancellationToken);
		}
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
		{
			return _write.WriteAsync(buffer, cancellationToken);
		}
		public override int Read(Span<byte> buffer)
		{
			return base.Read(buffer);
		}
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return _write.FlushAsync(cancellationToken);
		}
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			return _read.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return _read.ReadAsync(buffer, offset, count, cancellationToken);
		}
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
		{
			return _read.ReadAsync(buffer, cancellationToken);
		}

		public override int ReadByte()
		{
			return _read.ReadByte();
		}

		public override int ReadTimeout { get => _read.ReadTimeout; set => _read.ReadTimeout = value; }

		public override int WriteTimeout { get => _write.WriteTimeout; set => _write.WriteTimeout = value; }


		public override void WriteByte(byte value)
		{
			_write.WriteByte(value);
		}

		protected override void Dispose(bool disposing)
		{

			_read.Dispose();
			_write.Dispose();
			base.Dispose(disposing);
		}


	}
}