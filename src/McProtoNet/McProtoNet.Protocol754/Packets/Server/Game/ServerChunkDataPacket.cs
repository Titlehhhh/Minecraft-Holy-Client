using McProtoNet.NBT;

namespace McProtoNet.Protocol754.Packets.Server
{


    public sealed class ServerChunkDataPacket : MinecraftPacket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }
        public bool FullChunk { get; set; }
        public int PrimaryBitMask { get; set; }
        public NbtCompound Heightmaps { get; set; }
        public int[]? Biomes { get; set; }
        public byte[] Data { get; set; }

        public NbtCompound[] BlockEntities { get; set; }
        public override void Write(IMinecraftPrimitiveWriter stream)
        {
            stream.WriteInt(ChunkX);
            stream.WriteInt(ChunkZ);
            stream.WriteBoolean(FullChunk);
            stream.WriteVarInt(PrimaryBitMask);
            stream.WriteNbt(Heightmaps);
            if (FullChunk)
            {
                stream.WriteVarInt(Biomes.Length);
                foreach (var item in Biomes)
                {
                    stream.WriteVarInt(item);
                }
            }

            stream.WriteByteArray(Data);

            stream.WriteVarInt(BlockEntities.Length);
            foreach (var item in BlockEntities)
            {
                stream.WriteNbt(item);
            }
        }
        public override void Read(IMinecraftPrimitiveReader stream)
        {
            ChunkX = stream.ReadInt();
            ChunkZ = stream.ReadInt();
            FullChunk = stream.ReadBoolean();
            PrimaryBitMask = stream.ReadVarInt();
            Heightmaps = stream.ReadOptionalNbt();
            if (FullChunk)
            {
                Biomes = new int[stream.ReadVarInt()];
                for (int i = 0; i < Biomes.Length; i++)
                {
                    Biomes[i] = stream.ReadVarInt();
                }
            }
            else
            {
                Biomes = new int[0];
            }
            Data = stream.ReadByteArray();
            BlockEntities = new NbtCompound[stream.ReadVarInt()];
            for (int i = 0; i < BlockEntities.Length; i++)
            {
                BlockEntities[i] = stream.ReadOptionalNbt();
            }
        }

        public ServerChunkDataPacket() { }
    }
}

