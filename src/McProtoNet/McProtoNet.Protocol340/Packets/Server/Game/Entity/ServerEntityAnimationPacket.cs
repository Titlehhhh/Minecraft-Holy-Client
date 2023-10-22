namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerEntityAnimationPacket : MinecraftPacket
    {


        //this.entityId = in.readVarInt();
        //this.animation = MagicValues.key(Animation.class, in.readUnsignedByte());
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerEntityAnimationPacket() { }
    }

}
