using ICSharpCode.SharpZipLib.Zip.Compression;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{
	public readonly struct PacketOut : IDisposable
	{
		private readonly int offset;
		private readonly int length;
		private readonly byte[] buffer;
		private readonly ArrayPool<byte> pool;

		public byte[] Buffer => buffer;
		public int Offset => offset;
		public int Length => length;

		public PacketOut(int offset, int length, byte[] buffer, ArrayPool<byte> pool)
		{
			this.offset = offset;
			this.length = length;
			this.buffer = buffer;
			this.pool = pool;
		}

		public void Dispose()
		{
			pool.Return(buffer);
		}

		public ReadOnlyMemory<byte> GetMemory()
		{
			return new ReadOnlyMemory<byte>(buffer, offset, length);
		}



	}
	public class MinecraftPacketSenderNew
	{

		public Stream BaseStream { get; set; }



		//private static ArrayPool<byte> VarIntPool = ArrayPool<byte>.Create(10, 20);

		private readonly Deflater Deflater = new Deflater();


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



						//var buffer = input.GetBuffer();
						//output.WriteVarInt(buffer.Length);

						//Deflater.SetInput(buffer);
						//Deflater.Finish();

						//var deflateBuf = new byte[8192];
						//while (!Deflater.IsFinished)
						//{
						//	var j = Deflater.Deflate(deflateBuf);
						//	output.WriteBytes(deflateBuf.AsSpan(0, j));
						//}

						//Deflater.Reset();


						Deflater.SetInput(packet.Buffer, packet.Offset, packet.Length);

						Deflater.Finish();

						var compressed_packet = ArrayPool<byte>.Shared.Rent(8192);
						int compressedLength = 0;
						try
						{
							int offset = 0;
							int length = 8192;
							while (!Deflater.IsFinished)
							{
								var j = Deflater.Deflate(compressed_packet, offset, length);
								compressedLength += j;

								offset += j;
								length -= j;
								//await BaseStream.WriteAsync(buffer.AsMemory(0, j));
							}


							int fullsize = compressedLength + uncompressedSize.GetVarIntLength();

							await BaseStream.WriteVarIntAsync(fullsize, token);
							await BaseStream.WriteVarIntAsync(uncompressedSize, token);
							await BaseStream.WriteAsync(compressed_packet.AsMemory(0, compressedLength), token);
						}
						finally
						{
							Deflater.Reset();
							ArrayPool<byte>.Shared.Return(compressed_packet);
						}
						//using (var compressedPacket = StaticResources.MSmanager.GetStream())
						//{
						//	using (var zlibStream = new ZLibStream(compressedPacket, CompressionMode.Compress, true))
						//	{
						//		await zlibStream.WriteAsync(memory.Slice(0, idLen), token);
						//		await data.CopyToAsync(zlibStream, token);
						//	}

						//	int compressedPacketLength = (int)compressedPacket.Length;



						//	byte uncompressedSizeLength = uncompressedSize.GetVarIntLength(memory);




						//	int fullSize = uncompressedSizeLength + compressedPacketLength;

						//	byte fullsize_len = fullSize.GetVarIntLength(memory.Slice(uncompressedSizeLength));


						//	await BaseStream.WriteAsync(memory.Slice(uncompressedSizeLength, fullsize_len), token);

						//	await BaseStream.WriteAsync(memory.Slice(0, uncompressedSizeLength), token);
						//	//await BaseStream.WriteVarIntAsync(fullSize, token);

						//	//await BaseStream.WriteVarIntAsync(uncompressedSize, token);

						//	compressedPacket.Position = 0;
						//	await compressedPacket.CopyToAsync(BaseStream, token);

						//}
					}
					else
					{
						uncompressedSize++;

						await BaseStream.WriteVarIntAsync(uncompressedSize, token);
						await BaseStream.WriteAsync(ZERO_VARINT, token);
						await BaseStream.WriteAsync(packet.GetMemory(), token);

						//byte unc_len = uncompressedSize.GetVarIntLength(memory.Slice(idLen));

						//await BaseStream.WriteVarIntAsync(uncompressedSize, token);

						//await BaseStream.WriteAsync(memory.Slice(idLen, unc_len), token);

						//await BaseStream.WriteAsync(ZERO_VARINT, token);

						//await BaseStream.WriteAsync(memory.Slice(0, idLen), token);
						//await BaseStream.WriteAsync(buffer, 0, idLen, token);

						//await data.CopyToAsync(BaseStream, token);


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