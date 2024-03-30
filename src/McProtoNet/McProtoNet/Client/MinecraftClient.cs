using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Protocol;
using McProtoNet.Utils;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Net.Sockets;

namespace McProtoNet
{


	public partial class MinecraftClient : IDisposable
	{

		#region Fields
		private ILogger _logger;

		private Pipe pipe;

		private CancellationTokenSource CTS;

		private IPacketPallete _packetPallete;

		private int _currentState = 0;


		private Stream mainStream;
		private MinecraftStream minecraftStream;
		private MinecraftPacketReader PacketReader;
		private MinecraftPacketSender PacketSender;



		private int _isActive = 0;
		private MinecraftVersion _protocol;
		#endregion
		#region Properties
		public event Action<ClientStateChanged> OnStateChanged;
		public event Action<Exception> OnErrored;

		public ClientState CurrentState => (ClientState)_currentState;

		public bool IsActive => _isActive == 1;


		public ClientConfig Config { get; set; } = new();
		#endregion









		public MinecraftClient()
		{
			pipe = new Pipe(new PipeOptions(pauseWriterThreshold: 10, resumeWriterThreshold: 5, useSynchronizationContext: false)
			{

			});



			PacketReader = new();
			PacketSender = new();



			CreateEvents();

		}






		private IPacketPallete CreatePallete()
		{
			IPacketPallete? packetPallete = null;
			if (Config.Version <= MinecraftVersion.MC_1_12_2_Version)
				packetPallete = PacketPalette_1_12_2.Instance;
			else if (Config.Version < MinecraftVersion.MC_1_14_Version)
				packetPallete = PacketPalette_1_13.Instance;
			else if (Config.Version <= MinecraftVersion.MC_1_15_Version)
				packetPallete = PacketPalette_1_14.Instance;
			else if (Config.Version <= MinecraftVersion.MC_1_15_2_Version)
				packetPallete = PacketPalette_1_15.Instance;
			else if (Config.Version <= MinecraftVersion.MC_1_16_1_Version)
			{
				packetPallete = PacketPalette_1_16.Instance;
			}
			else if (Config.Version <= MinecraftVersion.MC_1_16_5_Version)
				packetPallete = PacketPalette_1_16_2.Instance;
			else if (Config.Version <= MinecraftVersion.MC_1_17_1_Version)
				packetPallete = PacketPalette_1_17.Instance;
			else if (Config.Version <= MinecraftVersion.MC_1_18_2_Version)
				packetPallete = PacketPalette_1_18.Instance;
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


		private void ResetFields()
		{
			if (CTS is not null)
				if (!CTS.IsCancellationRequested)
				{
					CTS.Cancel();
				}
			CTS = new();
			_canceling = 0;
			_isActive = 1;
			_currentState = 0;
			_errorInvoked = 0;

			PacketReader.SwitchCompression(0);
			PacketSender.SwitchCompression(0);

			PacketSender.BaseStream = null;
			PacketReader.BaseStream = null;
		}

		public async Task Start(Serilog.ILogger logger)
		{
			ResetFields();

			_logger = logger;
			_packetPallete = CreatePallete();

			ValidateConfig();

			_protocol = Config.Version;
			bool runLoop = false;
			try
			{
				ChangeState(ClientState.Connecting);
				await Connect();

				ChangeState(ClientState.HandShake);
				await HandShake();

				ChangeState(ClientState.Login);

				await this.SendPacket(w =>
				{
					w.WriteString(this.Config.Username);
				}, 0x00);


				await LoginCore(CTS.Token);



				var readStream = pipe.Reader.AsStream();
				PacketReader.BaseStream = readStream;



				var fill = FillPipeAsync();
				var read = ReadPacketLoop();
				var combined = Task.WhenAll(fill, read);

				_ = combined.ContinueWith(t =>
				{
					try
					{
						if (t.IsFaulted)
						{
							CancelAll(t.Exception);
						}
						else
						{
							CancelAll(new TaskCanceledException());
						}
						this.pipe.Reset();
						this.InvokeError();
					}
					catch
					{

					}
					finally
					{

					}


				});
				runLoop = true;


			}
			catch (Exception e)
			{

				CancelAll(e);
				if (!runLoop)
				{
					InvokeError();
				}
				_logger.Error(e, "Во время запуска клиента произошла ошибка");
				//throw e;
			}


		}





		private void ChangeState(ClientState state)
		{

			var old = Interlocked.Exchange(ref _currentState, (int)state);

			ClientStateChanged changed = new((ClientState)old, state);

			OnStateChanged?.Invoke(changed);
		}
		private int _canceling = 0;
		private Exception _error;
		private void CancelAll(Exception exception)
		{
			if (Interlocked.CompareExchange(ref _canceling, 1, 0) == 0)
			{
				_error = exception;

				CTS?.Cancel();
				CTS?.Dispose();
				CTS = null;



				tcpClient?.Dispose();
				tcpClient = null;

				this.mainStream?.Dispose();
				this.mainStream = null;

				minecraftStream?.Dispose();
				minecraftStream = null;




			}
		}
		private int _errorInvoked = 0;
		private void InvokeError()
		{
			if (this._error is not null)
			{
				OnErrored?.Invoke(this._error);
				_error = null;
			}
		}

		public void Disconnect()
		{
			this.mainStream?.Dispose();
			this.mainStream = null;
			minecraftStream?.Dispose();
			minecraftStream = null;


		}

		public void Disconnect(Exception exception)
		{
			CancelAll(exception);
			InvokeError();
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


			if (CTS is not null)
			{

				CTS.Dispose();
				CTS = null;
			}

			tcpClient?.Dispose();
			tcpClient = null;
			semaphore?.Dispose();
			semaphore = null;
			mainStream?.Dispose();
			mainStream = null;

			minecraftStream?.Dispose();
			minecraftStream = null;

			//PacketSender?.Dispose();
			//PacketSender = null;

			try
			{
				pipe.Reader.Complete();
				pipe.Writer.Complete();
				pipe.Reset();
			}
			catch
			{

			}

			_events.Dispose();
			pipe = null;


			GC.SuppressFinalize(this);
		}



		#region Core




		//private int threshold;










		private TcpClient tcpClient;

		private async ValueTask<Stream> CreateTcp(CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			if (Config.Proxy is null)
			{

				tcpClient = new TcpClient();

				_logger.Information("Подключение");
				await tcpClient.ConnectAsync(Config.Host, Config.Port, token);
				return tcpClient.GetStream();
			}
			_logger.Information($"Подключение к {Config.Proxy.Type} прокси {Config.Proxy.ProxyHost}:{Config.Proxy.ProxyPort}");


			tcpClient = new();
			try
			{

				await tcpClient.ConnectAsync(Config.Proxy.ProxyHost, Config.Proxy.ProxyPort, token);

				var tcpStream = tcpClient.GetStream();

				using (var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
				{

					using var combined = CancellationTokenSource.CreateLinkedTokenSource(timeout.Token, token);

					using var reg = combined.Token.Register(() => tcpStream.Dispose());

					var stream = await Config.Proxy.ConnectAsync(
						tcpStream,
						Config.Host,
						Config.Port,
						combined.Token);

					return stream;
				}

			}
			catch
			{
				tcpClient.Dispose();
				throw;
			}


		}

		public async ValueTask Connect()
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
			return SendPacketAsync(
					 new HandShakePacket(
						 HandShakeIntent.LOGIN,
						 (int)_protocol,
						 Config.HandshakeHost ?? Config.Host,
						 Config.HandshakePort ?? Config.Port),
					 0x00);


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
				ChangeState(ClientState.Play);
				_logger.Information("Переход в Play режим");
				return true;
			}
			else if (id == 0x03)
			{
				var threshold = reader.ReadVarInt();
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
				throw new Exception("Unknown packet: " + id);
			}

			return false;
		}

