namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientSelectTradePacket : MinecraftPacket
    {
        public int Slot { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteVarInt(Slot);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Slot = stream.ReadVarInt();
        }
        public ClientSelectTradePacket() { }

        public ClientSelectTradePacket(int Slot)
        {
            this.Slot = Slot;
        }
    }
}
