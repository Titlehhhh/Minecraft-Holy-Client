using DotNext;
using DotNext.Threading;
using LibDeflate;
using Org.BouncyCastle.Bcpg;
using System.IO.Pipelines;
using System.Reactive.Subjects;


namespace McProtoNet.Protocol
{
	internal sealed class PacketPipeHandler : Disposable
	{

		private readonly IDuplexPipe duplexPipe;
		private readonly MinecraftPacketPipeReader reader;
		private readonly MinecraftPacketPipeWriter writer;

		private readonly ZlibCompressor compressor;
		private readonly ZlibDecompressor decompressor;




		public PacketPipeHandler(IDuplexPipe duplexPipe)
		{
			this.duplexPipe = duplexPipe;

			reader = new MinecraftPacketPipeReader(duplexPipe.Input, decompressor);
			writer = new MinecraftPacketPipeWriter(duplexPipe.Output, compressor);
		}

		public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
		{
			try
			{
				await writer.SendPacketAsync(data, cancellationToken);
			}
			catch (OperationCanceledException)
			{

			}
			catch (Exception ex)
			{
				await duplexPipe.Input.CompleteAsync(ex);
			}
		}



		public async Task StartAsync(CancellationToken cancellationToken)
		{
			try
			{
				await foreach (var packet in reader.ReadPacketsAsync(cancellationToken))
				{

				}
			}
			catch (Exception ex)
			{
				duplexPipe.Output.CancelPendingFlush();
			}
			finally
			{
				await duplexPipe.Output.CompleteAsync();
			}
		}




		public void Complete()
		{

		}


	}

}