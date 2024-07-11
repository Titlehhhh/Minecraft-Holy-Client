namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefOption : ProtodefType, IPathTypeEnumerable
{
    public ProtodefOption(ProtodefType type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type = type;
    }

    public ProtodefType Type { get; }

    public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
    {
        if (Type is IPathTypeEnumerable)
            yield return new KeyValuePair<string, ProtodefType>("type", Type);
    }

    public override object Clone()
    {
        return new ProtodefOption((ProtodefType)Type.Clone());
    }

    public override string? GetNetType()
    {
        var netType = Type.GetNetType();
        if (netType is not null) return netType + "?";
        return null;
    }

    private bool Equals(ProtodefOption other)
    {
        return Type.Equals(other.Type);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ProtodefOption other && Equals(other));
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode();
    }
}