namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerWindowPropertyPacket : MinecraftPacket
    {


        //this.windowId = in.readUnsignedByte();
        //this.property = in.readShort();
        //this.value = in.readShort();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerWindowPropertyPacket() { }
    }

}
