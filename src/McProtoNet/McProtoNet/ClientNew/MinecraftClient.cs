

using DotNext.Buffers;
using McProtoNet.Core.Protocol.Pipelines;
using System.Reactive.Subjects;
using System.Threading.Channels;
using System.IO.Pipelines;
using DotNext;
using LibDeflate;
using DotNext.Threading;

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

		private readonly IDuplexPipe duplexPipe;

		public MinecraftClient(MinecraftClientContext context)
		{
			ArgumentNullException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(context.Pipe, nameof(context.Pipe));
			ArgumentOutOfRangeException.ThrowIfNegative(context.ProtocolVersion, nameof(context.ProtocolVersion));


			this.duplexPipe = context.Pipe;
			this.compressionThreshold = context.CompressionThreshold;
			this.protocolVersion = context.ProtocolVersion;

			packetReader = new MinecraftPacketPipeReader(context.Pipe.Input, context.Decompressor);
			packetWriter = new MinecraftPacketPipeWriter(context.Pipe.Output, context.Compressor);

			packetReader.CompressionThreshold = compressionThreshold;
			packetWriter.CompressionThreshold = compressionThreshold;
		}

		private AsyncLock sendLock = new AsyncLock();
		private CancellationTokenSource internalCts;
		internal async ValueTask SendPacket(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
		{

			var holder = await sendLock.AcquireAsync(cancellationToken);
			try
			{
				await packetWriter.SendPacketAsync(data, cancellationToken);
			}
			catch (Exception ex)
			{
				await duplexPipe.Input.CompleteAsync(ex).ConfigureAwait(false);
				//duplexPipe.Input.CancelPendingRead();
				throw;
			}
			finally
			{
				holder.Dispose();
			}
		}

		public Task Start(CancellationToken cancellationToken)
		{
			return StartReceive(cancellationToken);
		}

		private async Task StartReceive(CancellationToken cancellationToken)
		{
			try
			{
				await foreach (var packet in packetReader.ReadPacketsAsync(cancellationToken))
				{
					OnPacket?.Invoke(packet);
				}
			}
			catch (Exception ex)
			{
				await duplexPipe.Output.CompleteAsync(ex).ConfigureAwait(false);
				throw;
			}
		}

		protected override void Dispose(bool disposing)
		{

			sendLock.Dispose();


			OnDisposed?.Invoke();
			base.Dispose(disposing);
		}
	}
}
