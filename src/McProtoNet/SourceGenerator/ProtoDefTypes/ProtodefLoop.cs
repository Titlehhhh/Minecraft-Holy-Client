using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefLoop : ProtodefType, IPathTypeEnumerable
{
    [JsonPropertyName("endVal")] public uint EndValue { get; set; }

    [JsonPropertyName("type")] public ProtodefType Type { get; set; }

    public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
    {
        if (Type is IPathTypeEnumerable)
            yield return new KeyValuePair<string, ProtodefType>("type", Type);
    }
}