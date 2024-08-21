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


    [JsonPropertyName("anon")] public bool? Anon { get; }

    [JsonPropertyName("name")] public string? Name { get; }

    [JsonPropertyName("type")] public ProtodefType Type { get; }

    [JsonIgnore] public bool IsPass { get; set; }
    [JsonIgnore] public bool IsAnon => Anon == true;
    
    

    public override void OnDeserialized()
    {
        Type.Parent = this;
    }

    public override object Clone()
    {
        var owner = new ProtodefContainerField(Anon, Name, (ProtodefType)Type.Clone());
        owner.Type.Parent = owner;
        return owner;
    }
}