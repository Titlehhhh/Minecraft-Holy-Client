namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerConfirmTransactionPacket : MinecraftPacket
    {
        public byte WindowId { get; private set; }
        public ushort ActionId { get; private set; }
        public bool Accepted { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            WindowId = stream.ReadUnsignedByte();
            ActionId = stream.ReadUnsignedShort();
            Accepted = stream.ReadBoolean();
        }
        public ServerConfirmTransactionPacket() { }
    }
}

