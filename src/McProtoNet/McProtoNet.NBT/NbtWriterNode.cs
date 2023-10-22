namespace McProtoNet.NBT
{
    /// <summary>
    /// Represents state of a node in the NBT file tree, used by NbtWriter
    /// </summary>
    internal sealed class NbtWriterNode
    {
        public NbtTagType ParentType;
        public NbtTagType ListType;
        public int ListSize;
        public int ListIndex;
    }
}