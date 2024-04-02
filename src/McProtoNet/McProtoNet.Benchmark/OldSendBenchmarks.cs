using McProtoNet.Core.Protocol;
using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace McProtoNet.Benchmark
{

	[MemoryDiagnoser(true)]
	public class OldSendBenchmarks
	{
		[Params(0, 256)]
		public int CompressionThreshold { get; set; }

		[Params(64, 512)]
		public int PacketSize { get; set; }

		[Params(1, 1_000_000)]
		public int Count { get; set; }

		private MinecraftPacketSender sender;
		private MemoryStream ms;

		private Packet packet;

		[GlobalSetup]
		public void Setup()
		{
			sender = new();
			ms = new(1024);

			byte[] data = new byte[PacketSize];

			Random.Shared.NextBytes(data);

			packet = new Packet(3, new MemoryStream(data));
		}

		[Benchmark]
		public async ValueTask OldSend()
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
