using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefLoop : ProtodefType, IPathTypeEnumerable
{
    private ProtodefType _type;
    [JsonPropertyName("endVal")] public uint EndValue { get; set; }

    [JsonPropertyName("type")]
    public ProtodefType Type
    {
        get => _type;
        set => _type = value ?? throw new ArgumentNullException("value");
    }

    public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
    {
        if (Type is IPathTypeEnumerable)
            yield return new KeyValuePair<string, ProtodefType>("type", Type);
    }

    public override object Clone()
    {
        return new ProtodefLoop
        {
            EndValue = EndValue,
            Type = (ProtodefType)Type.Clone()
        };
    }
}