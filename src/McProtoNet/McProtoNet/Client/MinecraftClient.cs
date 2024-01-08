using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;
using McProtoNet.Utils;
using Serilog;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace McProtoNet
{


	public partial class MinecraftClient : IDisposable
	{
		private ILogger _logger;



		private Subject<ClientStateChanged> _state = new();
		public IObservable<ClientStateChanged> State => _state;

		public bool IsActive => _isActive == 1;

		private int _isActive = 0;

		public ClientConfig Config { get; set; } = new();

		private MinecraftVersion _protocol;

		private Pipe pipe;

		private CancellationTokenSource CTS;

		private IPacketPallete _packetPallete;

		private int _currentState = 0;


		Stream mainStream;
		private MinecraftStream minecraftStream;
		public MinecraftPacketReader PacketReader;
		public MinecraftPacketSender PacketSender;

		public MinecraftClient()
		{
			pipe = new Pipe(new PipeOptions(pauseWriterThreshold: 10, resumeWriterThreshold: 5, useSynchronizationContext: false)
			{

			});


			PacketReader = new();
			PacketSender = new();



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


		public async Task Start(Serilog.ILogger logger)
		{
			Interlocked.Exchange(ref _isActive, 1);

			_logger = logger;
			_packetPallete = CreatePallete();

			ValidateConfig();

			_protocol = Config.Version;
			try
			{
				ChangeState(ClientState.Connecting);
				await Connect();

				ChangeState(ClientState.HandShake);
				await HandShake();

				ChangeState(ClientState.Login);
				await Login();

				ChangeState(ClientState.Play);




			}
			catch (Exception e)
			{
				ErrorState(e);
				_logger.Error(e, "Во время запуска клиента произошла ошибка");
				throw e;
			}


		}

		

		public ClientState CurrentState => (ClientState)_currentState;

		private void ChangeState(ClientState state)
		{


			var old = Interlocked.Exchange(ref _currentState, (int)state);

			ClientStateChanged changed = new((ClientState)old, state);

			_state.OnNext(changed);
		}

		private void ErrorState(Exception exception)
		{
			if (Interlocked.CompareExchange(ref _isActive, 0, 1) == 1)
			{
				_state.OnError(exception);
				_state.OnCompleted();

			}
		}
		private void OnComplete()
		{
			if (Interlocked.CompareExchange(ref _isActive, 0, 1) == 1)
			{
				_state.OnCompleted();

			}
		}

		public void Disconnect()
		{
			OnComplete();

		}



		~MinecraftClient()
		{
			Dispose();
		}

		private bool _disposed = false;
		public void Dispose()
		{

			if (_disposed) return;




			_disposed = true;

			GC.SuppressFinalize(this);
		}
		public ValueTask DisposeAsync()
		{

			return ValueTask.CompletedTask;
		}


		#region Core




		private int threshold;





		


		private void Reset()
		{
			CTS.Cancel();
			minecraftStream.Dispose();
			minecraftStream = null;
		}



		private async Task<Stream> CreateTcp(CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			if (Config.Proxy is null)
			{

				TcpClient tcp = new TcpClient();

				_logger.Information("Подключение");
				await tcp.ConnectAsync(Config.Host, Config.Port, token);
				return tcp.GetStream();
			}
			_logger.Information($"Подключение к {Config.Proxy.Type} прокси {Config.Proxy.ProxyHost}:{Config.Proxy.ProxyPort}");




			return await Config.Proxy.ConnectAsync(Config.Host, Config.Port, 5000, token);


		}

		public async Task Connect()
		{
			CTS = new();
			mainStream = await CreateTcp(CTS.Token);

			minecraftStream = new MinecraftStream(mainStream);

			PacketSender.BaseStream = minecraftStream;
			PacketReader.BaseStream = minecraftStream;

		}
		public ValueTask HandShake()
		{
			
			_logger.Information("Рукопожатие");
			return PacketSender.SendPacketAsync(
					 new HandShakePacket(
						 HandShakeIntent.LOGIN,
						 (int)_protocol,
						 Config.Host,
						 Config.Port),
					 0x00, CTS.Token);


		}
		public async ValueTask Login()
		{
			CTS.Token.ThrowIfCancellationRequested();

		




			await this.SendPacket(w =>
			{
				w.WriteString(this.Config.Username);
			}, 0x00);


			await LoginCore(CTS.Token);




			var readStream = pipe.Reader.AsStream();







			var fill = FillPipeAsync();
			var read = ReadPacketLoop();

			//return Task.WhenAll(read, fill);
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


		private async Task ReadPacketLoop(CancellationToken cancellationToken)
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

								//packetReceived.Invoke(reader, packetIn, cancellationToken);
							}
							finally
							{
								Performance.Readers.Return(reader);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				//OnError?.Invoke(ex);
				//throw;
			}
			finally
			{
				await pipe.Reader.CompleteAsync();
			}

		}
		private async ValueTask FillPipeAsync()
		{
			try
			{
				const int minimumBufferSize = 128;
				while (!CTS.IsCancellationRequested)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(minimumBufferSize);
					int bytesRead = await stream.ReadAsync(memory, CTS.Token);




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
			catch (Exception ex)
			{
				//OnError?.Invoke(ex);
				//throw;
			}
			finally
			{
				await pipe.Writer.CompleteAsync();
			}
		}









		#region Send



		private ValueTask SendEncrypt(byte[] key, byte[] token)
		{
			return SendPacket(w =>
			{
				w.WriteByteArray(key);
				w.WriteByteArray(token);
			}, 0x01);
		}

		#endregion

		#endregion





	}
}