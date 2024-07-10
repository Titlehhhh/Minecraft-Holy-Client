namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefVarLong : ProtodefType
{
    public override string ToString()
    {
        return "varlong";
    }

    public override string? GetNetType()
    {
        return "long";
    }
    private static readonly int hash = "varlong".GetHashCode();


    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefString;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}