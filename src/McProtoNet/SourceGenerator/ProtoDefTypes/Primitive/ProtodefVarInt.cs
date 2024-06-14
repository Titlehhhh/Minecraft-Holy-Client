namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefVarInt : ProtodefType
{
    public override string ToString()
    {
        return "varint";
    }

    public override string? GetNetType()
    {
        return "int";
    }
}