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

		private Stream mainStream;
		private MinecraftPacketReader native_reader;
		private MinecraftPacketReader pipelines_reader;
		private MinecraftPacketReaderNew pipelines_reader_new;
	

		private Pipe pipe;
		private PipeReader pipeReader2;

		[GlobalSetup]
		public async Task Setup()
		{

			mainStream = new MemoryStream();

			var sender = new MinecraftPacketSender();
			sender.BaseStream = mainStream;
			for (int i = 0; i < 1_000_000; i++)
			{
				var data = new byte[128];

				MemoryStream ms = new MemoryStream(data);

				ms.Position = 0;


				var packet = new Packet(15, ms);

				await sender.SendPacketAsync(packet);
			}
			mainStream.Position = 0;


			//var fs = File.OpenWrite("data.bin");

			//await mainStream.CopyToAsync(fs);
			//await fs.FlushAsync();
			//fs.Position = 0;
			//mainStream = fs;


			native_reader = new MinecraftPacketReader();


			native_reader.BaseStream = mainStream;

			pipelines_reader = new MinecraftPacketReader();

			pipe = new Pipe(new PipeOptions())
			{

			};

			pipelines_reader.BaseStream = pipe.Reader.AsStream();

			pipelines_reader_new = new();
			pipelines_reader_new.BaseStream = pipe.Reader.AsStream();

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


		[Benchmark]
		public async Task Read()
		{
			mainStream.Position = 0;

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
		[Benchmark]
		public async Task ReadWithPipelines()
		{

			var fill = FillPipe();
			var read = ReadPipe();

			await Task.WhenAll(fill, read);
			pipe.Reset();
		}

		[Benchmark]
		public async Task ReadWithPipelinesNew()
		{

			var fill = FillPipe();
			var read = ReadPipeNew();

			await Task.WhenAll(fill, read);
			pipe.Reset();
		}
		private async Task ReadPipeNew()
		{
			mainStream.Position = 0;
			int count = 0;
			try
			{
				while (true)
				{
					using var packet = await pipelines_reader_new.ReadNextPacketAsync();
					count++;
				}
			}
			catch
			{
				if (count != 1_000_000)
					throw new Exception(count.ToString());
			}
			finally
			{
				await pipe.Reader.CompleteAsync();
			}

		}
		private async Task ReadPipe()
		{
			mainStream.Position = 0;

			try
			{
				while (true)
				{
					using var packet = await pipelines_reader.ReadNextPacketAsync();
				}
			}
			catch
			{

			}
			finally
			{
				await pipe.Reader.CompleteAsync();
			}

		}

		[Benchmark]
		public async Task ReadWithPipelines2()
		{
			//mainStream.Position = 0;
			//pipeReader2 = PipeReader.Create(mainStream, new StreamPipeReaderOptions(leaveOpen: true));

			EmptyProcessor processor = new();
			using PacketPipeReader pipeReader = new PacketPipeReader(pipe.Reader, processor);


			var fill = FillPipe();
			var read =pipeReader.RunAsync();

			await Task.WhenAll(fill, read);
			pipe.Reset();

			if (processor.Count != 1_000_000)
			{
				throw new Exception(processor.Count.ToString());
			}
		}

		private async Task FillPipe()
		{
			mainStream.Position = 0;

			try
			{
				while (true)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(4096);


					int bytesRead = await mainStream.ReadAsync(memory);


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

	public sealed class EmptyProcessor : IPacketProcessor
	{
		public int Count { get; private set; }
		public void ProcessPacket(int id, ref ReadOnlySpan<byte> data)
		{
			Count++;
			//throw new NotImplementedException();
		}
	}

}
