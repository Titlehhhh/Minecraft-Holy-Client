using McProtoNet.Protocol340.Data.World.Chunk;

namespace McProtoNet.Protocol340.Packets.Server
{


    public sealed class ServerChunkDataPacket : MinecraftPacket
    {
        public int CurrentDimension { get; set; }

        public int X { get; private set; }
        public int Z { get; private set; }
        public ChunkColumn Column { get; private set; }



        public override void Read(IMinecraftPrimitiveReader stream)
        {
            Column = new ChunkColumn(16);
            X = stream.ReadInt();
            Z = stream.ReadInt();


            bool full = stream.ReadBoolean();
            int mask = stream.ReadVarInt();
            int a = stream.ReadVarInt();
            //  byte[] blockData = stream.ReadByteArray();



            for (int chunkY = 0; chunkY < 16; chunkY++)
            {
                if ((mask & (1 << chunkY)) == 0)
                    continue;

                byte bitsPerBlock = stream.ReadUnsignedByte();
                bool usePalette = bitsPerBlock <= 8;

                if (bitsPerBlock < 4)
                    bitsPerBlock = 4;

                int paletteLength = stream.ReadVarInt();

                int[] palette = new int[paletteLength];

                for (int i = 0; i < paletteLength; i++)
                {
                    palette[i] = stream.ReadVarInt();

                }
                uint valueMask = (uint)((1L << bitsPerBlock) - 1);

                ulong[] dataArray = stream.ReadULongArray();

                Chunk chunk = new Chunk(16);

                if (dataArray.Length > 0)
                {
                    int longIndex = 0;
                    int startOffset = -bitsPerBlock;

                    for (int y = 0; y < 16; y++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            for (int x = 0; x < 16; x++)
                            {
                                uint blockId;

                                startOffset += bitsPerBlock;
                                bool overlap = false;

                                if ((startOffset + bitsPerBlock) > 64)
                                {

                                    if (startOffset >= 64)
                                    {
                                        startOffset -= 64;
                                        longIndex++;
                                    }
                                    else overlap = true;

                                }

                                if (overlap)
                                {
                                    int endOffset = 64 - startOffset;
                                    blockId = (ushort)((dataArray[longIndex] >> startOffset | dataArray[longIndex + 1] << endOffset) & valueMask);
                                }
                                else
                                {
                                    blockId = (ushort)((dataArray[longIndex] >> startOffset) & valueMask);
                                }

                                if (usePalette)
                                {
                                    if (paletteLength <= blockId)
                                    {
                                        int blockNumber = (y * 16 + z) * 16 + x;
                                        throw new IndexOutOfRangeException(String.Format("Block ID {0} is outside Palette range 0-{1}! (bitsPerBlock: {2}, blockNumber: {3})",
                                            blockId,
                                            paletteLength - 1,
                                            bitsPerBlock,
                                            blockNumber));
                                    }

                                    blockId = (ushort)palette[blockId];
                                }

                                chunk[x, y, z] = new BlockState(blockId);

                            }
                        }
                    }
                }

                Column[chunkY] = chunk;

                stream.ReadByteArray(16 * 16 * 16 / 2);
                if (CurrentDimension == 0)
                    stream.ReadByteArray(16 * 16 * 16 / 2);

            }
        }



        public override void Write(IMinecraftPrimitiveWriter stream)
        {

        }



        public ServerChunkDataPacket() { }
    }

}
