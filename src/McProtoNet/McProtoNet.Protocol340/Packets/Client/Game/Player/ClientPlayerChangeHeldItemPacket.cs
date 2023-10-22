namespace McProtoNet.Protocol340.Packets.Client.Game
{


    public sealed class ClientPlayerChangeHeldItemPacket : MinecraftPacket
    {
        public short Slot { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        //out.writeShort(this.slot);
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteShort(Slot);
        }
        public ClientPlayerChangeHeldItemPacket()
        {

        }

        public ClientPlayerChangeHeldItemPacket(short slot)
        {
            Slot = slot;
        }
    }
}
