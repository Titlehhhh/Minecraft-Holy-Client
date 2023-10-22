namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientCloseWindowPacket : MinecraftPacket
    {
        public byte WindowId { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteUnsignedByte(WindowId);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            WindowId = stream.ReadUnsignedByte();
        }
        public ClientCloseWindowPacket() { }

        public ClientCloseWindowPacket(byte WindowId)
        {
            this.WindowId = WindowId;
        }
    }
}
