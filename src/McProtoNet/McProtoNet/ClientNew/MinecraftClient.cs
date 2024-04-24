

using DotNext.Buffers;
using McProtoNet.Core.Protocol.Pipelines;
using System.Reactive.Subjects;
using System.Threading.Channels;
using System.IO.Pipelines;
using DotNext;
using LibDeflate;

namespace McProtoNet.ClientNew
{
	internal delegate void MinecraftPacketHandler(DecompressedMinecraftPacket packet);
	public sealed class MinecraftClientContext
	{
		public ZlibCompressor Compressor { get; }
		public ZlibDecompressor Decompressor { get; }

		public IDuplexPipe Pipe { get; }

		public int CompressionThreshold { get; set; }
		public int ProtocolVersion { get; set; }
	}

	public sealed partial class MinecraftClient : Disposable
	{

		internal event MinecraftPacketHandler OnPacket;
		internal event Action OnDisposed;


		private readonly int compressionThreshold;
		private readonly int protocolVersion;
		private readonly MinecraftPacketPipeReader packetReader;
		private readonly MinecraftPacketPipeWriter packetWriter;



		public MinecraftClient(MinecraftClientContext context)
		{
			ArgumentNullException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(context.Pipe, nameof(context.Pipe));
			ArgumentOutOfRangeException.ThrowIfNegative(context.ProtocolVersion, nameof(context.ProtocolVersion));

			this.compressionThreshold = context.CompressionThreshold;
			this.protocolVersion = context.ProtocolVersion;

			packetReader = new MinecraftPacketPipeReader(context.Pipe.Input, context.Decompressor);
			packetWriter = new MinecraftPacketPipeWriter(context.Pipe.Output, context.Compressor);

			packetReader.CompressionThreshold = compressionThreshold;
			packetWriter.CompressionThreshold = compressionThreshold;
		}


		internal ValueTask SendPacket(ReadOnlyMemory<byte> data)
		{

		}

		public Task Start(CancellationToken cancellationToken)
		{
			Task receive = StartReceive(cancellationToken);

			return Task.WhenAll(receive);
		}

		private async Task StartReceive(CancellationToken cancellationToken)
		{
			await foreach (var packet in packetReader.ReadPacketsAsync(cancellationToken))
			{
				OnPacket?.Invoke(packet);
			}
		}
	}
}
