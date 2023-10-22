namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerUpdateScorePacket : MinecraftPacket
    {


        //this.entry = in.readString();
        //this.action = MagicValues.key(ScoreboardAction.class, in.readVarInt());
        //this.objective = in.readString();
        //if(this.action == ScoreboardAction.ADD_OR_UPDATE) {
        //this.value = in.readVarInt();
        //}
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerUpdateScorePacket() { }
    }

}
