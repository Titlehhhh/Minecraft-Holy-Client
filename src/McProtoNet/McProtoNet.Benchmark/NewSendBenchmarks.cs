using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Buffers;
using McProtoNet.Experimental;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(true)]
	public class NewSendBenchmarks
	{
		[Params(0, 256)]
		public int CompressionThreshold { get; set; }

		[Params(64, 512)]
		public int PacketSize { get; set; }

		[Params(1, 100_000)]
		public int Count { get; set; }

		private MinecraftPacketSenderNew sender;
		private MemoryStream ms;

		private McProtoNet.Experimental.PacketOut packet;

		[GlobalSetup]
		public void Setup()
		{
			sender = new();
			ms = new(1024);

			byte[] data = new byte[PacketSize];

			Random.Shared.NextBytes(data);

			packet = new McProtoNet.Experimental.PacketOut(0, data.Length, data, ArrayPool<byte>.Shared);
		}

		[Benchmark]
		public async ValueTask NewSend()
		{
			sender.BaseStream = ms;
			sender.SwitchCompression(CompressionThreshold);
			for (int i = 0; i < Count; i++)
			{
				ms.Position = 0;
				await sender.SendPacketAsync(packet);
			}
		}
	}

}
