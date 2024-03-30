using McProtoNet.Core;
using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{
	public struct PacketNew : IDisposable
	{
		private readonly byte[] _buffer;
		private readonly ArrayPool<byte> pool;
		private int _id;
		public PacketNew(int id, byte[] buffer, ArrayPool<byte> pool)
		{
			_id = id;
			this.pool = pool;
			this._buffer = buffer;
		}

		public void Dispose()
		{
			pool.Return(_buffer);
		}
	}


	public class MinecraftPacketReaderNew
	{
		public Stream BaseStream { get; set; }

		private readonly Inflater Inflater = new Inflater();




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


				//var buffer2 = new byte[length];
				//Inflater.SetInput(buffer);
				//Inflater.Inflate(buffer2);
				//Inflater.Reset();

				//return new PacketBuffer(buffer2, this._useAnonymousNbt);

				len -= sizeUncompressed.GetVarIntLength();

				var buffer_compress = ArrayPool<byte>.Shared.Rent(len);

				try
				{
					//using var buffer = StaticResources.MSmanager.GetStream(null, len);

					await BaseStream.ReadExactlyAsync(buffer_compress, 0, len, token);

					//buffer.Advance(len);

					//buffer.Position = 0;

					var uncompressed_buffer = ArrayPool<byte>.Shared.Rent(sizeUncompressed);

					try
					{

						Inflater.SetInput(buffer_compress, 0, len);
						Inflater.Inflate(uncompressed_buffer, 0, sizeUncompressed);
						Inflater.Reset();

						return new PacketNew(-1, uncompressed_buffer, ArrayPool<byte>.Shared);
					}
					catch
					{
						ArrayPool<byte>.Shared.Return(uncompressed_buffer);
						throw;
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