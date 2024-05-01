using DotNext;
using DotNext.Threading;
using LibDeflate;
using System.IO.Pipelines;
using System.Reactive.Subjects;


namespace McProtoNet.Protocol
{
	public sealed class MinecraftProtocolPipeHandler : Disposable
	{

		public IObservable<InputPacket> OnPacket => onPacket;

		private readonly IDuplexPipe duplexPipe;
		private readonly MinecraftPacketPipeReader reader;
		private readonly MinecraftPacketPipeWriter writer;

		private readonly ZlibCompressor compressor = new(4);
		private readonly ZlibDecompressor decompressor = new();

		private readonly AsyncLock asyncLock = new AsyncLock();



		private readonly Subject<InputPacket> onPacket = new();
		private CancellationTokenSource cts = new();

		private int compressionThreshold;
		public int CompressionThreshold
		{
			get => compressionThreshold;
			set
			{
				compressionThreshold = value;
				reader.CompressionThreshold = value;
				writer.CompressionThreshold = value;
			}
		}


		public MinecraftProtocolPipeHandler(IDuplexPipe duplexPipe)
		{
			this.duplexPipe = duplexPipe;

			reader = new MinecraftPacketPipeReader(duplexPipe.Input, decompressor);
			writer = new MinecraftPacketPipeWriter(duplexPipe.Output, compressor);
		}

		public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> data)
		{

			if (state.Value != PipeHandlerState.Listening)
			{
				throw new InvalidOperationException("Invalid state");
			}


			var l = await asyncLock.AcquireAsync(cts.Token);

			try
			{
				await writer.SendPacketAsync(data, cts.Token);
			}
			catch (OperationCanceledException)
			{

			}
			catch (Exception ex)
			{
				await duplexPipe.Input.CompleteAsync(ex);
			}
			finally
			{
				l.Dispose();
			}
		}

		private Atomic<PipeHandlerState> state = new Atomic<PipeHandlerState>();

		public PipeHandlerState State => state.Value;

		private Task listen;

		public Task StartListenAsync()
		{
			if (state.CompareAndSet(PipeHandlerState.None, PipeHandlerState.Listening))
			{
				cts = new CancellationTokenSource();
				listen = DoStartListen();
				return listen;
			}
			throw new InvalidOperationException("Invalid State");
		}

		private async Task DoStartListen()
		{
			try
			{
				await foreach (var packet in reader.ReadPacketsAsync(cts.Token))
				{
					onPacket.OnNext(packet);
				}
			}
			catch (Exception ex)
			{
				//isActive.Write(0);
				duplexPipe.Output.CancelPendingFlush();
				onPacket.OnError(ex);
			}
			finally
			{
				await duplexPipe.Output.CompleteAsync();

				state.Write(PipeHandlerState.None);

				onPacket.OnCompleted();
			}
		}



		public Task Stop()
		{
			if (state.CompareAndSet(PipeHandlerState.Listening, PipeHandlerState.Stopping))
			{
				return DoStop();
			}
			return Task.CompletedTask;
		}

		private async Task DoStop()
		{
			cts.Cancel();
			await listen;
			cts.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			cts.Dispose();
			compressor.Dispose();
			decompressor.Dispose();
			asyncLock.Dispose();
			onPacket.Dispose();
			base.Dispose(disposing);
		}
	}

	public enum PipeHandlerState
	{
		None,
		Listening,
		Stopping
	}
}