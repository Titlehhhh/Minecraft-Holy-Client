

using McProtoNet.Client;
using System.Net.Sockets;

MinecraftLogin login = new MinecraftLogin();

TcpClient tcpClient = new TcpClient();

ushort port = 64451;

await tcpClient.ConnectAsync("127.0.0.1", port);

await login.Login(tcpClient.GetStream(), new LoginOptions("127.0.0.1", port, 754, "TestBox"));

await Task.Delay(-1);
