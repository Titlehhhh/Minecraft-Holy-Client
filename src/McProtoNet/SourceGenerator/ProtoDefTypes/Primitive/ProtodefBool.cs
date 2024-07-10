namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBool : ProtodefType
{
    public override string ToString()
    {
        return "bool";
    }

    public override string? GetNetType()
    {
        return "bool";
    }

    

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefBool other;
    }

    private static readonly int boolHash = "bool".GetHashCode();
    public override int GetHashCode()
    {
        return boolHash;
    }
}