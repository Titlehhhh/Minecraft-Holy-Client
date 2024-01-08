using Microsoft.IO;
using System.Buffers;
using System.IO.Compression;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketReader : IMinecraftPacketReader
	{
		public Stream BaseStream { get; set; }



		public MinecraftPacketReader(Stream baseStream)
		{
			BaseStream = baseStream;
		}
		public MinecraftPacketReader()
		{

		}


		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Packet ReadNextPacket()
		{
			ThrowIfDisposed();

			int len = BaseStream.ReadVarInt();
			if (_compressionThreshold <= 0)
			{

				int id = BaseStream.ReadVarInt();
				len -= id.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(len);

				BaseStream.ReadExactly(memory.Memory.Slice(0, len).Span);

				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, len).Span));

			}

			int sizeUncompressed = BaseStream.ReadVarInt();
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(sizeUncompressed);

				BaseStream.ReadExactly(memory.Memory.Slice(0, len).Span);

				Memory<byte> compressedData = memory.Memory.Slice(0, len);

				using (var fastStream = StaticResources.MSmanager.GetStream(compressedData.Span))
				{
					using (var ReadZlib = new ZLibStream(fastStream, CompressionMode.Decompress, true))
					{
						int id = ReadZlib.ReadVarInt();

						sizeUncompressed -= id.GetVarIntLength();

						ReadZlib.ReadExactly(memory.Memory.Slice(0, sizeUncompressed).Span);

						return new Packet(
							id,
							StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, sizeUncompressed).Span));
					}
				}



			}
			{

				int id = BaseStream.ReadVarInt();
				len -= id.GetVarIntLength() + 1;

				var memory = MemoryPool<byte>.Shared.Rent(len);

				BaseStream.ReadExactly(memory.Memory.Slice(0, len).Span);
				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, len).Span));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public async ValueTask<Packet> ReadNextPacketAsync(CancellationToken token)
		{
			ThrowIfDisposed();

			int len = await BaseStream.ReadVarIntAsync(token);
			if (_compressionThreshold <= 0)
			{

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength();

				//var memory = MemoryPool<byte>.Shared.Rent(len);



				var stream = StaticResources.MSmanager.GetStream(null, len);

				var memory = stream.GetMemory(len);

				await BaseStream.ReadExactlyAsync(memory.Slice(0, len), token);

				stream.Advance(len);

				return new(
					id,
					stream);

			}

			int sizeUncompressed = await BaseStream.ReadVarIntAsync(token);
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				using var compressedData = StaticResources.MSmanager.GetStream(null, sizeUncompressed);


				var memory = compressedData.GetMemory(sizeUncompressed);

				await BaseStream.ReadExactlyAsync(memory.Slice(0, len), token);

				compressedData.Advance(len);



				compressedData.Position = 0;
				using (var ReadZlib = new ZLibStream(compressedData, CompressionMode.Decompress, true))
				{




					int id = await ReadZlib.ReadVarIntAsync(token);

					sizeUncompressed -= id.GetVarIntLength();

					var uncompressedStream = StaticResources.MSmanager.GetStream(null, sizeUncompressed);



					await ReadZlib.ReadExactlyAsync(uncompressedStream
						.GetMemory(sizeUncompressed)
						.Slice(0, sizeUncompressed), token);

					uncompressedStream.Advance(sizeUncompressed);
					uncompressedStream.Position = 0;

					return new Packet(
						id,
						uncompressedStream);
				}



			}
			{

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength() + 1;



				var stream = StaticResources.MSmanager.GetStream(null, len);

				await BaseStream.ReadExactlyAsync(stream
					.GetMemory(len)
					.Slice(0, len), token);

				stream.Advance(len);
				stream.Position = 0;

				return new(
					id,
					stream);
			}

		}
		private int _compressionThreshold;
		public void SwitchCompression(int threshold)
		{
			_compressionThreshold = threshold;
		}
		private bool _disposed = false;

		private void ThrowIfDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(MinecraftPacketReader));
		}
		~MinecraftPacketReader()
		{
			Dispose();
		}
		public void Dispose()
		{
			if (_disposed)
				return;

			//fastStream?.Dispose();
			//fastStream = null;

			_disposed = true;
			GC.SuppressFinalize(this);
		}




	}
}


