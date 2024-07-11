using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefPrefixedString : ProtodefType
{
    [JsonConstructor]
    public ProtodefPrefixedString(ProtodefType countType)
    {
        ArgumentNullException.ThrowIfNull(countType);
        CountType = countType;
    }

    [JsonPropertyName("countType")] public ProtodefType CountType { get; }

    public override string? GetNetType()
    {
        return "string";
    }

    public override object Clone()
    {
        return new ProtodefPrefixedString((ProtodefType)CountType.Clone());
    }

    private bool Equals(ProtodefPrefixedString other)
    {
        return CountType.Equals(other.CountType);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is ProtodefPrefixedString other && Equals(other));
    }

    public override int GetHashCode()
    {
        return CountType.GetHashCode();
    }
}