namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayerAbilitiesPacket : MinecraftPacket
    {


        //byte flags = in.readByte();
        //this.invincible = (flags & 1) > 0;
        //this.canFly = (flags & 2) > 0;
        //this.flying = (flags & 4) > 0;
        //this.creative = (flags & 8) > 0;
        //this.flySpeed = in.readFloat();
        //this.walkSpeed = in.readFloat();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayerAbilitiesPacket() { }
    }

}
