namespace McProtoNet.NBT;

/// <summary>
///     Represents state of a node in the NBT file tree, used by NbtReader
/// </summary>
internal sealed class NbtReaderNode
{
    public int ListIndex;
    public NbtTagType ListType;
    public string? ParentName;
    public int ParentTagLength;
    public NbtTagType ParentTagType;
}