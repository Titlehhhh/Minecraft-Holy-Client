using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using LibDeflate;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public static class PacketPipeReaderExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ReadOnlySequence<byte> DecompressMultiSegment(ReadOnlySequence<byte> compressed, byte[] decompressed, ZlibDecompressor decompressor, int sizeUncompressed, out int id)
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
		public static async IAsyncEnumerable<DecompressedMinecraftPacket> Decompress(this IAsyncEnumerable<ReadOnlySequence<byte>> messages, int compressionThreshold)
		{
			using (ZlibDecompressor decompressor = new())
			{
				await foreach (ReadOnlySequence<byte> data in messages)
				{
					byte[]? rented = null;
					try
					{
						int id = -1;
						ReadOnlySequence<byte> mainData = default;

						if (compressionThreshold > 0)
						{

							data.TryReadVarInt(out int sizeUncompressed, out int len);




							ReadOnlySequence<byte> compressed = data.Slice(len);
							if (sizeUncompressed > 0)
							{
								if (sizeUncompressed < compressionThreshold)
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

						yield return new DecompressedMinecraftPacket(id, mainData);
					}
					finally
					{
						if (rented is not null)
						{
							ArrayPool<byte>.Shared.Return(rented);
						}
					}


				}

			}
		}
	}

}