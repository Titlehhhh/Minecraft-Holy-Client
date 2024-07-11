using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefContainerField : ProtodefType
{
    [JsonConstructor]
    public ProtodefContainerField(bool? anon, string? name, ProtodefType type)
    {
        Anon = anon;
        Name = name;
        ArgumentNullException.ThrowIfNull(type);
        Type = type;
    }

    //public bool IsAnon => Anon;

    [JsonPropertyName("anon")] public bool? Anon { get; }

    [JsonPropertyName("name")] public string? Name { get; }

    [JsonPropertyName("type")] public ProtodefType Type { get; }

    public override void OnDeserialized()
    {
        Type.Parent = this;
    }

    public override object Clone()
    {
        return new ProtodefContainerField(Anon, Name, (ProtodefType)Type.Clone());
    }
}