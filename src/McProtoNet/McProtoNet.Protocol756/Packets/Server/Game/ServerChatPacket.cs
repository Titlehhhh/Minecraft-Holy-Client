namespace McProtoNet.Protocol756.Packets.Server
{

    public sealed class ServerChatPacket : MinecraftPacket
    {
        public string Message { get; set; }
        public byte Position { get; set; }
        public Guid Sender { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteString(Message);
            stream.WriteUnsignedByte(Position);
            stream.WriteUuid(Sender);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Message = stream.ReadString();
            Position = stream.ReadUnsignedByte();
            Sender = stream.ReadUUID();
        }
        public ServerChatPacket() { }
    }
}

