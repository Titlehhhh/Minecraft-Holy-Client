using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefArray : ProtodefType, IPathTypeEnumerable
{
    [JsonPropertyName("type")] public ProtodefType Type { get; set; }

    [JsonPropertyName("countType")] public ProtodefType CountType { get; set; }


    [JsonPropertyName("count")] public object? Count { get; set; }

    public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
    {
        if (Type is IPathTypeEnumerable)
            yield return new KeyValuePair<string, ProtodefType>("type", Type);
    }

    public override string? GetNetType()
    {
        var netType = Type.GetNetType();
        if (netType is not null) return netType + "[]";
        return null;
    }

    private bool Equals(ProtodefArray other)
    {
        return Type.Equals(other.Type) && CountType.Equals(other.CountType) && Equals(Count, other.Count);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ProtodefArray other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, CountType, Count);
    }
}