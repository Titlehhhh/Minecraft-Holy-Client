namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerPluginMessagePacket : MinecraftPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Channel = stream.ReadString();
            Data = stream.ReadToEnd();
        }
        public ServerPluginMessagePacket() { }
    }
}

