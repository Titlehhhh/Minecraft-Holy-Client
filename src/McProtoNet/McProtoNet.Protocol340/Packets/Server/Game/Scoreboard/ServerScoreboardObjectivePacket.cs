namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerScoreboardObjectivePacket : MinecraftPacket
    {


        //this.name = in.readString();
        //this.action = MagicValues.key(ObjectiveAction.class, in.readByte());
        //if(this.action == ObjectiveAction.ADD || this.action == ObjectiveAction.UPDATE) {
        //this.displayName = in.readString();
        //this.type = MagicValues.key(ScoreType.class, in.readString());
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerScoreboardObjectivePacket() { }
    }

}
