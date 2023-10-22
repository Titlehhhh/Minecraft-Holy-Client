namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityHeadLookPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.headYaw = in.readByte() * 360 / 256f;
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityHeadLookPacket() { }
    }

}
