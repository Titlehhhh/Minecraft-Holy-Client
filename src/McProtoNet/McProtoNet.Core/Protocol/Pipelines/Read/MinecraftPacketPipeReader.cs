using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.Core;
using System.Runtime.InteropServices;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Diagnostics;
using System.Threading;
using LibDeflate;
using DotNext.Buffers;
using System.Reactive;
using DotNext;
using System.Net.Sockets;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public sealed class MinecraftPacketPipeReader
	{


		private readonly PipeReader pipeReader;
		private readonly ZlibDecompressor decompressor;
		public int CompressionThreshold { get; set; }

		public MinecraftPacketPipeReader(PipeReader pipeReader, ZlibDecompressor decompressor)
		{
			this.pipeReader = pipeReader;
			this.decompressor = decompressor;
		}

		public async IAsyncEnumerable<DecompressedMinecraftPacket> ReadPacketsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			while (!cancellationToken.IsCancellationRequested)
			{

				ReadResult result = default;
				try
				{
					result = await pipeReader.ReadAsync(cancellationToken).ConfigureAwait(false);
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception ex)
				{
					break;
				}
				if (result.IsCompleted)
				{
					break;
				}
				

				ReadOnlySequence<byte> buffer = result.Buffer;

				while (TryReadPacket(ref buffer, out ReadOnlySequence<byte> packet))
				{
					yield return Decompress(packet);
				}

				if (result.IsCanceled)
				{
					break;
				}
			}


			await pipeReader.CompleteAsync().ConfigureAwait(false);
			yield break;

		}




		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> packet)
		{
			scoped SequenceReader<byte> reader = new(buffer);

			packet = ReadOnlySequence<byte>.Empty;

			if (buffer.Length < 1)
			{
				return false; // Недостаточно данных для чтения заголовка пакета
			}

			int length;
			int bytesRead;
			if (!reader.TryReadVarInt(out length, out bytesRead))
			{
				return false; // Невозможно прочитать длину заголовка
			}


			if (length > reader.Remaining)
			{
				return false; // Недостаточно данных для чтения полного пакета
			}

			reader.Advance(length);

			// Чтение данных пакета
			packet = buffer.Slice(bytesRead, length);
			buffer = reader.UnreadSequence;

			return true;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private DecompressedMinecraftPacket Decompress(in ReadOnlySequence<byte> data)
		{
			byte[]? rented = null;
			try
			{
				int id = -1;
				ReadOnlySequence<byte> mainData = default;

				if (CompressionThreshold > 0)
				{

					data.TryReadVarInt(out int sizeUncompressed, out int len);




					ReadOnlySequence<byte> compressed = data.Slice(len);
					if (sizeUncompressed > 0)
					{
						if (sizeUncompressed < CompressionThreshold)
						{
							throw new Exception("Размер несжатого пакета меньше порога сжатия.");
						}
						byte[] decompressed = ArrayPool<byte>.Shared.Rent(sizeUncompressed);
						rented = decompressed;
						if (compressed.IsSingleSegment)
						{



							var result = decompressor.Decompress(
											compressed.FirstSpan,
											decompressed.AsSpan(0, sizeUncompressed),
											out int written);

							if (result != OperationStatus.Done)
								throw new Exception("Zlib: " + result);


							id = decompressed.AsSpan().ReadVarInt(out len);



							mainData = new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);
						}
						else
						{
							mainData = DecompressMultiSegment(compressed, decompressed, decompressor, sizeUncompressed, out id);
						}


					}
					else if (sizeUncompressed == 0)
					{
						compressed.TryReadVarInt(out id, out len);
						mainData = compressed.Slice(len);
					}
					else
					{
						throw new InvalidOperationException($"sizeUncompressed negative: {sizeUncompressed}");
					}

				}
				else
				{
					data.TryReadVarInt(out id, out int len);

					mainData = data.Slice(len);
				}

				return new DecompressedMinecraftPacket(id, mainData);
			}
			finally
			{
				if (rented is not null)
				{
					ArrayPool<byte>.Shared.Return(rented);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlySequence<byte> DecompressMultiSegment(ReadOnlySequence<byte> compressed, byte[] decompressed, ZlibDecompressor decompressor, int sizeUncompressed, out int id)
		{

			int compressedLength = (int)compressed.Length;

			using scoped SpanOwner<byte> compressedTemp = compressedLength <= 256 ?
				new SpanOwner<byte>(stackalloc byte[compressedLength]) :
				new SpanOwner<byte>(compressedLength);

			scoped Span<byte> decompressedSpan = decompressed.AsSpan(0, sizeUncompressed);

			scoped Span<byte> compressedTempSpan = compressedTemp.Span;


			compressed.CopyTo(compressedTempSpan);




			var result = decompressor.Decompress(
						compressedTempSpan,
						decompressedSpan,
						out int written);

			if (result != OperationStatus.Done)
				throw new Exception("Zlib: " + sizeUncompressed);

			id = decompressedSpan.ReadVarInt(out int len);

			return new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);

		}

	}

}