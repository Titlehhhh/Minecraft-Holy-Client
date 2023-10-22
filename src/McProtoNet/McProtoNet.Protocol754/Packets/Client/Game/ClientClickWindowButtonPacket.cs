namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientClickWindowButtonPacket : MinecraftPacket
    {
        public byte WindowId { get; private set; }
        public byte ButtonId { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteUnsignedByte(WindowId);
            stream.WriteUnsignedByte(ButtonId);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            WindowId = stream.ReadUnsignedByte();
            ButtonId = stream.ReadUnsignedByte();
        }
        public ClientClickWindowButtonPacket() { }

        public ClientClickWindowButtonPacket(byte WindowId, byte ButtonId)
        {
            this.WindowId = WindowId;
            this.ButtonId = ButtonId;
        }
    }
}
