using McProtoNet.NBT;

namespace McProtoNet.Protocol
{
    public sealed class Slot
    {
        public int ItemId { get; }
        public short ItemCount { get; }
        public NbtCompound? Nbt { get; }
    }
}