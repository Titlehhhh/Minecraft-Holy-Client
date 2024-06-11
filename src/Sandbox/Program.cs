

using McProtoNet.Client;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

Console.WriteLine("Start");

MinecraftClient minecraftClient = new MinecraftClient()
{
	Host = "127.0.0.1",
	Port = 5554,
	Username = "TestBot",
	Version = 754
};

minecraftClient.OnPacket += (s, e) =>
{
	//Console.WriteLine("Received: " + e.Id);
};

await minecraftClient.Start();

await Task.Delay(2000);

minecraftClient.Stop();


await Task.Delay(500);
await minecraftClient.Start();

await Task.Delay(-1);