// See https://aka.ms/new-console-template for more information

using McProtoNet.Client;

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
                
               // Console.WriteLine("Recv: "+packet.Id);
            };
            await client.Start();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await Task.Delay(-1);
    }
}