namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefVarInt : ProtodefType
{
    private static readonly int hash = "varint".GetHashCode();

    public override string ToString()
    {
        return "varint";
    }

    public override string? GetNetType()
    {
        return "int";
    }

    public override object Clone()
    {
        return new ProtodefVarInt();
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefString;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}