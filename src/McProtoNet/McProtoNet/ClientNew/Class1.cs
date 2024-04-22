

using DotNext.Buffers;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using McProtoNet.Experimental;
using System.Buffers;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;

namespace McProtoNet.ClientNew
{
	public interface IPacket
	{
		internal void Write();
		internal void Read();
	}

	public interface IMinecraftClient : IDisposable
	{
		public IObservable<IPacket> OnPacket { get; }

		public ValueTask SendPacketAsync(IPacket packet, CancellationToken token);

		public Task StartAsync(CancellationToken cancellationToken = default);

	}
	public interface IMinecraftLogin
	{
		public Task<LoginizationResult> Login(Stream source, LoginOptions options, CancellationToken cancellationToken = default);
	}

	public readonly struct LoginizationResult
	{
		public readonly Stream TransportStream;
		public readonly int CompressionThreshold;

		public LoginizationResult(Stream transportStream, int compressionThreshold)
		{
			TransportStream = transportStream;
			CompressionThreshold = compressionThreshold;
		}
	}
	public readonly struct LoginOptions
	{
		public readonly string Host;
		public readonly ushort Port;
		public readonly int ProtocolVersion;
		public readonly string Username;
	}
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
	public readonly struct MinecraftClientOptions
	{
		public readonly int ProtocolVersion;
		public readonly int Username;
	}
	public sealed class MinecraftClientFactory
	{

		public IMinecraftClient Create(MinecraftClientOptions options)
		{
			if(options.ProtocolVersion == 754)
			{
				
			}
			throw new NotImplementedException();
		}
	}


	public abstract class MinecraftClient : IMinecraftClient
	{
		private protected Subject<IPacket> onPacket = new Subject<IPacket>();
		public IObservable<IPacket> OnPacket => onPacket;


		public MinecraftClient(Stream stream)
		{

		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public ValueTask SendPacketAsync(IPacket packet, CancellationToken token)
		{
			throw new NotImplementedException();
		}

		public Task StartAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
