namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefOption : ProtodefType, IPathTypeEnumerable
{
    public ProtodefOption(ProtodefType type)
    {
        Type = type;
    }

    public ProtodefType Type { get; }

    public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
    {
        if (Type is IPathTypeEnumerable)
            yield return new KeyValuePair<string, ProtodefType>("type", Type);
    }

    public override string? GetNetType()
    {
        var netType = Type.GetNetType();
        if (netType is not null) return netType + "?";
        return null;
    }
}