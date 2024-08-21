using System;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace McProtoNet.Benchmark;

[MemoryDiagnoser()]
public class OldSendBenchmarks
{
    private MemoryStream ms;

    private Packet packet;

    private MinecraftPacketSender sender;

    [Params(0, 256)] public int CompressionThreshold { get; set; }

    [Params(64, 512)] public int PacketSize { get; set; }

    [Params(1, 1_000_000)] public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        sender = new();
        ms = new MemoryStream(1024);

        var data = new byte[PacketSize];

        Random.Shared.NextBytes(data);

        packet = new Packet(3, new MemoryStream(data));
    }

    [Benchmark]
    public async ValueTask OldSend()
    {
        sender.BaseStream = ms;
        sender.SwitchCompression(CompressionThreshold);
        for (var i = 0; i < Count; i++)
        {
            ms.Position = 0;
            await sender.SendPacketAsync(packet);
        }
    }
}