namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerDisplayScoreboardPacket : MinecraftPacket
    {


        //this.position = MagicValues.key(ScoreboardPosition.class, in.readByte());
        //this.name = in.readString();
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerDisplayScoreboardPacket() { }
    }

}
