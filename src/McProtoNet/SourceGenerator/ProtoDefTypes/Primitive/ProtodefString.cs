namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefString : ProtodefType
{
    private static readonly int hash = "string".GetHashCode();

    public override string ToString()
    {
        return "string";
    }

    public override string? GetNetType()
    {
        return "string";
    }

    public override object Clone()
    {
        return new ProtodefString();
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