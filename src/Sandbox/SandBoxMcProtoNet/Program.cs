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
     await   MultipleConnections();
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
                    Host = "194.147.90.7",
                    Port = 25565,
                    Username = $"Adolf",
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
    
    


    public static void SwitchGenerator(string packet, string side)
    {
        int[] arr = null;

        if (side == "Clientbound")
        {
        }
    }


   
}