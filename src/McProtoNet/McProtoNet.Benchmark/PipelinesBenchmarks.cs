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
using System.IO;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(true)]
	public class PipelinesBenchmarks
	{




		[Params(128)]
		public int CompressionThreshold { get; set; }
		[Params(100_000)]
		public int PacketsCount { get; set; }

		private Pipe pipe;

		private long TotalBytes;

		[GlobalSetup]
		public async Task Setup()
		{

			using var mainStream = File.OpenWrite("data.bin");

			var sender = new MinecraftPacketSender();
			sender.SwitchCompression(CompressionThreshold);
			sender.BaseStream = mainStream;
			for (int i = 0; i < PacketsCount; i++)
			{
				var data = new byte[Random.Shared.Next(10, 512)];
				TotalBytes += data.Length;
				Random.Shared.NextBytes(data);

				MemoryStream ms = new MemoryStream(data);

				ms.Position = 0;


				var packet = new Packet(Random.Shared.Next(0, 60), ms);

				await sender.SendPacketAsync(packet);
			}
			await mainStream.FlushAsync();
			pipe = new Pipe();
		}
		[GlobalCleanup]
		public void Clean()
		{

		}


		[Benchmark]
		public async Task ReadNew()
		{
			using var native_reader = new MinecraftPacketReaderNew();
			using var fs = File.OpenRead("data.bin");
			native_reader.BaseStream = fs;
			native_reader.SwitchCompression(CompressionThreshold);

			for (int i = 0; i < PacketsCount; i++)
			{
				try
				{
					using var packet = await native_reader.ReadNextPacketAsync();
				}
				catch
				{
					throw new Exception($"Packet: {i} error");
				}
			}
		}




		[Benchmark]
		public async Task ReadWithPipelines2()
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
				long count = 0;
				await foreach (ReadOnlySequence<byte> data in reader.ReadPacketsAsync())
				{
					ReadOnlySequence<byte> mainData = default;
					byte[]? rented = null;
					if (CompressionThreshold > 0)
					{
						data.TryReadVarInt(out int sizeUncompressed, out int len);
						ReadOnlySequence<byte> compressed = data.Slice(len);
						if (sizeUncompressed == 0)
						{

							data.TryReadVarInt(out int id, out len);
							mainData = compressed.Slice(len);
						}
						else
						{
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

								int id = decompressed.AsSpan().ReadVarInt(out len);



								mainData = new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);
							}
							else
							{
								mainData = DecompressCore(compressed, decompressed, decompressor, sizeUncompressed);

							}
						}

					}
					else
					{
						data.TryReadVarInt(out int id, out int len);

						mainData = data.Slice(len);
					}


					count += mainData.Length;

					if (rented is not null)
					{
						ArrayPool<byte>.Shared.Return(rented);
					}
				}

				if (count != TotalBytes)
				{
					throw new Exception($"данные не прочитаны полностью: Должно быть: {TotalBytes} Прочитано: {count}");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private async Task FillPipe()
		{
			using var fs = File.OpenRead("data.bin");
			try
			{
				while (true)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(512);
					try
					{
						int count = await fs.ReadAsync(memory);
						if (count == 0)
							return;

						pipe.Writer.Advance(count);
					}
					catch
					{

					}


					FlushResult flushResult = await pipe.Writer.FlushAsync();

					if (flushResult.IsCanceled || flushResult.IsCompleted)
						return;

				}
			}
			finally
			{
				await pipe.Writer.CompleteAsync();
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ReadOnlySequence<byte> DecompressCore(ReadOnlySequence<byte> compressed, byte[] decompressed, ZlibDecompressor decompressor, int sizeUncompressed)
		{
			//byte[] compressedTemp = ArrayPool<byte>.Shared.Rent((int)compressed.Length);

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
				throw new Exception("Zlib");

			int id = decompressedSpan.ReadVarInt(out int len);



			return new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);

		}


	}


}
