

using DotNext.Buffers;
using McProtoNet.Cryptography;
using McProtoNet.Protocol;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace McProtoNet.Client
{
	public sealed class MinecraftLogin
	{
		private readonly static byte[] VarIntLoginIntent;
		static MinecraftLogin()
		{
			MemoryStream ms = new MemoryStream();
			ms.WriteVarInt(2);
			VarIntLoginIntent = ms.ToArray();
		}


		public async Task<LoginizationResult> Login(Stream source, LoginOptions options, CancellationToken cancellationToken = default)
		{
			AesStream result = new AesStream(source);

			using MinecraftPacketSender sender = new MinecraftPacketSender();
			using MinecraftPacketReader reader = new MinecraftPacketReader();

			using var handshake = CreateHandshake(options.Host, options.Port);
			await sender.SendPacketAsync(handshake, cancellationToken).ConfigureAwait(false);


			using var loginStart = CreateLoginStart(options.Username);
			await sender.SendPacketAsync(loginStart, cancellationToken).ConfigureAwait(false);




			return new LoginizationResult(result, 0);
		}

		private static OutputPacket CreateLoginStart(string name)
		{
			if (name.Length > 16)
			{
				throw new ArgumentOutOfRangeException();
			}
			throw null;
			//return new OutputPacket(0, 0, null, ArrayPool<byte>.Shared);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static OutputPacket CreateHandshake(string host, ushort port)
		{
			if (host.Length > 255)
			{
				throw new ArgumentOutOfRangeException();
			}
			int count = Encoding.UTF8.GetByteCount(host);
			count = 2 + count + 2 + VarIntLoginIntent.Length;

			byte[] buffer = ArrayPool<byte>.Shared.Rent(count);

			scoped BufferWriterSlim<byte> writer = new BufferWriterSlim<byte>(buffer);
			// WriteProtocolVersion Todo
			writer.WriteBigEndian(port);
			throw null;
			//return new OutputPacket(0, writer.WrittenCount, buffer, ArrayPool<byte>.Shared);
		}

		public class LoginizationResult
		{
			private AesStream result;
			private int v;

			public LoginizationResult(AesStream result, int v)
			{
				this.result = result;
				this.v = v;
			}
		}
	}
}
