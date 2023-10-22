using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerSetSlotPacket : MinecraftPacket
    {
        public byte WindowId { get; private set; }
        public short Slot { get; private set; }
        public ItemStack? Item { get; private set; }


        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            WindowId = stream.ReadUnsignedByte();
            Slot = stream.ReadShort();
            Item = stream.ReadItem();
        }
        public ServerSetSlotPacket() { }
    }
}

