// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Text;
using DiscordRPC;
using DotNext;
using DotNext.Buffers;
using DotNext.Collections.Generic;
using DotNext.IO;
using McProtoNet;
using McProtoNet.Abstractions;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Net;
using McProtoNet.Protocol;
using McProtoNet.Serialization;
using QuickProxyNet;
using QuickProxyNet.ProxyChecker;

internal class Program
{
    private static async Task<List<ProxyRecord>> ParseProxy(string url, ProxyType type)
    {
        List<ProxyRecord> records = new();
        using HttpClient httpClient = new HttpClient();
        await using var stream = await httpClient.GetStreamAsync(url);
        using var sr = new StreamReader(stream);
        
        while (!sr.EndOfStream)
        {
            string? line = await sr.ReadLineAsync();

            if (!string.IsNullOrEmpty(line))
            {
                var hostport = line.Split(':');
                try
                {
                    ProxyRecord record = new ProxyRecord(type, hostport[0], int.Parse(hostport[1]), null);
                    records.Add(record);
                }
                catch
                {
                    // ignored
                }
            }
        }

        return records;
    }

    public static async Task Main(string[] args)
    {
        HashSet<ProxyRecord> records = new HashSet<ProxyRecord>();

        foreach (var type in Enum.GetValues<ProxyType>())
        {
            try
            {
                string typeStr = type.ToString().ToLower();
                string url =
                    $"https://raw.githubusercontent.com/SevenworksDev/proxy-list/refs/heads/main/proxies/{typeStr}.txt";
                records.AddAll(await ParseProxy(url, type));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        Console.WriteLine($"Parse {records.Count} proxies");

        var checker = ProxyChecker.CreateChunked(records, new ProxyCheckerChunkedOptions()
        {
            QueueSize = 100,
            InfinityLoop = false,
            IsSingleConsumer = true,
            SendAlive = false,
            ChunkSize = 30_000,
            ConnectTimeout = TimeSpan.FromSeconds(10),
            TargetHost = "google.com",
            TargetPort = 443
        });

        Task t = checker.Start();

        Task read = Task.Run(async () =>
        {
            await using (StreamWriter sw = new StreamWriter("proxies.txt", true))
            {
                try
                {
                    var proxy = await checker.GetNextProxy(default);
                    Console.WriteLine($"PROXY! {proxy.Type} GetType(): {proxy.GetType()}");
                    await sw.WriteLineAsync($"{proxy.Type.ToString().ToLower()}://{proxy.ProxyHost}:{proxy.ProxyPort}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        });

        await Task.WhenAll(t, read);

        return;

        MinecraftClient client = new MinecraftClient()
        {
            ConnectTimeout = TimeSpan.FromSeconds(30),
            Host = "94.130.3.102",
            Port = 25845,
            Username = $"TTT",
            Version = MinecraftVersion.Latest
        };
        await client.Start();
        await Task.Delay(-1);
        return;

        await MultipleConnections();
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
                    Host = "94.130.3.102",
                    Port = 25845,
                    Username = $"TTT",
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