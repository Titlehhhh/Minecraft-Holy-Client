using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefMapper : ProtodefType
{
    [JsonConstructor]
    public ProtodefMapper(string type, Dictionary<string, string> mappings)
    {
        Type = type;
        Mappings = mappings;
    }

    [JsonPropertyName("type")] public string Type { get; private set; }

    [JsonPropertyName("mappings")] public Dictionary<string, string> Mappings { get; private set; } = new();
}