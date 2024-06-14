using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefPrefixedString : ProtodefType
{
    [JsonConstructor]
    public ProtodefPrefixedString(ProtodefType countType)
    {
        CountType = countType;
    }

    [JsonPropertyName("countType")] public ProtodefType CountType { get; }

    public override string? GetNetType()
    {
        return "string";
    }
}