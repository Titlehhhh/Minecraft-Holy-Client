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

    private ProtodefMapper(ProtodefMapper other)
    {
        Type = other.Type;
        Mappings = new Dictionary<string, string>(other.Mappings);
    }

    [JsonPropertyName("type")] public string Type { get; }

    [JsonPropertyName("mappings")] public Dictionary<string, string> Mappings { get; } = new();

    public override object Clone()
    {
        return new ProtodefMapper(this);
    }
}