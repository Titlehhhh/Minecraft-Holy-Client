using McProtoNet.Core.Protocol;
using System;
using System.IO;
using System.Threading.Tasks;



namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(true)]
	public class OldReadBenchmarks
	{
		[Params(0, 256)]
		public int CompressionThreshold { get; set; }

		[Params(64, 256, 512, 1024)]
		public int PacketSize { get; set; }

		[Params(1, 1000, 1_000_000)]
		public int Count { get; set; }

		private MinecraftPacketReader reader;
		private MemoryStream ms;

		//private Packet packet;

		[GlobalSetup]
		public void Setup()
		{
			reader = new();
			ms = new(1024);

			byte[] data = new byte[PacketSize];

			Random.Shared.NextBytes(data);

			var packet = new Packet(3, new MemoryStream(data));

			var sender = new MinecraftPacketSender(ms);

			ms.Position = 0;
			sender.SendPacketAsync(packet).GetAwaiter().GetResult();
		}

		[Benchmark]
		public async ValueTask Send()
		{
			reader.BaseStream = ms;
			for (int i = 0; i < Count; i++)
			{
				ms.Position = 0;
				var packet = await reader.ReadNextPacketAsync();

				packet.Dispose();
			}
		}
	}

	[MemoryDiagnoser(true)]
	public class NewReadBenchmarks
	{
		[Params(0, 256)]
		public int CompressionThreshold { get; set; }

		[Params(64, 256, 512, 1024)]
		public int PacketSize { get; set; }

		[Params(1, 1000, 1_000_000)]
		public int Count { get; set; }

		private MinecraftPacketReaderNew reader;
		private MemoryStream ms;

		//private Packet packet;

		[GlobalSetup]
		public void Setup()
		{
			reader = new();
			ms = new(1024);

			byte[] data = new byte[PacketSize];

			Random.Shared.NextBytes(data);

			var packet = new Packet(3, new MemoryStream(data));

			var sender = new MinecraftPacketSender(ms);

			ms.Position = 0;
			sender.SendPacketAsync(packet).GetAwaiter().GetResult();
		}

		[Benchmark]
		public async ValueTask Send()
		{
			reader.BaseStream = ms;
			for (int i = 0; i < Count; i++)
			{
				ms.Position = 0;
				var packet = await reader.ReadNextPacketAsync();

				packet.Dispose();
			}
		}
	}

}
