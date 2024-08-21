using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.Core;
using LibDeflate;

namespace McProtoNet.Experimental
{
	public sealed class MinecraftPacketReaderNew : IDisposable
	{
		public Stream BaseStream { get; set; }

		//private readonly Inflater Inflater = new Inflater();

		private readonly ZlibDecompressor decompressor = new();



		//[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public async ValueTask<PacketNew> ReadNextPacketAsync(CancellationToken token = default)
		{
			int len = await BaseStream.ReadVarIntAsync(token);
			if (_compressionThreshold <= 0)
			{

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength();

				var buffer = ArrayPool<byte>.Shared.Rent(len);

				try
				{
					await BaseStream.ReadExactlyAsync(buffer, 0, len, token);

					return new PacketNew(id, buffer, ArrayPool<byte>.Shared, 0, len);
				}
				catch
				{
					ArrayPool<byte>.Shared.Return(buffer);
					throw;
				}



			}

			int sizeUncompressed = await BaseStream.ReadVarIntAsync(token);



			if (sizeUncompressed > 0)
			{

				if (sizeUncompressed < _compressionThreshold)
					throw new Exception($"Длина sizeUncompressed меньше порога сжатия. sizeUncompressed: {sizeUncompressed} Порог: {_compressionThreshold}");


				len -= sizeUncompressed.GetVarIntLength();

				var buffer_compress = ArrayPool<byte>.Shared.Rent(len);

				try
				{


					await BaseStream.ReadExactlyAsync(buffer_compress, 0, len, token);

					var uncompressed = ArrayPool<byte>.Shared.Rent(sizeUncompressed);

					var status = decompressor.Decompress(
						buffer_compress.AsSpan(0, len),
						uncompressed.AsSpan(0, sizeUncompressed), out int written);
					if (status == OperationStatus.Done)
					{

						int id = ReadVarInt(uncompressed, out int offset);

						int length = sizeUncompressed - offset;

						return new PacketNew(id, uncompressed, ArrayPool<byte>.Shared, offset, length);
					}
					else
					{
						throw new Exception("Decompress Error");
					}

				}
				finally
				{
					ArrayPool<byte>.Shared.Return(buffer_compress);
				}








			}
			else
			{

				if (sizeUncompressed != 0)
					throw new Exception("size incorrect");

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength() + 1;


				var buffer = ArrayPool<byte>.Shared.Rent(len);

				try
				{
					await BaseStream.ReadExactlyAsync(buffer, 0, len, token);

					return new PacketNew(id, buffer, ArrayPool<byte>.Shared, 0, len);
				}
				catch
				{
					ArrayPool<byte>.Shared.Return(buffer);
					throw;
				}



			}


		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ReadVarInt(Span<byte> data, out int len)
		{



			int numRead = 0;
			int result = 0;
			byte read;
			do
			{

				read = data[numRead];

				int value = read & 0b01111111;
				result |= value << 7 * numRead;

				numRead++;
				if (numRead > 5)
				{
					throw new ArithmeticException("VarInt too long");
				}


			} while ((read & 0b10000000) != 0);

			//data = data.Slice(numRead);


			len = numRead;
			return result;
		}

		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}

		public void Dispose()
		{
			decompressor.Dispose();
		}
	}
}
