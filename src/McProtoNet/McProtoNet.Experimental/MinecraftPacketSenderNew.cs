using System.Buffers;
using System.Runtime.CompilerServices;
using LibDeflate;
using McProtoNet.Core;

namespace McProtoNet.Experimental
{
	public sealed class MinecraftPacketSenderNew : IDisposable
	{

		public Stream BaseStream { get; set; }




		private readonly ZlibCompressor compressor = new ZlibCompressor(6);

		private const int ZERO_VARLENGTH = 1;
		private static readonly byte[] ZERO_VARINT = { 0 };


		#region Send        
		public async ValueTask SendPacketAsync(PacketOut packet, CancellationToken token = default)
		{

			//ThrowIfDisposed();
			//int id = packet.Id;
			//var data = packet.Data;
			//data.Position = 0;

			try
			{


				if (_compressionThreshold > 0)
				{


					int uncompressedSize = packet.GetMemory().Length;

					if (uncompressedSize >= _compressionThreshold)
					{
						int length = compressor.GetBound(uncompressedSize);

						var compressedBuffer = ArrayPool<byte>.Shared.Rent(length);

						try
						{
							int bytesCompress = compressor.Compress(packet.GetMemory().Span, compressedBuffer.AsSpan(0, length));

							int compressedLength = bytesCompress;

							int fullsize = compressedLength + uncompressedSize.GetVarIntLength();

							await BaseStream.WriteVarIntAsync(fullsize, token).ConfigureAwait(false);
							await BaseStream.WriteVarIntAsync(uncompressedSize, token).ConfigureAwait(false);
							//await BaseStream.WriteAsync(compressed.Memory, token);

							await BaseStream.WriteAsync(compressedBuffer.AsMemory(0, bytesCompress), token).ConfigureAwait(false);
						}
						finally
						{
							ArrayPool<byte>.Shared.Return(compressedBuffer);
						}



					}
					else
					{
						uncompressedSize++;

						await BaseStream.WriteVarIntAsync(uncompressedSize, token).ConfigureAwait(false);
						await BaseStream.WriteAsync(ZERO_VARINT, token).ConfigureAwait(false);
						await BaseStream.WriteAsync(packet.GetMemory(), token).ConfigureAwait(false);



					}

				}
				else
				{
					await SendPacketWithoutCompressionAsync(packet, token).ConfigureAwait(false);
				}
				await BaseStream.FlushAsync(token);
			}
			finally
			{
				//semaphore.Release();
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private async ValueTask SendPacketWithoutCompressionAsync(PacketOut packet, CancellationToken token)
		{

			int len = packet.GetMemory().Length;

			await BaseStream.WriteVarIntAsync(len, token).ConfigureAwait(false);

			await BaseStream.WriteAsync(packet.GetMemory(), token).ConfigureAwait(false);

			

		}
		#endregion

		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}

		public void Dispose()
		{
			compressor.Dispose();
		}
	}
}
