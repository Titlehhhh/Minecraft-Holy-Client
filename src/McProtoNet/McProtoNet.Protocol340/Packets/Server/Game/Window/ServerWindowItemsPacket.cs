namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerWindowItemsPacket : MinecraftPacket
    {


        //this.windowId = in.readUnsignedByte();
        //this.items = new ItemStack[in.readShort()];
        //for(int index = 0; index < this.items.length; index++) {
        //this.items[index] = NetUtil.readItem(in);
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerWindowItemsPacket() { }
    }

}
