using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBitFieldNode : ICloneable
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("size")] public int Size { get; set; }

    [JsonPropertyName("signed")] public bool Signed { get; set; }

    public object Clone()
    {
        return new ProtodefBitFieldNode
        {
            Name = Name,
            Size = Size,
            Signed = Signed
        };
    }

    public override string ToString()
    {
        return $"name: {Name} size: {Size} signed: {Signed}";
    }
}