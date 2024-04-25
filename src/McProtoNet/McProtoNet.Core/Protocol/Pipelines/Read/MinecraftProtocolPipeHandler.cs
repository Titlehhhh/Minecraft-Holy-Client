using System.IO.Pipelines;
using LibDeflate;
using DotNext.Threading;
using System.Reactive.Subjects;
using DotNext;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public sealed class MinecraftProtocolPipeHandler : Disposable
	{

		public IObserver<DecompressedMinecraftPacket> OnPacket => onPacket;

		private readonly IDuplexPipe duplexPipe;
		private readonly MinecraftPacketPipeReader reader;
		private readonly MinecraftPacketPipeWriter writer;

		private readonly ZlibCompressor compressor = new(4);
		private readonly ZlibDecompressor decompressor = new();

		private readonly AsyncLock asyncLock = new AsyncLock();



		private readonly Subject<DecompressedMinecraftPacket> onPacket = new();
		private readonly CancellationTokenSource cts = new();


		public MinecraftProtocolPipeHandler(IDuplexPipe duplexPipe)
		{
			this.duplexPipe = duplexPipe;

			reader = new MinecraftPacketPipeReader(duplexPipe.Input, decompressor);
			writer = new MinecraftPacketPipeWriter(duplexPipe.Output, compressor);
		}

		public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> data)
		{
			var l = await asyncLock.AcquireAsync(cts.Token);

			try
			{
				await writer.SendPacketAsync(data, cts.Token);
			}
			finally
			{
				l.Dispose();
			}
		}
		public async Task StartListenAsync()
		{
			bool completed = false;
			try
			{
				await foreach (var packet in reader.ReadPacketsAsync(cts.Token))
				{
					onPacket.OnNext(packet);
				}
			}
			catch (Exception ex)
			{
				onPacket.OnError(ex);
			}
			finally
			{
				onPacket.OnCompleted();
			}


			

		}

		public void Stop()
		{
			cts.Cancel();
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

}