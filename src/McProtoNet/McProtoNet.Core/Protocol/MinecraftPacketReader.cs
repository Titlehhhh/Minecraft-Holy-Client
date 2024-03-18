using Microsoft.IO;
using Org.BouncyCastle.Asn1.Cms;
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
			//ThrowIfDisposed();

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
			//ThrowIfDisposed();

			int len = await BaseStream.ReadVarIntAsync(token);
			if (_compressionThreshold <= 0)
			{
				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength();
				if (true)
				{


					var stream = StaticResources.MSmanager.GetStream(null, len);

					await BaseStream.ReadExactlyAsync(stream.GetMemory(len).Slice(0, len), token);

					stream.Advance(len);

					stream.Position = 0;

					return new Packet(id, stream);
				}
				else
				{

					byte[] buffer = new byte[len];

					await BaseStream.ReadExactlyAsync(buffer, token);

					var stream = new MemoryStream(buffer);
					stream.Position = 0;

					return new Packet(id, stream);
				}



			}

			int sizeUncompressed = await BaseStream.ReadVarIntAsync(token);


			if (sizeUncompressed > 0)
			{
				
				len -= sizeUncompressed.GetVarIntLength();

				if (true)
				{

					using var buffer = StaticResources.MSmanager.GetStream(null, len);

					await BaseStream.ReadExactlyAsync(buffer.GetMemory(len).Slice(0, len), token);

					buffer.Advance(len);

					buffer.Position = 0;

					

					using (var ReadZlib = new ZLibStream(buffer, CompressionMode.Decompress, true))
					{
						int id = await ReadZlib.ReadVarIntAsync(token);
						sizeUncompressed -= id.GetVarIntLength();

						var stream = StaticResources.MSmanager.GetStream(null, sizeUncompressed);

						await ReadZlib.ReadExactlyAsync(stream.GetMemory(sizeUncompressed).Slice(0, sizeUncompressed), token);

						stream.Advance(sizeUncompressed);

						stream.Position = 0;
						return new Packet(id, stream);
					}
				}
				else
				{
					byte[] buffer = new byte[len];
					await BaseStream.ReadExactlyAsync(buffer, token);
					using var ms = new MemoryStream(buffer);
					ms.Position = 0;

					using (var ReadZlib = new ZLibStream(ms, CompressionMode.Decompress, true))
					{
						int id = await ReadZlib.ReadVarIntAsync(token);
						sizeUncompressed -= id.GetVarIntLength();



						byte[] decompressed = new byte[sizeUncompressed];

						await ReadZlib.ReadExactlyAsync(decompressed, token);

						var stream = new MemoryStream(decompressed);
						stream.Position = 0;

						return new Packet(id, stream);
					}
				}






			}
			else
			{
				if (sizeUncompressed != 0)
					throw new Exception("size incorrect");

				int id = await BaseStream.ReadVarIntAsync(token);
				len -= id.GetVarIntLength() + 1;

				if (true)
				{

					var stream = StaticResources.MSmanager.GetStream();

					await BaseStream.ReadExactlyAsync(stream.GetMemory(len).Slice(0, len), token);

					stream.Advance(len);

					stream.Position = 0;


					return new Packet(id, stream);
				}
				else
				{
					byte[] buffer = new byte[len];

					await BaseStream.ReadExactlyAsync(buffer, token);

					var stream = new MemoryStream(buffer);
					stream.Position = 0;
					return new Packet(id, stream);
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


