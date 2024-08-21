namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefVarLong : ProtodefType
{
    private static readonly int hash = "varlong".GetHashCode();

    public override string ToString()
    {
        return "varlong";
    }

    public override string? GetNetType()
    {
        return "long";
    }

    public override object Clone()
    {
        return new ProtodefVarLong();
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