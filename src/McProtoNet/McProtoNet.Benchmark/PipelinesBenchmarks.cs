using BenchmarkDotNet.Attributes;
using DotNext.Buffers;
using DotNext.IO;
using LibDeflate;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
using McProtoNet.Experimental;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(true)]
	public class PipelinesBenchmarks
	{

		public int CompressionThreshold { get; set; } = 128;

		public int PacketsCount { get; set; } = 1_000_000;

		private Pipe pipe;

		private long TotalBytes;


		public async Task Setup()
		{


			using var mainStream = File.Open("data.bin", FileMode.Create, FileAccess.Write, FileShare.Write);



			var sender = new MinecraftPacketSender();
			sender.SwitchCompression(CompressionThreshold);
			sender.BaseStream = mainStream;
			for (int i = 0; i < PacketsCount; i++)
			{
				var data = new byte[Random.Shared.Next(10, 512)];

				int id = Random.Shared.Next(0, 60);

				TotalBytes += data.Length;
				Random.Shared.NextBytes(data);



				MemoryStream ms = new MemoryStream(data);

				ms.Position = 0;


				var packet = new Packet(id, ms);

				await sender.SendPacketAsync(packet);
			}
			await mainStream.FlushAsync();


			pipe = new Pipe();
		}
		public void Clean()
		{

		}

		public async Task ReadStream1()
		{
			using var mainStream = File.OpenRead("data.bin");

			var native_reader = new MinecraftPacketReader();

			native_reader.BaseStream = mainStream;
			native_reader.SwitchCompression(CompressionThreshold);

			for (int i = 0; i < PacketsCount; i++)
			{
				using var packet = await native_reader.ReadNextPacketAsync();

			}


		}
		public async Task ReadStream2()
		{
			using var mainStream = File.OpenRead("data.bin");


			using var native_reader = new MinecraftPacketReaderNew();

			native_reader.BaseStream = mainStream;
			native_reader.SwitchCompression(CompressionThreshold);

			for (int i = 0; i < PacketsCount; i++)
			{

				using var packet = await native_reader.ReadNextPacketAsync();

			}
		}





		public async Task ReadWithPipelines()
		{

			PacketPipeReader pipeReader = new PacketPipeReader(pipe.Reader);

			var fill = FillPipe();
			var read = ProcessPackets(pipeReader);



			await Task.WhenAll(fill, read);



			pipe.Reset();

		}
		private async Task ProcessPackets(PacketPipeReader reader)
		{
			using (ZlibDecompressor decompressor = new())
			{
				long bytesCount = 0;
				int count = 0;
				await foreach (ReadOnlySequence<byte> data in reader.ReadPacketsAsync())
				{

					try
					{
						int id = -1;
						ReadOnlySequence<byte> mainData = default;
						byte[]? rented = null;
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

									try
									{

										var result = decompressor.Decompress(
														compressed.FirstSpan,
														decompressed.AsSpan(0, sizeUncompressed),
														out int written);

										if (result != OperationStatus.Done)
											throw new Exception("Zlib: " + result);
									}
									catch
									{
										throw new Exception($"data: {data.Length} uncompressed: {sizeUncompressed} compressed: {compressed.Length} compressed f:{compressed.FirstSpan.Length}");
									}

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





						bytesCount += mainData.Length;



						count++;

						if (rented is not null)
						{
							ArrayPool<byte>.Shared.Return(rented);
						}




					}
					catch (Exception ex)
					{
						throw new Exception($"В пакете {count} ошибка:", ex);
					}
				}

				if (bytesCount != TotalBytes)
				{
					throw new Exception($"данные не прочитаны полностью: Должно быть: {TotalBytes} Прочитано: {bytesCount}");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private async Task FillPipe()
		{
			using var mainStream = File.OpenRead("data.bin");

			try
			{
				while (true)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(512);
					try
					{
						int count = await mainStream.ReadAsync(memory);

						if (count == 0)
							return;

						pipe.Writer.Advance(count);

						FlushResult flushResult = await pipe.Writer.FlushAsync();

						if (flushResult.IsCanceled || flushResult.IsCompleted)
							return;
					}
					catch (Exception ex)
					{
						Console.WriteLine("Fill exception " + ex);
					}




				}
			}
			finally
			{
				await pipe.Writer.CompleteAsync();
			}
		}


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


	}
}
