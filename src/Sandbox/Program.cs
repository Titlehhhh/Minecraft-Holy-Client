

using McProtoNet.Client;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

Console.WriteLine("Start");

MinecraftClient minecraftClient = new MinecraftClient()
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