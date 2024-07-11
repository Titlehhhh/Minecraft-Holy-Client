public static class Mappings
{
    private static readonly Dictionary<string, string> CustomToNetType = new()
    {
        { "UUID", "Guid" },
        { "position", "Position" },
        { "slot", "Slot?" },
        { "restBuffer", "byte[]" },
        { "nbt", "NbtTag" },
        { "optionalNbt", "NbtTag?" },
        { "anonymousNbt", "NbtTag" },
        { "anonOptionalNbt", "NbtTag?" }
    };

    private static readonly Dictionary<string, string> readDict = new()
    {
        { "varint", "ReadVarInt" },
        { "varlong", "ReadVarLong" },
        { "string", "ReadString" },
        { "bool", "ReadBoolean" },
        { "u8", "ReadUnsignedByte" },
        { "i8", "ReadSignedByte" },
        { "u16", "ReadUnsignedShort" },
        { "i16", "ReadSignedShort" },
        { "u32", "ReadUnsignedInt" },
        { "i32", "ReadSignedInt" },
        { "u64", "ReadUnsignedLong" },
        { "i64", "ReadSignedLong" },
        { "f32", "ReadFloat" },
        { "f64", "ReadDouble" },
        { "UUID", "ReadUUID" },
        { "restBuffer", "ReadRestBuffer" },
        { "position", "ReadPosition" },
        { "slot", "ReadSlot" },
        { "nbt", "ReadNbt" },
        { "optionalNbt", "ReadOptionalNbt" },
        { "anonymousNbt", "ReadNbt" },
        { "anonOptionalNbt", "ReadOptionalNbt" }
    };

    private static readonly Dictionary<string, string> writeDict = new()
    {
        { "varint", "WriteVarInt" },
        { "varlong", "WriteVarLong" },
        { "string", "WriteString" },
        { "bool", "WriteBoolean" },
        { "u8", "WriteUnsignedByte" },
        { "i8", "WriteSignedByte" },
        { "u16", "WriteUnsignedShort" },
        { "i16", "WriteSignedShort" },
        { "u32", "WriteUnsignedInt" },
        { "i32", "WriteSignedInt" },
        { "u64", "WriteUnsignedLong" },
        { "i64", "WriteSignedLong" },
        { "f32", "WriteFloat" },
        { "f64", "WriteDouble" },
        { "UUID", "WriteUUID" },
        { "restBuffer", "WriteBuffer" },
        { "position", "WritePosition" },
        { "slot", "WriteSlot" },
        { "nbt", "WriteNbt" },
        { "optionalNbt", "WriteOptionalNbt" },
        { "anonymousNbt", "WriteNbt" },
        { "anonOptionalNbt", "WriteOptionalNbt" }
    };
}