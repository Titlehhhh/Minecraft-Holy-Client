using McProtoNet.Protocol754.Data;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerWindowItemsPacket : MinecraftPacket
    {
        public byte WindowId { get; private set; }
        public ItemStack?[] Items { get; private set; }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            WindowId = stream.ReadUnsignedByte();

            Items = new ItemStack?[stream.ReadShort()];
            for (int i = 0; i < Items.Length; i++)
            {

                Items[i] = stream.ReadItem();
            }
        }
        public ServerWindowItemsPacket() { }
    }
}

