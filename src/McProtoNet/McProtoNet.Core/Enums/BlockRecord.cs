namespace McProtoNet.Core
{
    public struct BlockRecord
    {
        public int ID { get; private set; }
        public byte Meta { get; private set; }

        public BlockRecord(int iD, byte meta)
        {
            ID = iD;
            Meta = meta;
        }
    }
}
