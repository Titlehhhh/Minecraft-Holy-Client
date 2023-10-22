
namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPluginMessagePacket : MinecraftPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }



        public ClientPluginMessagePacket()
        {

        }

        public ClientPluginMessagePacket(string channel, byte[] data)
        {
            Channel = channel;
            Data = data;
        }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeString(this.channel);
        //out.writeBytes(this.data);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Channel);
            stream.Write(Data);
        }
    }
}
