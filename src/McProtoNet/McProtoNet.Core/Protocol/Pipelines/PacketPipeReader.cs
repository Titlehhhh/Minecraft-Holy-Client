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


namespace McProtoNet.Core.Protocol.Pipelines
{
	public interface IPacketProcessor
	{
		void ProcessPacket(int id, ref ReadOnlySpan<byte> data);
	}
	public sealed class PacketPipeReader : IDisposable
	{
		private ZlibDecompressor decompressor = new();

		private readonly PipeReader pipeReader;
		private readonly IPacketProcessor packetProcessor;
		private int _compressionThreshold;
		public int CompressionThreshold
		{
			get => _compressionThreshold;
			set => _compressionThreshold = value;
		}
		public PacketPipeReader(PipeReader pipeReader, IPacketProcessor packetProcessor)
		{
			this.pipeReader = pipeReader;
			this.packetProcessor = packetProcessor;
		}

		public async Task RunAsync()
		{
			while (true)
			{
				ReadResult readResult = await pipeReader.ReadAsync();

				ReadOnlySequence<byte> buffer = readResult.Buffer;

				if(TryReadPackets(ref buffer))
				{

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

		private bool TryReadPackets(ref ReadOnlySequence<byte> buffer)
		{
			SequenceReader<byte> reader = new SequenceReader<byte>(buffer);

			int count = 0;
			while (TryReadPacket(ref reader))
			{
				count++;
				buffer = buffer.Slice(reader.Position);
			}

			return count > 0;

		}

		private bool TryReadPacket(ref SequenceReader<byte> reader)
		{
			if (TryReadVarInt(ref reader, out int length, out _))
			{
				if (_compressionThreshold <= 0)
				{
					if (TryReadVarInt(ref reader, out int id, out int id_len))
					{
						length -= id_len;

						if (reader.Remaining >= length)
						{
							ReadPacketWithoutCompression(id, ref reader, length);
							return true;
						}

					}
				}
				else
				{
					if (TryReadVarInt(ref reader, out int sizeUncompressed, out int len))
					{
						if (sizeUncompressed > 0)
						{
							//sizeUncompressed -= len;
							length -= len;
							if (reader.Remaining >= length)
							{
								ReadPacketWithCompression(ref reader, length, sizeUncompressed);
								return true;
							}
						}
						else
						{
							if (TryReadVarInt(ref reader, out int id, out int id_len))
							{
								length -= id_len + 1;
								if (reader.Remaining >= length)
								{
									ReadPacketWithoutCompression(id, ref reader, length);
									return true;
								}
							}
						}
					}
				}

			}
			return false;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ReadPacketWithCompression(ref SequenceReader<byte> reader, int length, int sizeUncompressed)
		{
			if (reader.UnreadSpan.Length >= length)
			{
				using (scoped SpanOwner<byte> uncompressed = new SpanOwner<byte>(sizeUncompressed))
				{
					DoDecompress(reader.UnreadSpan.Slice(0, length), uncompressed.Span, out int written);
					reader.Advance(length);


					ReadOnlySpan<byte> data = uncompressed.Span;

					int id = ReadVarInt(ref data);

					this.packetProcessor.ProcessPacket(id, ref data);
				}
			}
			else if (reader.Remaining >= length)
			{
				using (SpanOwner<byte> compressed = new SpanOwner<byte>(length))
				{
					Span<byte> span = compressed.Span;
					reader.TryCopyTo(span);
					reader.Advance(length);

					using (scoped SpanOwner<byte> uncompressed = new SpanOwner<byte>(sizeUncompressed))
					{
						DoDecompress(compressed.Span, uncompressed.Span, out int written);



						ReadOnlySpan<byte> data = uncompressed.Span;

						int id = ReadVarInt(ref data);

						this.packetProcessor.ProcessPacket(id, ref data);
					}
				}
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoDecompress(ReadOnlySpan<byte> data, Span<byte> uncompressed, out int bytesWitten)
		{
			var result = decompressor.Decompress(data, uncompressed, out bytesWitten);
			if (result != OperationStatus.Done)
				throw new InvalidOperationException("Decompress Error status: " + result);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ReadPacketWithoutCompression(int id, ref SequenceReader<byte> reader, int length)
		{
			ReadOnlySpan<byte> unread = reader.UnreadSpan;
			if (unread.Length >= length)
			{
				ReadOnlySpan<byte> data = unread.Slice(0, length);
				reader.Advance(length);
				this.packetProcessor.ProcessPacket(id, ref data);
			}
			else
			{

				if (length <= 256)
				{
					Span<byte> data = stackalloc byte[length];
					reader.TryCopyTo(data);
					reader.Advance(length);

					ReadOnlySpan<byte> r_only = data;
					this.packetProcessor.ProcessPacket(id, ref r_only);

				}
				else
				{
					byte[] arrayPoolBuffer = ArrayPool<byte>.Shared.Rent(length);
					try
					{
						reader.TryCopyTo(arrayPoolBuffer.AsSpan(0, length));
						reader.Advance(length);

						ReadOnlySpan<byte> r_only = new ReadOnlySpan<byte>(arrayPoolBuffer, 0, length);
						this.packetProcessor.ProcessPacket(id, ref r_only);
					}
					finally
					{
						ArrayPool<byte>.Shared.Return(arrayPoolBuffer);
					}

				}
			}
		}
		private int ReadVarInt(ref ReadOnlySpan<byte> data)
		{



			int numRead = 0;
			int result = 0;
			byte read;
			do
			{

				read = data[numRead];
				{
					int value = read & 0b01111111;
					result |= value << 7 * numRead;

					numRead++;
					if (numRead > 5)
					{
						throw new ArithmeticException("VarInt too long");
					}
				}

			} while ((read & 0b10000000) != 0);

			data = data.Slice(0, numRead);


			//len = numRead;
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