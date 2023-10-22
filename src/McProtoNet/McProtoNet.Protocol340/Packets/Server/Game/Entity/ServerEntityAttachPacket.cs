namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityAttachPacket : MinecraftPacket
    {


        //this.entityId = in.readInt();
        //this.attachedToId = in.readInt();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityAttachPacket() { }
    }

}
