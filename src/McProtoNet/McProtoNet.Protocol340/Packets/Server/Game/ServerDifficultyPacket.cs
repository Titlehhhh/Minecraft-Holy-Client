namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerDifficultyPacket : MinecraftPacket
    {


        //this.difficulty = MagicValues.key(Difficulty.class, in.readUnsignedByte());
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerDifficultyPacket() { }
    }

}
