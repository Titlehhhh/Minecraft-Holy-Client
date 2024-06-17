using McProtoNet;
using McProtoNet.Client;
using McProtoNet.Protocol754;

Console.WriteLine("Start");


var minecraftClient = new MinecraftClient
{
    Host = "127.0.0.1",
    Port = 56057,
    Username = "TestBot3",
    Version = 754 // 1.16.5
};

Protocol_754 protocol = new Protocol_754(minecraftClient);

protocol.OnChatPacket.Subscribe(x => { Console.WriteLine(ChatParser.ParseText(x.Message)); });
protocol.OnKeepAlivePacket.Subscribe(x => { protocol.SendKeepAlive(x.KeepAliveId); });




await minecraftClient.Start();

await Task.Delay(1000);

Console.WriteLine("Start spam");
for (int i = 1; i <= 20; i++)
{
    await Task.Delay(500);
    await protocol.SendChat("asdasdasdasdasdadasdasd: " + i.ToString());
}

Thread.Sleep(-1);