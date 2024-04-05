using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.Core;
using LibDeflate;

namespace McProtoNet.Experimental
{
	public class MinecraftPacketReaderNew
	{
		public Stream BaseStream { get; set; }

		//private readonly Inflater Inflater = new Inflater();

		private readonly ZlibDecompressor decompressor = new();



		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public async ValueTask<PacketNew> ReadNextPacketAsync(CancellationToken token = default)
		{
			//ThrowIfDisposed();

			int len = await BaseStream.ReadVarIntAsync(token);
			if (_compressionThreshold <= 0)
			{

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength();

				//var stream = StaticResources.MSmanager.GetStream(null, len);

				var buffer = ArrayPool<byte>.Shared.Rent(len);

				try
				{
					await BaseStream.ReadExactlyAsync(buffer, 0, len, token);

					//stream.Advance(len);

					//stream.Position = 0;




					return new PacketNew(id, buffer, ArrayPool<byte>.Shared);
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


				len -= sizeUncompressed.GetVarIntLength();

				var buffer_compress = ArrayPool<byte>.Shared.Rent(len);

				try
				{
					var uncompressed = ArrayPool<byte>.Shared.Rent(sizeUncompressed);

					await BaseStream.ReadExactlyAsync(buffer_compress, 0, len, token);


					var status = decompressor.Decompress(buffer_compress.AsSpan(0, len), uncompressed.AsSpan(0, sizeUncompressed), out int written);
					if (status == OperationStatus.Done)
					{
						return new PacketNew(1, uncompressed, ArrayPool<byte>.Shared);
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

					return new PacketNew(id, buffer, ArrayPool<byte>.Shared);
				}
				catch
				{
					ArrayPool<byte>.Shared.Return(buffer);
					throw;
				}



			}


		}
		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}

	}
}
