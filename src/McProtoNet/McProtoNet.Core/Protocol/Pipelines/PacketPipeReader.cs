using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Core;
using System.Runtime.InteropServices;
using LibDeflate;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Threading.Channels;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public interface IReadResult : IDisposable
	{
		ReadOnlyMemory<byte> Memory { get; }
		int Id { get; }
	}

	public readonly struct McReadResult : IDisposable
	{
		private readonly object _memory;
		public ReadOnlyMemory<byte> Memory
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (ReadOnlyMemory<byte>)_memory;
		}

		public readonly int Id { get; }


		private readonly object? _owner;



		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal McReadResult(int id, ReadOnlyMemory<byte> memory)
		{
			Id = id;
			_memory = memory;
			_owner = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal McReadResult(int id, MemoryOwner<byte> owner)
		{
			Id = id;
			_memory = owner.Memory;
			_owner = owner;


		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal McReadResult(int id, MemoryOwner<byte> owner, int offset)
		{
			Id = id;
			_memory = owner.Memory.Slice(offset);
			_owner = owner;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal McReadResult(int id, MemoryOwner<byte> owner, int offset, int length)
		{
			Id = id;
			_memory = owner.Memory.Slice(offset, length);
			_owner = owner;
		}


		public void Dispose()
		{
			Unsafe.As<IDisposable>(_owner)?.Dispose();

			//this = default;
		}
	}


	public sealed class PacketPipeReader : IDisposable
	{
		private ZlibDecompressor decompressor = new();

		private readonly PipeReader pipeReader;

		private int _compressionThreshold;
		public int CompressionThreshold
		{
			get => _compressionThreshold;
			set => _compressionThreshold = value;
		}
		public PacketPipeReader(PipeReader pipeReader)
		{
			this.pipeReader = pipeReader;
		}

		public async IAsyncEnumerable<McReadResult> RunAsync([EnumeratorCancellation] CancellationToken cancellation = default)
		{

			try
			{
				while (!cancellation.IsCancellationRequested)
				{

					ReadResult readResult = await pipeReader.ReadAsync(cancellation);

					ReadOnlySequence<byte> buffer = readResult.Buffer;

					while (TryReadPacket(ref buffer, out McReadResult result))
					{
						
						yield return result;						
					}

					if (readResult.IsCompleted)
					{
						if (!buffer.IsEmpty)
						{
							throw new InvalidDataException("Incomplete message.");
						}
						break;
					}
					if (readResult.IsCanceled)
					{
						break;
					}


					pipeReader.AdvanceTo(buffer.Start, buffer.End);
				}

			}
			finally
			{
				await pipeReader.CompleteAsync();

			}
			yield break;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out McReadResult result)
		{
			scoped SequenceReader<byte> reader = new SequenceReader<byte>(buffer);
			if (TryReadVarInt(ref reader, out int length, out _))
			{
				if (reader.Remaining >= length)
				{
					if (_compressionThreshold <= 0)
					{
						TryReadVarInt(ref reader, out int id, out int id_len);
						length -= id_len;
						result = ReadPacketWithoutCompression(id, ref reader, length);
					}
					else
					{
						TryReadVarInt(ref reader, out int sizeUncompressed, out int len);

						if (sizeUncompressed > 0)
						{
							length -= len;
							result = ReadPacketWithCompression(ref reader, length, sizeUncompressed);
						}
						else
						{
							TryReadVarInt(ref reader, out int id, out int id_len);
							length -= id_len + 1;
							result = ReadPacketWithoutCompression(id, ref reader, length);
						}
					}

					buffer = buffer.Slice(reader.Position);

					return true;
				}
				else
				{
					result = default;
					return false;
				}

			}
			else
			{
				result = default;
				return false;
			}

		}


		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private McReadResult ReadPacketWithCompression(ref SequenceReader<byte> reader, int length, int sizeUncompressed)
		{



			MemoryOwner<byte> uncompressed = s_allocator.AllocateExactly(sizeUncompressed);

			try
			{
				scoped Span<byte> uncompressedSpan = uncompressed.Span;

				int id = 0;
				int offset = 0;

				ReadOnlySpan<byte> unread = reader.UnreadSpan;
				if (unread.Length >= length)
				{


					DoDecompress(unread.Slice(0, length), uncompressedSpan, out int written);
					reader.Advance(length);





					id = ReadVarInt(uncompressedSpan, out offset);



				}
				else
				{
					using (SpanOwner<byte> compressed = length <= 256 ?
						new SpanOwner<byte>(stackalloc byte[length]) :
						new SpanOwner<byte>(length))
					{
						Span<byte> span = compressed.Span;

						ReadOnlySequence<byte> compressedPayload = reader.UnreadSequence.Slice(0, length);

						compressedPayload.CopyTo(span);		
						
						reader.Advance(length);

						DoDecompress(span, uncompressedSpan, out int written);

						id = ReadVarInt(uncompressedSpan, out offset);
					}
				}

				return new McReadResult(id, uncompressed, offset);
			}
			catch
			{
				uncompressed.Dispose();
				throw;
			}
		}
		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoDecompress(ReadOnlySpan<byte> data, Span<byte> uncompressed, out int bytesWitten)
		{
			var result = decompressor.Decompress(data, uncompressed, out bytesWitten);
			if (result != OperationStatus.Done)
				throw new InvalidOperationException("Decompress Error status: " + result);
		}
		private static readonly MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private McReadResult ReadPacketWithoutCompression(int id, ref SequenceReader<byte> reader, int length)
		{


			MemoryOwner<byte> owner = s_allocator.AllocateExactly(length);
			try
			{
				ReadOnlySequence<byte> payload = reader.UnreadSequence.Slice(0, length);
				payload.CopyTo(owner.Span);

				reader.Advance(length);

				return new McReadResult(id, owner);

			}
			catch
			{
				owner.Dispose();
				throw;
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


		private bool TryReadVarInt(ref SequenceReader<byte> reader, out int res, out int length)
		{



			int numRead = 0;
			int result = 0;
			byte read;
			do
			{

				if (reader.TryPeek(numRead, out read))
				{
					int value = read & 0b01111111;
					result |= value << 7 * numRead;

					numRead++;
					if (numRead > 5)
					{
						throw new ArithmeticException("VarInt too long");
					}
				}
				else
				{
					res = 0;
					length = -1;
					return false;
				}

			} while ((read & 0b10000000) != 0);

			reader.Advance(numRead);

			res = result;
			length = numRead;
			return true;
		}
		bool disposed;
		public void Dispose()
		{
			if (disposed)
				return;
			disposed = true;

			decompressor.Dispose();
			decompressor = null;

			GC.SuppressFinalize(this);
		}
	}

}