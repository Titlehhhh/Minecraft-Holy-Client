namespace McProtoNet.Protocol754.Packets.Server
{

    public sealed class LoginPluginRequestPacket : MinecraftPacket
    {
        public int MessageID { get; set; }
        public string Channel { get; set; }
        public byte[] Data { get; set; }

        public override void Read(IMinecraftPrimitiveReader stream)
        {

            MessageID = stream.ReadVarInt();
            Channel = stream.ReadString();
            Data = stream.ReadToEnd();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public LoginPluginRequestPacket()
        {

        }
    }
}
