using BenchmarkDotNet.Attributes;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Buffers;
namespace McProtoNet.Benchmark
{
	//[MemoryDiagnoser(true)]
	//public class NewSendBenchmarks
	//{
	//	[Params(0, 256)]
	//	public int CompressionThreshold { get; set; }

	//	[Params(64, 512)]
	//	public int PacketSize { get; set; }

	//	[Params(1, 100_000)]
	//	public int Count { get; set; }


	//	private MemoryStream ms;

	//	private McProtoNet.Experimental.PacketOut packet;

	//	[GlobalSetup]
	//	public void Setup()
	//	{
	//		ms = new(PacketSize * Count + Count * 25);

	//		byte[] data = new byte[PacketSize];

	//		Random.Shared.NextBytes(data);

	//		packet = new McProtoNet.Experimental.PacketOut(0, data.Length, data, null);
	//	}

	//	[Benchmark]
	//	public async ValueTask NewSend()
	//	{
	//		ms.Position = 0;
	//		using MinecraftPacketSenderNew sender = new();
	//		sender.BaseStream = ms;
	//		sender.SwitchCompression(CompressionThreshold);
	//		for (int i = 0; i < Count; i++)
	//		{
	//			ms.Position = 0;
	//			await sender.SendPacketAsync(packet);
	//		}
	//	}
	//}

}
