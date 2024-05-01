using DotNext;
using DotNext.Threading;
using LibDeflate;
using McProtoNet.Protocol;
using System.IO.Pipelines;

namespace McProtoNet.Client
{

	internal delegate void MinecraftPacketHandler(InputPacket packet);
	public sealed class ClientContext
	{
		public ZlibCompressor Compressor { get; }
		public ZlibDecompressor Decompressor { get; }

		public IDuplexPipe Pipe { get; }

		public int CompressionThreshold { get; set; }
		public int ProtocolVersion { get; set; }
	}



	public sealed partial class Client : Disposable
	{

		internal event MinecraftPacketHandler OnPacket;
		internal event Action OnDisposed;


		private readonly int compressionThreshold;
		private readonly int protocolVersion;

		private readonly IDuplexPipe duplexPipe;

		public Client(ClientContext context)
		{
			ArgumentNullException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(context.Pipe, nameof(context.Pipe));
			ArgumentOutOfRangeException.ThrowIfNegative(context.ProtocolVersion, nameof(context.ProtocolVersion));


			this.duplexPipe = context.Pipe;
			this.compressionThreshold = context.CompressionThreshold;
			this.protocolVersion = context.ProtocolVersion;


		}

		private AsyncLock sendLock = new AsyncLock();
		private CancellationTokenSource internalCts;
		internal async ValueTask SendPacket(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
		{

		}

		public Task Start(CancellationToken cancellationToken)
		{
			return StartReceive(cancellationToken);
		}

		private async Task StartReceive(CancellationToken cancellationToken)
		{
			try
			{
				//await foreach (var packet in packetReader.ReadPacketsAsync(cancellationToken))
				{
					//OnPacket?.Invoke(packet);
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
