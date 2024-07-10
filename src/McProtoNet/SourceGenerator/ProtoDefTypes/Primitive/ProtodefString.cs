namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefString : ProtodefType
{
    public override string ToString()
    {
        return "string";
    }

    public override string? GetNetType()
    {
        return "string";
    }

    private static readonly int hash = "string".GetHashCode();


    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefString;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}