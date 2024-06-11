using DotNext;
using DotNext.Threading;
using LibDeflate;
using Org.BouncyCastle.Bcpg;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Reactive.Subjects;


namespace McProtoNet.Protocol
{
	public delegate void PacketHandler(object sender, InputPacket inputPacket);
	internal sealed class PacketPipeHandler : Disposable
	{
		public PacketHandler PacketReceived;
		private int compressionThreshold;
		private readonly IDuplexPipe duplexPipe;
		private readonly MinecraftPacketPipeReader reader;
		private readonly MinecraftPacketPipeWriter writer;


		public int CompressionThreshold
		{
			get => compressionThreshold; set
			{
				reader.CompressionThreshold = value;
				writer.CompressionThreshold = value;
				compressionThreshold = value;
			}
		}


		public PacketPipeHandler(IDuplexPipe duplexPipe, ZlibCompressor compressor, ZlibDecompressor decompressor)
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
				await foreach (InputPacket packet in reader.ReadPacketsAsync(cancellationToken))
				{
					PacketReceived?.Invoke(this, packet);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("PacketPipeHandler Exception " + ex);
				duplexPipe.Output.CancelPendingFlush();
			}
			finally
			{
				Debug.WriteLine("PacketPipeHandler stop");
				await duplexPipe.Output.CompleteAsync();
			}

		}




		public void Complete()
		{
			duplexPipe.Output.Complete();
			duplexPipe.Input.Complete();
		}


	}

}