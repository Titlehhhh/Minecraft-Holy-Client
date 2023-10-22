namespace McProtoNet.NBT
{
    internal enum NbtParseState
    {
        AtStreamBeginning,
        AtCompoundBeginning,
        InCompound,
        AtCompoundEnd,
        AtListBeginning,
        InList,
        AtStreamEnd,
        Error
    }
}