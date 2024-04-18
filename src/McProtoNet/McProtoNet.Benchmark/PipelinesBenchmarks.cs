using BenchmarkDotNet.Attributes;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
using McProtoNet.Experimental;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(true)]
	public class PipelinesBenchmarks
	{
		private MinecraftPacketReaderNew native_reader;
		private MinecraftPacketReader pipelines_reader;
		private MinecraftPacketReaderNew pipelines_reader_new;


		private Pipe pipe;
		private PipeReader pipeReader2;

		[Params(256)]
		public int CompressionThreshold { get; set; }
		[Params(1_000_000)]
		public int PacketsCount { get; set; }

		[GlobalSetup]
		public async Task Setup()
		{

			var mainStream = new MemoryStream();

			var sender = new MinecraftPacketSender();
			sender.SwitchCompression(CompressionThreshold);
			sender.BaseStream = mainStream;
			for (int i = 0; i < PacketsCount; i++)
			{
				var data = new byte[512];

				MemoryStream ms = new MemoryStream(data);

				ms.Position = 0;


				var packet = new Packet(15, ms);

				await sender.SendPacketAsync(packet);
			}
			mainStream.Position = 0;


			//using var fs = File.OpenWrite("data.bin");

			await File.WriteAllBytesAsync("data.bin", mainStream.ToArray());
			//fs.Position = 0;
			//mainStream = fs;


			native_reader = new MinecraftPacketReaderNew();


			//native_reader.BaseStream = mainStream;

			pipelines_reader = new MinecraftPacketReader();

			pipe = new Pipe(new PipeOptions(readerScheduler: PipeScheduler.Inline, writerScheduler: PipeScheduler.Inline))
			{

			};

			pipelines_reader.BaseStream = pipe.Reader.AsStream();
			pipelines_reader.SwitchCompression(CompressionThreshold);
			pipelines_reader_new = new();
			pipelines_reader_new.BaseStream = pipe.Reader.AsStream();
			pipelines_reader_new.SwitchCompression(CompressionThreshold);

			//pipeReader2 = PipeReader.Create(mainStream, readerOptions: new StreamPipeReaderOptions(leaveOpen: true));
		}
		[GlobalCleanup]
		public void Clean()
		{
			try
			{
				pipe.Reset();
			}
			catch
			{

			}
		}


		//[Benchmark]
		public async Task ReadNew()
		{
			using var fs = File.OpenRead("data.bin");
			native_reader.BaseStream = fs;

			try
			{
				while (true)
				{
					using var packet = await native_reader.ReadNextPacketAsync();
				}
			}
			catch
			{

			}
		}
		//[Benchmark]
		public async Task ReadWithPipelines()
		{

			var fill = FillPipe();
			var read = ReadPipe();

			await Task.WhenAll(fill, read);
			pipe.Reset();
		}

		//[Benchmark]
		public async Task ReadWithPipelinesNew()
		{

			var fill = FillPipe();
			var read = ReadPipeNew();

			await Task.WhenAll(fill, read);
			pipe.Reset();
		}
		private async Task ReadPipeNew()
		{



			for (int i = 0; i < PacketsCount; i++)
			{


				using var packet = await pipelines_reader_new.ReadNextPacketAsync();
			}

			await pipe.Reader.CompleteAsync();

		}
		private async Task ReadPipe()
		{


			for (int i = 0; i < PacketsCount; i++)
			{


				using var packet = await pipelines_reader.ReadNextPacketAsync();
			}

			await pipe.Reader.CompleteAsync();


		}

		[Benchmark]
		public async Task ReadWithPipelines2()
		{
			//mainStream.Position = 0;
			//pipeReader2 = PipeReader.Create(mainStream, new StreamPipeReaderOptions(leaveOpen: true));

			using PacketPipeReader pipeReader = new PacketPipeReader(pipe.Reader);

			pipeReader.CompressionThreshold = CompressionThreshold;

			var fill = FillPipe();
			var read = pipeReader.RunAsync();



			int count = 0;

			await foreach (var result in read)
			{
				//result.Dispose();

				count++;

			}
			await fill;
			if (count != PacketsCount)
			{
				throw new Exception();
			}

			pipe.Reset();
		}

		private async Task FillPipe()
		{
			using var fs = File.OpenRead("data.bin");


			try
			{
				while (true)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(4096);


					int bytesRead = await fs.ReadAsync(memory);


					if (bytesRead <= 0)
					{
						break;
					}

					pipe.Writer.Advance(bytesRead);


					FlushResult result = await pipe.Writer.FlushAsync();
					if (result.IsCompleted)
					{
						break;
					}
					if (result.IsCanceled)
					{
						break;
					}


				}
			}
			catch
			{

			}
			finally
			{
				await pipe.Writer.CompleteAsync();
			}
		}

	}


}
