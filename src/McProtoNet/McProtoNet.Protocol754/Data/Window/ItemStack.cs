using McProtoNet.NBT;

namespace McProtoNet.Protocol754.Data
{

    public class ItemStack
    {
        public int Id { get; private set; }
        public sbyte Amount { get; private set; }
        public NbtCompound? Nbt { get; private set; }

        public ItemStack(int id, sbyte amount, NbtCompound? nbt)
        {
            Id = id;
            Amount = amount;
            Nbt = nbt;
        }
    }
}
