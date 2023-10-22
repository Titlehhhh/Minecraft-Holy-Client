namespace McProtoNet.Protocol756.Packets.Server
{


    public sealed class ServerDisconnectPacket : MinecraftPacket
    {
        public string Message { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Message);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Message = stream.ReadString();
        }
        public ServerDisconnectPacket() { }
    }
}

