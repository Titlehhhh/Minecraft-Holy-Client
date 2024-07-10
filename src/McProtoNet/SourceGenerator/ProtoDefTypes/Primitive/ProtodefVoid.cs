namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefVoid : ProtodefType
{
    public override string ToString()
    {
        return "void";
    }

    public override string? GetNetType()
    {
        return "void";
    }

    private static readonly int hash = "void".GetHashCode();


    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefString;
    }

    public override int GetHashCode()
    {
        return hash;
    }
}