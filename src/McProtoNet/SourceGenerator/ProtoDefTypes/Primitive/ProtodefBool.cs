namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBool : ProtodefType
{
    private static readonly int boolHash = "bool".GetHashCode();

    public override string ToString()
    {
        return "bool";
    }

    public override string? GetNetType()
    {
        return "bool";
    }

    public override object Clone()
    {
        return new ProtodefBool();
    }


    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefBool other;
    }

    public override int GetHashCode()
    {
        return boolHash;
    }
}