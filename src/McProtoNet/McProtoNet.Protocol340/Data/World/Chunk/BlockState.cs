namespace McProtoNet.Protocol340.Data.World.Chunk
{
    public record struct BlockState
    {

        public readonly ushort Id;
        public readonly byte Data;

        public BlockState(uint id)
        {
            Id = (ushort)(id >> 4);
            Data = (byte)(id & 0xF);
        }

        public BlockState(ushort id, byte data)
        {
            Id = id;
            Data = data;
        }
    }
}
