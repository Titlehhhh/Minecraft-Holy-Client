using McProtoNet.NBT;
using McProtoNet.Protocol756.Data;
using System.Diagnostics;

namespace McProtoNet.Protocol756.Packets.Server
{


    public sealed class ServerJoinGamePacket : MinecraftPacket
    {
        private static byte GAMEMODE_MASK = 0x07;

        public int EntityId { get; set; }
        public bool Hardcore { get; set; }
        public GameMode GameMode { get; set; }
        public GameMode PreviousGamemode { get; set; }
        public int WorldCount { get; set; }
        public string[] WorldNames { get; set; }
        //public NbtCompound? DimensionCodec { get; set; }
        //public NbtCompound? Dimension { get; set; }
        //public string WorldName { get; set; }
        //public long HashedSeed { get; set; }
        //public int MaxPlayers { get; set; }
        //public int ViewDistance { get; set; }
        //public bool ReducedDebugInfo { get; set; }
        //public bool EnableRespawnScreen { get; set; }
        //public bool Debug { get; set; }
        //public bool Flat { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            //stream.WriteInt(this.EntityId);
            //stream.WriteBoolean(this.Hardcore);

            //stream.WriteUnsignedByte((byte)this.GameMode);
            //stream.WriteByte((sbyte)PreviousGamemode);
            //stream.WriteVarInt(this.WorldCount);
            //if (WorldNames is not null)
            //{
            //    foreach (string worldName in this.WorldNames)
            //        stream.WriteString(worldName);
            //}

            //stream.WriteNbt(this.DimensionCodec);
            //stream.WriteNbt(this.Dimension);
            //stream.WriteString(this.WorldName);

            //stream.WriteLong(this.HashedSeed);

            //stream.WriteVarInt(MaxPlayers);

            //stream.WriteVarInt(ViewDistance);

            //stream.WriteBoolean(ReducedDebugInfo);

            //stream.WriteBoolean(EnableRespawnScreen);

            //stream.WriteBoolean(Debug);

            //stream.WriteBoolean(Flat);
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            EntityId = stream.ReadInt();
            Hardcore = stream.ReadBoolean();
            GameMode = (GameMode)(stream.ReadUnsignedByte());
            PreviousGamemode = (GameMode)stream.ReadUnsignedByte();
            WorldCount = stream.ReadVarInt();
            WorldNames = new string[WorldCount];
            //for (int i = 0; i < WorldCount; i++)
            //{
            //    WorldNames[i] = stream.ReadString();
            //}


            //DimensionCodec = stream.ReadOptionalNbt();


            //Dimension = stream.ReadOptionalNbt();

            //WorldName = stream.ReadString();

            //HashedSeed = stream.ReadLong();

            //MaxPlayers = stream.ReadVarInt();

            //ViewDistance = stream.ReadVarInt();
            //ReducedDebugInfo = stream.ReadBoolean();
            //EnableRespawnScreen = stream.ReadBoolean();
            //Debug = stream.ReadBoolean();
            //Flat = stream.ReadBoolean();

        }
        public ServerJoinGamePacket() { }
    }
}

