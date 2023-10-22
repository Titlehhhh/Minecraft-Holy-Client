namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientMoveItemToHotbarPacket : MinecraftPacket
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
        public ClientMoveItemToHotbarPacket() { }

        public ClientMoveItemToHotbarPacket(int Slot)
        {
            this.Slot = Slot;
        }
    }
}
