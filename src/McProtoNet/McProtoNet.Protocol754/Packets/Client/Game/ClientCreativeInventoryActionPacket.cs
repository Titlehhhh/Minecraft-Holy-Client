using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Client
{


    public sealed class ClientCreativeInventoryActionPacket : MinecraftPacket
    {
        public short Slot { get; private set; }
        public ItemStack Item { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteShort(Slot);
            stream.WriteItem(Item);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public ClientCreativeInventoryActionPacket(short slot, ItemStack item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            Slot = slot;
            Item = item;
        }

        public ClientCreativeInventoryActionPacket() { }


    }
}
