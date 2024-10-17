// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Text;
using DiscordRPC;
using DotNext.Collections.Generic;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Protocol;
using McProtoNet.Serialization;
using QuickProxyNet;

internal class Program
{
    public static async Task Main(string[] args)
    {
        HttpsProxyClient proxyClient = new HttpsProxyClient("141.105.107.152", 5678);

        await proxyClient.ConnectAsync("45.136.204.198", 25565);
        Console.WriteLine("Start");
        return;
        await BowBots();
    }

    private static async Task BowBots()
    {
        List<BowBot> bots = new();
        for (int i = 0; i < 20; i++)
        {
            bots.Add(new BowBot($"TitleBot_{i:D3}"));
        }

        var tasks = bots.Select(x => x.Run());
        await Task.WhenAll(tasks);
        await Task.Delay(-1);
    }

    private static async Task MultipleConnections()
    {
        var list = new List<MinecraftClient>();
        try
        {
            var listProtocols = new List<MultiProtocol>();
            for (int i = 0; i < 1; i++)
            {
                MinecraftClient client = new MinecraftClient()
                {
                    ConnectTimeout = TimeSpan.FromSeconds(30),
                    Host = "127.0.0.1",
                    Port = 25565,
                    Username = $"TitleBot_{i + 1:D3}",
                    Version = MinecraftVersion.Latest
                };
                client.Disconnected += async (sender, eventArgs) =>
                {
                    if (eventArgs.Exception is not null)
                    {
                        Console.WriteLine("Errored: " + eventArgs.Exception.Message);
                        Console.WriteLine(eventArgs.Exception.StackTrace);
                        Console.WriteLine("Restart");
                        try
                        {
                            await client.Start();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Start: " + e);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Stopped");
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
                            //await proto.OnJoinGame.FirstOrDefaultAsync();
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }

                tasks.Add(RunBot(minecraftClient, listProtocols[index++]));
            }


            await Task.WhenAll(tasks);

            await Task.Delay(1000);
            var sends = listProtocols.Select(async b =>
            {
                try
                {
                    await b.SendChatPacket("Hello from Minecraft Holy Client");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("SendErr: " + exception);
                    // ignored
                }
            });
            await Task.WhenAll(sends);
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
            var g = GetServerboundPlayPacket(i, "MovePlayerPacketRot");
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