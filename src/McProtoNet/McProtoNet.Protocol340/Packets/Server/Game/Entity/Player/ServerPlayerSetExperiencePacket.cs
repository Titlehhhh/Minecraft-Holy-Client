namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerPlayerSetExperiencePacket : MinecraftPacket
    {


        //this.experience = in.readFloat();
        //this.level = in.readVarInt();
        //this.totalExperience = in.readVarInt();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerPlayerSetExperiencePacket() { }
    }

}
