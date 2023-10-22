namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityMetadataPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.metadata = NetUtil.readEntityMetadata(in);
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityMetadataPacket() { }
    }

}
