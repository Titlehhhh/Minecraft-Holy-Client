using BenchmarkDotNet.Attributes;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(false)]
	public class PipelinesBenchmarks
	{

		private Stream mainStream;
		private MinecraftPacketReader native_reader;
		private MinecraftPacketReader pipelines_reader;

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
				var data = new byte[512];

				MemoryStream ms = new MemoryStream(data);

				ms.Position = 0;


				var packet = new Packet(15, ms);

				await sender.SendPacketAsync(packet);
			}

			//var fs = File.OpenWrite("data.bin");
			mainStream.Position = 0;
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

			pipeReader2 = PipeReader.Create(mainStream);
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
		public Task ReadWithPipelines()
		{
			var fill = FillPipe();
			var read = ReadPipe();

			return Task.WhenAll(fill, read);
		}
		[Benchmark]
		public async Task ReadWithPipelines2()
		{
			EmptyProcessor processor = new();
			using PacketPipeReader pipeReader = new PacketPipeReader(pipeReader2, processor);

			await pipeReader.RunAsync();
		}

		private async Task FillPipe()
		{
			try
			{
				while (true)
				{
					Memory<byte> memory = pipe.Writer.GetMemory(128);


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
		private async Task ReadPipe()
		{
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
	}

	public sealed class EmptyProcessor : IPacketProcessor
	{
		public void ProcessPacket(int id, ref ReadOnlySpan<byte> data)
		{
			//throw new NotImplementedException();
		}
	}

}
