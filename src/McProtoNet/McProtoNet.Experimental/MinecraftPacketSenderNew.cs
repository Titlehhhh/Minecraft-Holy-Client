using System.Runtime.CompilerServices;
using LibDeflate;
using McProtoNet.Core;

namespace McProtoNet.Experimental
{
	public class MinecraftPacketSenderNew
	{

		public Stream BaseStream { get; set; }



		//private static ArrayPool<byte> VarIntPool = ArrayPool<byte>.Create(10, 20);

		//private readonly Deflater Deflater = new Deflater();

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



						using (var compressed = compressor.Compress(packet.GetMemory().Span, true))
						{

							int compressedLength = compressed.Memory.Length;

							int fullsize = compressedLength + uncompressedSize.GetVarIntLength();

							await BaseStream.WriteVarIntAsync(fullsize, token);
							await BaseStream.WriteVarIntAsync(uncompressedSize, token);
							await BaseStream.WriteAsync(compressed.Memory, token);
						}

					}
					else
					{
						uncompressedSize++;

						await BaseStream.WriteVarIntAsync(uncompressedSize, token);
						await BaseStream.WriteAsync(ZERO_VARINT, token);
						await BaseStream.WriteAsync(packet.GetMemory(), token);



					}

				}
				else
				{
					await SendPacketWithoutCompressionAsync(packet, token);
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

			await BaseStream.WriteVarIntAsync(len, token);

			await BaseStream.WriteAsync(packet.GetMemory(), token);

			//int Packetlength = (int)packet.Length;

			//var buffer = VarIntPool.Rent(10);



			//byte id_len = id.GetVarIntLength(buffer);

			//byte fullsize_len = (id_len + Packetlength).GetVarIntLength(buffer, id_len);


			//await BaseStream.WriteAsync(buffer, id_len, fullsize_len, token);
			//await BaseStream.WriteAsync(buffer, 0, id_len, token);




			//await packet.CopyToAsync(BaseStream, token);



		}
		#endregion

		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}


	}
}
