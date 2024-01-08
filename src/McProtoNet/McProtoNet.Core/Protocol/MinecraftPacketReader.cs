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

				var memory = MemoryPool<byte>.Shared.Rent(len);

				await BaseStream.ReadExactlyAsync(memory.Memory.Slice(0, len), token);

				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Span.Slice(0, len)),
					memory);

			}

			int sizeUncompressed = await BaseStream.ReadVarIntAsync(token);
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(sizeUncompressed);

				try
				{
					await BaseStream.ReadExactlyAsync(memory.Memory.Slice(0, len), token);

					Memory<byte> compressedData = memory.Memory.Slice(0, len);

					using (var fastStream = StaticResources.MSmanager.GetStream(compressedData.Span))
					using (var ReadZlib = new ZLibStream(fastStream, CompressionMode.Decompress, true))
					{
						int id = await ReadZlib.ReadVarIntAsync(token);

						sizeUncompressed -= id.GetVarIntLength();

						await ReadZlib.ReadExactlyAsync(memory.Memory.Slice(0, sizeUncompressed), token);

						return new Packet(
							id,
							StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, sizeUncompressed).Span),
							memory);
					}
				}
				catch
				{
					memory.Dispose();
					throw;
				}



			}
			{

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength() + 1;

				var memory = MemoryPool<byte>.Shared.Rent(len);
				try
				{
					await BaseStream.ReadExactlyAsync(memory.Memory.Slice(0, len), token);
					return new(
						id,
						StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, len).Span),
						memory);
				}
				catch
				{
					memory.Dispose();
					throw;
				}
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


