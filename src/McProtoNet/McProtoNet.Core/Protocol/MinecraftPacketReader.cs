using Microsoft.Extensions.ObjectPool;
using Microsoft.IO;
using System.Buffers;
using System.IO.Compression;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketReader : IMinecraftPacketReader
	{
		private static readonly RecyclableMemoryStreamManager MSmanager = new RecyclableMemoryStreamManager();
		private readonly bool disposeStream;
		private Stream _baseStream;
		private RecyclableMemoryStream fastStream = MSmanager.GetStream() as RecyclableMemoryStream;

		

		public MinecraftPacketReader(Stream baseStream, bool disposeStream)
		{
			_baseStream = baseStream;
		}
		public MinecraftPacketReader(Stream baseStream) : this(baseStream, true)
		{

		}

		public Packet ReadNextPacket()
		{
			ThrowIfDisposed();

			int len = _baseStream.ReadVarInt();
			if (_compressionThreshold <= 0)
			{

				int id = _baseStream.ReadVarInt();
				len -= id.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(len);

				_baseStream.ReadToEnd(memory.Memory.Slice(0, len).Span, len);

				return new(
					id,
					MSmanager.GetStream(memory.Memory.Slice(0, len).Span),
					memory);

			}

			int sizeUncompressed = _baseStream.ReadVarInt();
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(sizeUncompressed);

				_baseStream.ReadToEnd(memory.Memory.Slice(0, len).Span, len);

				Memory<byte> compressedData = memory.Memory.Slice(0, len);

				fastStream.Position = 0;
				fastStream.SetLength(len);
				var destMemory = fastStream.GetMemory(len);

				compressedData.CopyTo(destMemory);

				using (var ReadZlib = new ZLibStream(fastStream, CompressionMode.Decompress, true))
				{
					int id = ReadZlib.ReadVarInt();

					sizeUncompressed -= id.GetVarIntLength();

					ReadZlib.ReadToEnd(memory.Memory.Slice(0, sizeUncompressed).Span, sizeUncompressed);

					return new Packet(
						id,
						MSmanager.GetStream(memory.Memory.Slice(0, sizeUncompressed).Span),
						memory);
				}



			}
			{

				int id = _baseStream.ReadVarInt();
				len -= id.GetVarIntLength() + 1;

				var memory = MemoryPool<byte>.Shared.Rent(len);

				_baseStream.ReadToEnd(memory.Memory.Slice(0, len).Span, len);
				return new(
					id,
					MSmanager.GetStream(memory.Memory.Slice(0, len).Span),
					memory);
			}
		}

		public async ValueTask<Packet> ReadNextPacketAsync(CancellationToken token)
		{
			ThrowIfDisposed();

			int len = await _baseStream.ReadVarIntAsync(token);
			if (_compressionThreshold <= 0)
			{

				int id = await _baseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(len);

				await _baseStream.ReadToEndAsync(memory.Memory.Slice(0, len), len, token);

				return new(
					id,
					MSmanager.GetStream(memory.Memory.Span.Slice(0, len)),
					memory);

			}

			int sizeUncompressed = await _baseStream.ReadVarIntAsync(token);
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(sizeUncompressed);

				await _baseStream.ReadToEndAsync(memory.Memory.Slice(0, len), len, token);

				Memory<byte> compressedData = memory.Memory.Slice(0, len);

				fastStream.Position = 0;
				fastStream.SetLength(len);
				var destMemory = fastStream.GetMemory(len);

				compressedData.CopyTo(destMemory);

				using (var ReadZlib = new ZLibStream(fastStream, CompressionMode.Decompress, true))
				{
					int id = await ReadZlib.ReadVarIntAsync(token);

					sizeUncompressed -= id.GetVarIntLength();

					await ReadZlib.ReadToEndAsync(memory.Memory.Slice(0, sizeUncompressed), sizeUncompressed, token);

					return new Packet(
						id,
						MSmanager.GetStream(memory.Memory.Slice(0, sizeUncompressed).Span),
						memory);
				}



			}
			{

				int id = await _baseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength() + 1;

				var memory = MemoryPool<byte>.Shared.Rent(len);

				await _baseStream.ReadToEndAsync(memory.Memory.Slice(0, len), len, token);
				return new(
					id,
					MSmanager.GetStream(memory.Memory.Slice(0, len).Span),
					memory);
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

			fastStream?.Dispose();
			fastStream = null;
			if (disposeStream)
			{
				if (_baseStream is not null)
				{
					_baseStream.Dispose();
					_baseStream = null;
				}
			}
			_disposed = true;
			GC.SuppressFinalize(this);
		}

		public async ValueTask DisposeAsync()
		{
			if (_disposed)
				return;
			_disposed = true;
			if (fastStream is not null)
			{
				await fastStream.DisposeAsync();
				fastStream = null;
			}
			if (disposeStream)
			{
				if (_baseStream is not null)
				{
					await _baseStream.DisposeAsync();
					_baseStream = null;
				}
			}
			GC.SuppressFinalize(this);
		}

		
	}
}


