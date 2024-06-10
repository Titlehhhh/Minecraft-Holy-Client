using LibDeflate;
using Org.BouncyCastle.Bcpg;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace McProtoNet.Protocol
{
	public sealed class MinecraftPacketSender : IDisposable
	{

		public Stream BaseStream { get; set; }




		private readonly ZlibCompressor compressor = new ZlibCompressor(6);

		private const int ZERO_VARLENGTH = 1;
		private static readonly byte[] ZERO_VARINT = { 0 };

		public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> data, CancellationToken token = default)
		{


			try
			{


				if (_compressionThreshold > 0)
				{


					int uncompressedSize = data.Length;

					if (uncompressedSize >= _compressionThreshold)
					{
						int length = compressor.GetBound(uncompressedSize);

						var compressedBuffer = ArrayPool<byte>.Shared.Rent(length);

						try
						{
							int bytesCompress = compressor.Compress(data.Span, compressedBuffer.AsSpan(0, length));

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
						await BaseStream.WriteAsync(data, token).ConfigureAwait(false);
					}

				}
				else
				{
					await SendPacketWithoutCompressionAsync(data, token).ConfigureAwait(false);
				}
				await BaseStream.FlushAsync(token);
			}
			finally
			{

			}
		}

		#region Send        
		public ValueTask SendPacketAsync(OutputPacket packet, CancellationToken token = default)
		{
			return SendPacketAsync(packet.Memory, token);


		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private async ValueTask SendPacketWithoutCompressionAsync(ReadOnlyMemory<byte> data, CancellationToken token)
		{

			int len = data.Length;

			await BaseStream.WriteVarIntAsync(len, token).ConfigureAwait(false);

			await BaseStream.WriteAsync(data, token).ConfigureAwait(false);



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
