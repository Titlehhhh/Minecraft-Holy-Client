namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerSpawnParticlePacket : MinecraftPacket
    {


        //this.particle = MagicValues.key(Particle.class, in.readInt());
        //this.longDistance = in.readBoolean();
        //this.x = in.readFloat();
        //this.y = in.readFloat();
        //this.z = in.readFloat();
        //this.offsetX = in.readFloat();
        //this.offsetY = in.readFloat();
        //this.offsetZ = in.readFloat();
        //this.velocityOffset = in.readFloat();
        //this.amount = in.readInt();
        //this.data = new int[this.particle.getDataLength()];
        //for(int index = 0; index < this.data.length; index++) {
        //this.data[index] = in.readVarInt();
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerSpawnParticlePacket() { }
    }

}
