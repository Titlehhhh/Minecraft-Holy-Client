namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerRespawnPacket : MinecraftPacket
    {


        //this.dimension = in.readInt();
        //this.difficulty = MagicValues.key(Difficulty.class, in.readUnsignedByte());
        //this.gamemode = MagicValues.key(GameMode.class, in.readUnsignedByte());
        //this.worldType = MagicValues.key(WorldType.class, in.readString().toLowerCase());
        public override void Read(IMinecraftPrimitiveReader stream)
        {

        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerRespawnPacket() { }
    }

}
