// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using DotNext.Collections.Generic;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Serialization;
using QuickProxyNet;

internal class Program
{
    public static async Task Main(string[] args)
    {
        byte[] bytes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        Console.WriteLine(bytes.AsSpan().IndexOf(new byte[]{3,4,5,6}));
        //return;
        
        
        HttpProxyClient proxyClient = new HttpProxyClient("84.252.75.136", 4444);

        await proxyClient.ConnectAsync("google.com", 443);

        return;

        Console.WriteLine("Start");
        try
        {
            var list = new List<MinecraftClient>();
            var listProtocols = new List<MultiProtocol>();
            for (int i = 0; i < 200; i++)
            {
                MinecraftClient client = new MinecraftClient()
                {
                    ConnectTimeout = TimeSpan.FromSeconds(30),
                    Host = "192.168.0.7",
                    Port = 25565,
                    Username = $"TitleBot_{i + 1:D3}",
                    Version = MinecraftVersion.Latest
                };
                client.StateChanged += (sender, eventArgs) =>
                {
                    if (eventArgs.Error is not null)
                    {
                        Console.WriteLine(eventArgs.Error);
                    }
                };
                var protoTest = new MultiProtocol(client);
                listProtocols.Add(protoTest);
                list.Add(client);
            }

            List<Task> tasks = new List<Task>();
            int index = 0;
            await foreach (var minecraftClient in list)
            {
                static async Task RunBot(MinecraftClient client, MultiProtocol proto)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            await client.Start();
                            await proto.OnJoinGame.FirstOrDefaultAsync();
                            break;
                        }
                        catch (Exception e)
                        {
                            // Console.WriteLine(e);
                        }
                    }
                }

                tasks.Add(RunBot(minecraftClient, listProtocols[index++]));
            }

            await Task.WhenAll(tasks);
            //while (true)
            {
                await Task.Delay(5000);
                var sends = listProtocols.Select(async b =>
                {
                    try
                    {
                        await b.SendChatPacket("Hello from Minecraft Holy Client");
                    }
                    catch
                    {
                        // ignored
                    }
                });
                await Task.WhenAll(sends);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // throw;
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