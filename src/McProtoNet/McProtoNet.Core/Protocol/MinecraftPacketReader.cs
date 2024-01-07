using Microsoft.IO;
using System.Buffers;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace McProtoNet.Core.Protocol
{
	public class MinecraftPacketReader : IMinecraftPacketReader
	{
		
		private readonly bool disposeStream;
		private Stream _baseStream;
		


		public MinecraftPacketReader(Stream baseStream, bool disposeStream)
		{
			_baseStream = baseStream;
		}
		public MinecraftPacketReader(Stream baseStream) : this(baseStream, true)
		{

		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public Packet ReadNextPacket()
		{
			ThrowIfDisposed();

			int len = _baseStream.ReadVarInt();
			if (_compressionThreshold <= 0)
			{

				int id = _baseStream.ReadVarInt();
				len -= id.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(len);

				_baseStream.ReadExactly(memory.Memory.Slice(0, len).Span);

				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, len).Span));

			}

			int sizeUncompressed = _baseStream.ReadVarInt();
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(sizeUncompressed);

				_baseStream.ReadExactly(memory.Memory.Slice(0, len).Span);

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

				int id = _baseStream.ReadVarInt();
				len -= id.GetVarIntLength() + 1;

				var memory = MemoryPool<byte>.Shared.Rent(len);

				_baseStream.ReadExactly(memory.Memory.Slice(0, len).Span);
				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, len).Span));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public async ValueTask<Packet> ReadNextPacketAsync(CancellationToken token)
		{
			ThrowIfDisposed();

			int len = await _baseStream.ReadVarIntAsync(token);
			if (_compressionThreshold <= 0)
			{

				int id = await _baseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(len);

				await _baseStream.ReadExactlyAsync(memory.Memory.Slice(0, len), token);

				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Span.Slice(0, len)));

			}

			int sizeUncompressed = await _baseStream.ReadVarIntAsync(token);
			if (sizeUncompressed > 0)
			{
				len -= sizeUncompressed.GetVarIntLength();

				var memory = MemoryPool<byte>.Shared.Rent(sizeUncompressed);

				await _baseStream.ReadExactlyAsync(memory.Memory.Slice(0, len), token);

				Memory<byte> compressedData = memory.Memory.Slice(0, len);

				using (var fastStream = StaticResources.MSmanager.GetStream(compressedData.Span))
				using (var ReadZlib = new ZLibStream(fastStream, CompressionMode.Decompress, true))
				{
					int id = await ReadZlib.ReadVarIntAsync(token);

					sizeUncompressed -= id.GetVarIntLength();

					await ReadZlib.ReadExactlyAsync(memory.Memory.Slice(0, sizeUncompressed), token);

					return new Packet(
						id,
						StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, sizeUncompressed).Span));
				}



			}
			{

				int id = await _baseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength() + 1;

				var memory = MemoryPool<byte>.Shared.Rent(len);

				await _baseStream.ReadExactlyAsync(memory.Memory.Slice(0, len), token);
				return new(
					id,
					StaticResources.MSmanager.GetStream(memory.Memory.Slice(0, len).Span));
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
			//if (fastStream is not null)
			{
				//await fastStream.DisposeAsync();
				//fastStream = null;
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


