namespace McProtoNet.Protocol754.Packets.Client
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

        }
        public ClientPluginMessagePacket() { }


    }
}
