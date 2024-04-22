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

		public int PacketsCount { get; set; } = 100_000;

		private Pipe pipe;

		private long TotalBytes;

		[GlobalSetup]
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
		[GlobalCleanup]
		public void Clean()
		{

		}
		[Benchmark]
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
		[Benchmark]
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




		[Benchmark]
		public async Task ReadWithPipelines()
		{

			MinecraftPacketPipeReader pipeReader = new MinecraftPacketPipeReader(pipe.Reader);

			var fill = FillPipe();
			var read = ProcessPackets(pipeReader);



			await Task.WhenAll(fill, read);



			pipe.Reset();

		}
		private async Task ProcessPackets(MinecraftPacketPipeReader reader)
		{
			int count = 0;
			await foreach (var packet in reader.ReadPacketsAsync().Decompress(CompressionThreshold))
			{
				count++;
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





	}
}
