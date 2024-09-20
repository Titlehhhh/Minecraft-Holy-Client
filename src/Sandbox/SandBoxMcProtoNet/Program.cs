// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;
using DotNext.Collections.Generic;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Serialization;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Start");
        try
        {
            MinecraftClient client = new MinecraftClient()
            {
                ConnectTimeout = TimeSpan.FromSeconds(30),
                Host = "192.168.0.7",
                Port = 25565,
                Username = "TestBot",
                Version = MinecraftVersion.Latest
            };

            var protoTest = new MultiProtocol(client);
            await client.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await Task.Delay(-1);
    }

    private static void NewMethod()
    {
        List<KeyValuePair<int, int>> list = new();
        for (int i = 340; i <= 767; i++)
        {
            var g = GetClientboundPlayPacket(i, "DisconnectPacket");
            list.Add(new KeyValuePair<int, int>(i, g));
        }

        var ranges = list.GroupAdjacent(c => c.Value)
            .Select(g => new VersionRange(
                g.Min(c => c.Key),
                g.Max(c => c.Key),
                g.Key));


        foreach (var r in ranges)
        {
            Console.WriteLine(r.ToSwitchCaseSend() + ",");
        }


        return;
    }

    private static int GetServerboundPlayPacket(int version, string packet)
    {
        return Ext.ServerboundPlayPackets(version).IndexOf("Serverbound" + packet);
    }

    private static int GetClientboundPlayPacket(int version, string packet)
    {
        return Ext.ClientboundPlayPackets(version).IndexOf("Clientbound" + packet);
    }


    public static void SwitchGenerator(string packet, string side)
    {
        int[] arr = null;

        if (side == "Clientbound")
        {
        }
    }


    class VersionRange
    {
        public int Min { get; }
        public int Max { get; }
        public int Id { get; }

        public VersionRange(int min, int max, int id)
        {
            Min = min;
            Max = max;
            Id = id;
        }

        public string ToSwitchCaseSend()
        {
            if (Min != Max)
            {
                return $">= {Min} and <= {Max} => 0x{Id:X2}";
            }

            return $"{Min} => 0x{Id:X2}";
        }

        public override string ToString()
        {
            return $"From {Min} to {Max} id {Id}";
        }
    }
}