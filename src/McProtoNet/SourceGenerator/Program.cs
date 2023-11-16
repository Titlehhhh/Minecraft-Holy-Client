using McProtoNet.Core;
using McProtoNet.Core.IO;
using McProtoNet.Core.Packets;
using McProtoNet.Core.Protocol;
using McProtoNet.HighPerfomance;
using McProtoNet.MultiVersion.Proxy;
using McProtoNet.Protocol754;
using McProtoNet.Protocol754.Packets.Client;
using McProtoNet.Protocol754.Packets.Server;
using McProtoNet.Utils;
using QuickProxyNet;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SourceGenerator
{
    internal class Program
    {
        static int connections = 0;
        static IPacketFactory p754 = new PacketFactory754();
        static IPacketProvider packetProvider;
        static async Task Main(string[] args)
        {
            packetProvider = p754.CreateProvider(PacketCategory.Game, PacketSide.Client);

            Console.WriteLine("Start");

            string host = "itski.aternos.me";
            ushort port = 25565;

            if (port == 25565)
            {
                try
                {
                    IServerResolver resolver = new ServerResolver();
                    var result = await resolver.ResolveAsync(host);
                    host = result.Host;
                    port = result.Port;
                }
                catch
                {

                }
            }

            ProxyClientFactory factory = new ProxyClientFactory();
            IProxyRepositoryFactory repositoryFactory = new ProxyRepositoryFactory(new IProxyProvider[]
            {
                new FileProxyProvider()
            });
            var proxies = await repositoryFactory.CreateAsync(default);

            var proxy = await proxies.GetProxyAsync();
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port);
            // var networkStream = await proxy.ConnectAsync(host, port);
            var networkStream = tcpClient.GetStream();
            Stream stream = await LoginAsync(networkStream, "Bot_");
            // await Task.Delay(5000);
            MinecraftProtocolFast minecraftProtocol = new MinecraftProtocolFast(stream, threshold);
            try
            {
                minecraftProtocol.OnReceived += MinecraftProtocol_OnReceived;
                connections++;
                await minecraftProtocol.Start(default);
            }
            finally
            {
                minecraftProtocol.OnReceived -= MinecraftProtocol_OnReceived;
            }




        }

        private static void MinecraftProtocol_OnReceived(Packet obj)
        {
            if (packetProvider.TryGetInputPacket(obj.Id, out var pack))
            {
                Console.WriteLine("ReadPacket: " + pack.GetType().Name);
            }
            obj.Data.Dispose();
        }
        static int threshold = 0;
        static async Task<Stream> LoginAsync(Stream networkStream, string nick)
        {
            MinecraftStream result = new MinecraftStream(networkStream);
            using (IMinecraftProtocol proto = new MinecraftProtocol(result, false))
            {
                await proto.SendPacketAsync(new HandShakePacket(HandShakeIntent.LOGIN, 754, "", 25565), 0);

                await proto.SendPacketAsync(new LoginStartPacket(nick), 0x00);

                bool ok = false;
                do
                {
                    Packet readResult = await proto.ReadNextPacketAsync(default);
                    IMinecraftPrimitiveReader reader = new MinecraftPrimitiveReader(readResult.Data);

                    ok = await HandleLogin(reader, readResult.Id, proto);
                } while (!ok);

            }
            return result;
        }
        private static async ValueTask<bool> HandleLogin(IMinecraftPrimitiveReader reader, int id, IMinecraftProtocol proto)
        {
            if (id == 0x02)
                return true;
            else if (id == 0x03)
            {
                threshold = reader.ReadVarInt();
                proto.SwitchCompression(threshold);
            }
            else if (id == 0x01)
            {
                reader.ReadString();
                byte[] publicKey = reader.ReadByteArray();
                byte[] verifyToken = reader.ReadByteArray();
                var RSAService = CryptoHandler.DecodeRSAPublicKey(publicKey);
                byte[] secretKey = CryptoHandler.GenerateAESPrivateKey();

                byte[] g = RSAService.Encrypt(secretKey, false);
                byte[] token = RSAService.Encrypt(verifyToken, false);
                await proto.SendPacketAsync(new EncryptionResponsePacket(g, token), 0x01);
                //SendEncrypt(g, token);
                proto.SwitchEncryption(secretKey);

            }
            else if (id == 0x00)
            {
                var r = reader.ReadString();
                Console.WriteLine("LoginKick: " + r);
                throw new LoginRejectedException(r);
            }
            else
            {
                throw new Exception("unkown packet: " + id);
            }
            return false;
        }
    }
}
