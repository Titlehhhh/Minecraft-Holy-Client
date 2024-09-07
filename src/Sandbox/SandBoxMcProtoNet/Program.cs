// See https://aka.ms/new-console-template for more information

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
}