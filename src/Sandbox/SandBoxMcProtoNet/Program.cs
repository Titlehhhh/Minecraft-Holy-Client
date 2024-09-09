// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using DotNext.Collections.Generic;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Serialization;

internal class Program
{
    public static async Task Main(string[] args)
    {
        SwitchGenerator("ServerboundKeepAlivePacket");
        
        return;
        Console.WriteLine("Start");
        try
        {
            MinecraftClient client = new MinecraftClient()
            {
                ConnectTimeout = TimeSpan.FromSeconds(30),
                Host = "192.168.1.98",
                Port = 25565,
                Username = "TestBot",
                Version = MinecraftVersion.Latest
            };

            client.PacketReceived += (sender, packet) =>
            {
                if (packet.Id == 0x1D)
                {
                    //scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    //var reason = reader.ReadNbt();
                    //Console.WriteLine();
                }
            };
            await client.Start();
            await Task.Delay(5000);
            var protoTest = new MultiProtocol(client);
            Console.WriteLine("Hi Send");
            await protoTest.SendChatPacket("Hi");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await Task.Delay(-1);
    }

    private static int GetPacket(int version, string packet)
    {
        return MultiProtocol.ServerboundPlayPackets(version).IndexOf(packet);
    }

    public static void SwitchGenerator(string packet)
    {
        List<string> lines = new List<string>();
        int previousId = GetPacket(340, packet);
        int left = 340;
        int newPacketId = -1;

        for (int version = 341; version <= 766; version++)
        {
            if (version == 757)
                Debugger.Break();
            int previousVersion = version - 1;
            newPacketId = GetPacket(version, packet);

            if (newPacketId != previousId)
            {
                if (version - left == 1)
                {
                    lines.Add($"{left} => {IntToHex(previousId)}");
                }
                else
                {
                    lines.Add($">= {left} and <= {previousVersion} => {IntToHex(previousId)}");
                }

                left = version;
            }

            previousId = newPacketId;
        }

        newPacketId = GetPacket(767, packet);
        if (newPacketId != previousId)
        {
            if (767 - left == 1)
            {
                lines.Add($"{left} => {IntToHex(previousId)}");
            }
            else
            {
                lines.Add($">= {left} and <= {767} => {IntToHex(previousId)}");
            }
        }
        else
        {
            lines.Add($">= {left} and <= {767} => {IntToHex(previousId)}");
        }

        lines.Add("_ => throw new Exception(\"Unknown protocol version\")");

        Console.WriteLine(string.Join(", \n", lines));
    }

    private static string IntToHex(int id)
    {
        return $"0x{id:X2}";
    }
}