		private async Task ReadPacketLoop()
		{

			try
			{
				while (!CTS.IsCancellationRequested)
				{
					using (var packet = await PacketReader.ReadNextPacketAsync(CTS.Token))
					{
						if (_packetPallete.TryGetIn(packet.Id, out var packetIn))
						{
							packet.Data.Position = 0;

							var reader = Performance.Readers.Get();

							try
							{



								reader.BaseStream = packet.Data;
								OnPacket(reader, packetIn);

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

				await pipe.Reader.CompleteAsync(ex);


				CancelAll(ex);

				throw;
				//OnError?.Invoke(ex);
				//throw;
			}
			finally
			{

			}

			await pipe.Reader.CompleteAsync();


		}
		private async Task FillPipeAsync()
		{
			try
			{
				const int minimumBufferSize = 128;
				while (!CTS.IsCancellationRequested)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(minimumBufferSize);
					int bytesRead = await minecraftStream.ReadAsync(memory, CTS.Token);




					pipe.Writer.Advance(bytesRead);


					FlushResult result = await pipe.Writer.FlushAsync(CTS.Token);
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

				await pipe.Writer.CompleteAsync(ex);

				CancelAll(ex);
				throw;
			}
			finally
			{

			}

			await pipe.Writer.CompleteAsync();

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