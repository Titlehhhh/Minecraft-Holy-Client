

using DotNext.Buffers;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using McProtoNet.Experimental;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace McProtoNet.ClientNew
{
	public sealed class MinecraftLogin : IMinecraftLogin
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

			using MinecraftPacketSenderNew sender = new MinecraftPacketSenderNew();
			using MinecraftPacketReaderNew reader = new MinecraftPacketReaderNew();

			using var handshake = CreateHandshake(options.Host, options.Port);
			await sender.SendPacketAsync(handshake, cancellationToken).ConfigureAwait(false);


			using var loginStart = CreateLoginStart(options.Username);
			await sender.SendPacketAsync(loginStart, cancellationToken).ConfigureAwait(false);




			return new LoginizationResult(result, 0);
		}

		private static Experimental.PacketOut CreateLoginStart(string name)
		{
			if (name.Length > 16)
			{
				throw new ArgumentOutOfRangeException();
			}

			return new Experimental.PacketOut(0, 0, null, ArrayPool<byte>.Shared);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Experimental.PacketOut CreateHandshake(string host, ushort port)
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

			return new Experimental.PacketOut(0, writer.WrittenCount, buffer, ArrayPool<byte>.Shared);
		}
	}
}
