namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayerUseBedPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.position = NetUtil.readPosition(in);
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayerUseBedPacket() { }
    }

}
