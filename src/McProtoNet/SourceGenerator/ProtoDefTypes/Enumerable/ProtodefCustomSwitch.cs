using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefCustomSwitch : ProtodefSwitch
{
    [JsonIgnore] public string? Owner { get; set; }

    public override object Clone()
    {
        var owner = new ProtodefCustomSwitch
        {
            CompareToValue = CompareToValue,
            Fields = Fields.Select(x => new KeyValuePair<string, ProtodefType>(x.Key, (ProtodefType)x.Value.Clone()))
                .ToDictionary(),
            Default = Default!.Clone() as ProtodefType,
            Owner = Owner
        };
        foreach (var keyValuePair in owner.Fields) keyValuePair.Value.Parent = owner;

        return owner;
    }
}