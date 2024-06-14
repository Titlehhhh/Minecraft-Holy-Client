using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBitFieldNode
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("signed")] public bool Signed { get; set; }

    public override string ToString()
    {
        return $"name: {Name} size: {Size} signed: {Signed}";
    }
}