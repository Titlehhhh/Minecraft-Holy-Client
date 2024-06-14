using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBuffer : ProtodefType
{
    [JsonPropertyName("countType")] public ProtodefType? CountType { get; set; }

    [JsonPropertyName("count")] public object? Count { get; set; }

    [JsonPropertyName("rest")] public bool? Rest { get; set; }

    public override string? GetNetType()
    {
        return "byte[]";
    }
}