

using DotNext.Buffers;
using McProtoNet.Cryptography;
using McProtoNet.Protocol;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace McProtoNet.Client
{
	public sealed class MinecraftClientLogin
	{
		private readonly static byte[] VarIntLoginIntent;
		private readonly static byte[] LoginAcknowledged;

		private static MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

		public event Action<MinecraftClientState> StateChanged;
		static MinecraftClientLogin()
		{
			MemoryStream ms = new MemoryStream();
			ms.WriteVarInt(2);
			VarIntLoginIntent = ms.ToArray();
			ms.Position = 0;
			ms.SetLength(0);

			ms.WriteVarInt(0x03);
			LoginAcknowledged = ms.ToArray();

		}


		public async Task<LoginizationResult> Login(Stream source, LoginOptions options, CancellationToken cancellationToken = default)
		{
			AesStream mainStream = new AesStream(source);

			using MinecraftPacketSender sender = new MinecraftPacketSender();
			using MinecraftPacketReader reader = new MinecraftPacketReader();

			sender.BaseStream = mainStream;
			reader.BaseStream = mainStream;

			using var handshake = CreateHandshake(options.Host, options.Port, options.ProtocolVersion);

			StateChanged?.Invoke(MinecraftClientState.Handshaking);
			await sender.SendPacketAsync(handshake, cancellationToken).ConfigureAwait(false);


			using var loginStart = CreateLoginStart(options.Username, options);

			StateChanged?.Invoke(MinecraftClientState.Login);
			await sender.SendPacketAsync(loginStart, cancellationToken).ConfigureAwait(false);

			int threshold = 0;


			while (true)
			{
				InputPacket inputPacket = await reader.ReadNextPacketAsync().ConfigureAwait(false);

				bool needBreak = false;

				Console.WriteLine("ReadLoging: " + inputPacket.Id);

				switch (inputPacket.Id)
				{
					case 0x00:
						inputPacket.Data.TryReadString(out string reason, out _);
						throw new LoginRejectedException(reason);
						break;
					case 0x01:
						var encryptBegin = ReadEncryptionPacket(inputPacket);

						var RSAService = CryptoHandler.DecodeRSAPublicKey(encryptBegin.PublicKey);
						var secretKey = CryptoHandler.GenerateAESPrivateKey();


						byte[] sharedSecret = RSAService.Encrypt(secretKey, false);
						byte[] verifyToken = RSAService.Encrypt(encryptBegin.VerifyToken, false);

						using (var response = CreateEncryptionResponse(sharedSecret, verifyToken))
						{
							await sender.SendPacketAsync(response, cancellationToken);
						}

						mainStream.SwitchEncryption(secretKey);

						break;
					case 0x02:
						needBreak = true;

						if (options.ProtocolVersion == 765)
						{
							await sender.SendPacketAsync(LoginAcknowledged, cancellationToken);
						}

						break;					
					case 0x03:
						//Compress

						if (!inputPacket.Data.TryReadVarInt(out threshold, out _))
							throw new Exception("asd");
						reader.SwitchCompression(threshold);
						sender.SwitchCompression(threshold);

						Debug.WriteLine("Compress: " + threshold);
						break;
					//case 0x04:
					//	//Login plugin request
					//	Debug.WriteLine("Plugin");
					//	ReadOnlySequence<byte> buffer = inputPacket.Data;
					//	int offset = 0;
					//	buffer.TryReadVarInt(out int messageId, out offset);
					//	buffer = buffer.Slice(offset);
					//	buffer.TryReadString(out string channel, out offset);
					//	ReadOnlySequence<byte> data = buffer.Slice(offset);
					//	break;

					default: throw new Exception("Unknown packet: " + inputPacket.Id);
				}

				if (needBreak)
					break;
			}


			while (true)
			{
				InputPacket inputPacket = await reader.ReadNextPacketAsync().ConfigureAwait(false);

				
				Console.WriteLine("ReadConfig: " + inputPacket.Id);
			}



			return new LoginizationResult(mainStream, threshold);
		}


		private static OutputPacket CreateLoginAcknowledged()
		{
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(1);

			try
			{
				writer.WriteVarInt(0x03);


				writer.TryDetachBuffer(out MemoryOwner<byte> buffer);
				return new OutputPacket(buffer);
			}
			finally
			{
				writer.Dispose();
			}
		}

		private static EncryptionBeginPacket ReadEncryptionPacket(InputPacket inputPacket)
		{
			scoped SequenceReader<byte> reader = new SequenceReader<byte>(inputPacket.Data);

			reader.TryReadString(out string serverId);

			reader.TryReadVarInt(out int len, out _);

			byte[] publicKey = reader.UnreadSequence.Slice(0, len).ToArray();
			reader.Advance(len);

			reader.TryReadVarInt(out len, out _);
			byte[] verifyToken = reader.UnreadSequence.Slice(0, len).ToArray();


			return new EncryptionBeginPacket(serverId, publicKey, verifyToken);
		}

		private static OutputPacket CreateEncryptionResponse(byte[] sharedSecret, byte[] verifyToken)
		{
			int length = sharedSecret.Length + verifyToken.Length + 4;
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(length, s_allocator);

			try
			{
				writer.WriteVarInt(0x01);
				writer.WriteBuffer(sharedSecret);
				writer.WriteBuffer(verifyToken);


				writer.TryDetachBuffer(out MemoryOwner<byte> buffer);
				return new OutputPacket(buffer);
			}
			finally
			{
				writer.Dispose();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutputPacket CreateLoginStart(string name, LoginOptions options)
		{
			if (name.Length > 16)
			{
				throw new ArgumentOutOfRangeException();
			}
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(10, s_allocator);

			try
			{
				writer.WriteVarInt(0x00); //Packet Id
				writer.WriteString(name);

				if (options.ProtocolVersion == 765)
				{
					Guid guid = Guid.NewGuid();

					byte[] data = guid.ToByteArray();

					writer.Write(data);
				}

				writer.TryDetachBuffer(out MemoryOwner<byte> buffer);
				return new OutputPacket(buffer);
			}
			finally
			{
				writer.Dispose();
			}

		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutputPacket CreateHandshake(string host, ushort port, int version)
		{
			if (host.Length > 255)
			{
				throw new ArgumentOutOfRangeException();
			}
			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(10, s_allocator);
			try
			{
				writer.WriteVarInt(0x00); //Packet Id

				writer.WriteVarInt(version);
				writer.WriteString(host);
				writer.WriteBigEndian(port);
				writer.Write(VarIntLoginIntent);
				writer.TryDetachBuffer(out MemoryOwner<byte> owner);
				return new OutputPacket(owner);
			}
			finally
			{
				writer.Dispose();
			}
		}

		internal readonly struct EncryptionBeginPacket
		{
			public readonly string ServerId;
			public readonly byte[] PublicKey;
			public readonly byte[] VerifyToken;

			public EncryptionBeginPacket(string serverId, byte[] publicKey, byte[] verifyToken)
			{
				ServerId = serverId;
				PublicKey = publicKey;
				VerifyToken = verifyToken;
			}
		}
	}

	public class LoginizationResult
	{
		public readonly Stream Stream;
		public readonly int CompressionThreshold;

		public LoginizationResult(Stream stream, int compressionThreshold)
		{
			Stream = stream;
			CompressionThreshold = compressionThreshold;
		}
	}
}
