using System;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace McProtoNet.Benchmark;

[MemoryDiagnoser()]
public class OldReadBenchmarks
{
    private MemoryStream ms;

    private MinecraftPacketReader reader;

    [Params(0, 256)] public int CompressionThreshold { get; set; }

    [Params(64, 512)] public int PacketSize { get; set; }

    [Params(1, 1_000_000)] public int Count { get; set; }

    //private Packet packet;

    [GlobalSetup]
    public void Setup()
    {
        reader = new();
        ms = new MemoryStream(1024);

        var data = new byte[PacketSize];

        Random.Shared.NextBytes(data);

        var packet = new Packet(3, new MemoryStream(data));

        var sender = new MinecraftPacketSender(ms);
        sender.SwitchCompression(CompressionThreshold);
        ms.Position = 0;
        sender.SendPacketAsync(packet).GetAwaiter().GetResult();
    }

    [Benchmark]
    public async ValueTask OldRead()
    {
        reader.BaseStream = ms;
        reader.SwitchCompression(CompressionThreshold);
        for (var i = 0; i < Count; i++)
        {
            ms.Position = 0;
            var packet = await reader.ReadNextPacketAsync();

            packet.Dispose();
        }
    }
}