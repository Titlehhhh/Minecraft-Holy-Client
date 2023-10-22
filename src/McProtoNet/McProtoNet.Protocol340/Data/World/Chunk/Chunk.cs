namespace McProtoNet.Protocol340.Data.World.Chunk
{
    public class Chunk
    {
        private readonly BlockState[] blocks;

        public Chunk(BlockState[] blocks)
        {
            this.blocks = blocks;
        }
        public Chunk(int size)
        {
            this.blocks = new BlockState[size * size * size];
        }

        public BlockState this[int x, int y, int z]
        {
            get
            {
                return blocks[(y << 8) | (z << 4) | x];
            }
            set
            {
                blocks[(y << 8) | (z << 4) | x] = value;
            }
        }
    }
}
