namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefNumericType : ProtodefType
{
    public ProtodefNumericType(string name, string originalName, bool signed, ByteOrder order)
    {
        NetName = name;
        OriginalName = originalName;
        Signed = signed;
        Order = order;
    }

    public string NetName { get; }

    public string OriginalName { get; }

    public bool Signed { get; }

    public ByteOrder Order { get; }

    public override string ToString()
    {
        return NetName;
    }

    public override string? GetNetType()
    {
        return NetName;
    }

    private bool Equals(ProtodefNumericType other)
    {
        return NetName == other.NetName && OriginalName == other.OriginalName && Signed == other.Signed && Order == other.Order;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefNumericType other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NetName, OriginalName, Signed, (int)Order);
    }
}