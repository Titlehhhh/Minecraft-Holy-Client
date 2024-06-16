using McProtoNet.NBT;

namespace McProtoNet.Protocol;

public sealed class Slot
{
    public Slot(int itemId, sbyte itemCount, NbtTag? nbt)
    {
        ItemId = itemId;
        ItemCount = itemCount;
        Nbt = nbt;
    }

    public Slot()
    {
    }

    public int ItemId { get; set; }
    public sbyte ItemCount { get; set; }
    public NbtTag? Nbt { get; set; }
}