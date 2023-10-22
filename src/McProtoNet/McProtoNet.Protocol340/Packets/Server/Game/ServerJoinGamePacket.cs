using McProtoNet.Protocol340.Data;

namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerJoinGamePacket : MinecraftPacket
    {



        public GameMode GameMode { get; private set; }

        public int EntityId { get; private set; }
        public bool Hardcore { get; private set; }
        public int Dimension { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public byte MaxPlayers { get; private set; }
        public string WorldType { get; private set; }
        public bool ReducedDebugInfo { get; private set; }



        //this.entityId = in.readInt();
        //int gamemode = in.readUnsignedByte();
        //this.hardcore = (gamemode & 8) == 8;
        //gamemode &= -9;
        //this.gamemode = MagicValues.key(GameMode.class, gamemode);
        //this.dimension = in.readInt();
        //this.difficulty = MagicValues.key(Difficulty.class, in.readUnsignedByte());
        //this.maxPlayers = in.readUnsignedByte();
        //this.worldType = MagicValues.key(WorldType.class, in.readString().toLowerCase());
        //this.reducedDebugInfo = in.readBoolean();
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            this.EntityId = stream.ReadInt();
            byte gamemode = stream.ReadUnsignedByte();
            this.Hardcore = (gamemode & 8) == 8;
            gamemode &= -0;
            this.GameMode = (GameMode)gamemode;
            this.Dimension = stream.ReadInt();
            this.Difficulty = (Difficulty)stream.ReadUnsignedByte();
            this.WorldType = stream.ReadString();
            this.ReducedDebugInfo = stream.ReadBoolean();
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerJoinGamePacket() { }
    }

}
