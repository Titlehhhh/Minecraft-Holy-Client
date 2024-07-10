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
    
    private static readonly int hash = "varint".GetHashCode();


    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefString;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}