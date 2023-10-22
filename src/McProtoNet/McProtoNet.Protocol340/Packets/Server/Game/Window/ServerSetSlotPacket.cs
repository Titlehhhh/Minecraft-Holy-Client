namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSetSlotPacket : MinecraftPacket
    {


        //this.windowId = in.readUnsignedByte();
        //this.slot = in.readShort();
        //this.item = NetUtil.readItem(in);
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSetSlotPacket() { }
    }

}
