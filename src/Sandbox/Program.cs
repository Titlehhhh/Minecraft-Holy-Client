using McProtoNet.Client;

Console.WriteLine("Start");

var minecraftClient = new MinecraftClient
{
    Host = "127.0.0.1",
    Port = 7740,
    Username = "TestBot",
    Version = 765
};

minecraftClient.OnPacket += (s, e) =>
{
    //Console.WriteLine("Received: " + e.Id);
};


await minecraftClient.Start();

await Task.Delay(-1);