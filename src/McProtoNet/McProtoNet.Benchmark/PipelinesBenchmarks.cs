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
		private MinecraftPacketReaderNew native_reader;



		[Params(128)]
		public int CompressionThreshold { get; set; }
		[Params(100_000)]
		public int PacketsCount { get; set; }

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

				Random.Shared.NextBytes(data);

				MemoryStream ms = new MemoryStream(data);

				ms.Position = 0;


				var packet = new Packet(Random.Shared.Next(0, 60), ms);

				await sender.SendPacketAsync(packet);
			}
			await mainStream.FlushAsync();
			//mainStream.Position = 0;


			//using var fs = File.OpenWrite("data.bin");

			//await File.WriteAllBytesAsync("data.bin", mainStream.ToArray());
			//fs.Position = 0;
			//mainStream = fs;


			native_reader = new MinecraftPacketReaderNew();


			//native_reader.BaseStream = mainStream;





			//pipeReader2 = PipeReader.Create(mainStream, readerOptions: new StreamPipeReaderOptions(leaveOpen: true));
		}
		[GlobalCleanup]
		public void Clean()
		{

		}


		[Benchmark]
		public async Task ReadNew()
		{
			using var fs = File.OpenRead("data.bin");
			native_reader.BaseStream = fs;
			native_reader.SwitchCompression(CompressionThreshold);

			for (int i = 0; i < PacketsCount; i++)
			{
				using var packet = await native_reader.ReadNextPacketAsync();

			}
		}




		[Benchmark]
		public async Task ReadWithPipelines2()
		{
			//mainStream.Position = 0;
			//pipeReader2 = PipeReader.Create(mainStream, new StreamPipeReaderOptions(leaveOpen: true));

			using var fs = File.OpenRead("data.bin");

			PipeReader reader = PipeReader.Create(fs);

			PacketPipeReader pipeReader = new PacketPipeReader(reader);

			//pipeReader.CompressionThreshold = CompressionThreshold;

			//var fill = FillPipe();
			await ProcessPackets(pipeReader);

			//await Task.WhenAll(fill, read);




			//pipe.Reset();
		}

		private async Task ProcessPackets(PacketPipeReader reader)
		{
			using ZlibDecompressor decompressor = new();
			await foreach (ReadOnlySequence<byte> data in reader.ReadPacketsAsync())
			{
				//SequenceReader<byte> reader1 = new SequenceReader<byte>(data);

				ReadOnlySequence<byte> mainData = default;
				byte[]? rented = null;
				if (CompressionThreshold > 0)
				{
					PacketPipeReader.TryReadVarInt(data, out int sizeUncompressed, out int len);
					ReadOnlySequence<byte> compressed = data.Slice(len);
					if (sizeUncompressed == 0)
					{

						PacketPipeReader.TryReadVarInt(data, out int id, out len);
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
								throw new Exception("Zlib");

							int id = PacketPipeReader.ReadVarInt(decompressed.AsSpan(), out len);



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
					PacketPipeReader.TryReadVarInt(data, out int id, out int len);

					mainData = data.Slice(len);
				}


				if (rented is not null)
				{
					ArrayPool<byte>.Shared.Return(rented);
				}

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

			int id = PacketPipeReader.ReadVarInt(decompressedSpan, out int len);



			return new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);

		}


	}


}
