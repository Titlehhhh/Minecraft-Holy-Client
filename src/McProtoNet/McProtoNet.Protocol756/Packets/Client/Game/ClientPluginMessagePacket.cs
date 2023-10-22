using System.Text;

namespace McProtoNet.Protocol756.Packets.Client
{


    public sealed class ClientPluginMessagePacket : MinecraftPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Channel);
            stream.Write(Data);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Channel = stream.ReadString();
            Data = stream.ReadToEnd();
        }
        public ClientPluginMessagePacket() { }
        public override string ToString()
        {
            return $"Channel: {Channel} Data: {Encoding.UTF8.GetString(Data)}";
        }

    }
}
