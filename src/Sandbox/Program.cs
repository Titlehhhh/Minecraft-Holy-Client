

using McProtoNet.Client;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

Console.WriteLine("Start");

MinecraftClient minecraftClient = new MinecraftClient()
{
	Host = "127.0.0.1",
	Port = 4411,
	Username = "TestBot1",
	Version = 754
};

minecraftClient.PacketReceived += (s, e) =>
{
	//Console.WriteLine("Received: " + e.Id);
};

await minecraftClient.Start();

await Task.Delay(1000);

//await minecraftClient.SendTest();

await Task.Delay(-1);