using McProtoNet.Protocol340.Data;
using McProtoNet.Protocol340.Data.World.Chunk;
using McProtoNet.Protocol340.Util;

namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerMultiBlockChangePacket : MinecraftPacket
    {
        public BlockChangeRecord[] Records { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            int chunkX = stream.ReadInt();
            int chunkZ = stream.ReadInt();
            Records = new BlockChangeRecord[stream.ReadVarInt()];
            for (int index = 0; index < this.Records.Length; index++)
            {
                short pos = stream.ReadShort();
                BlockState block = stream.ReadBlockState();
                int x = (chunkX << 4) + (pos >> 12 & 15);
                int y = pos & 255;
                int z = (chunkZ << 4) + (pos >> 8 & 15);
                Records[index] = new BlockChangeRecord(new Vector3(x, y, z), block);
            }
        }

        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }

        public ServerMultiBlockChangePacket() { }
    }

}
