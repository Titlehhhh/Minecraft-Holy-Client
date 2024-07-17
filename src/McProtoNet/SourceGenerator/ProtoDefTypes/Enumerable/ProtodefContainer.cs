using System.Collections;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefContainer : ProtodefType, IPathTypeEnumerable,
    IJsonOnDeserialized
{
    public Dictionary<string, ProtodefContainerField> Fields { get; set; } = new();

    [JsonConstructor]
    public ProtodefContainer(List<ProtodefContainerField> fields)
    {
        this.Fields = fields.ToDictionary(x => x.Name, x => x);
    }

    private ProtodefContainer(ProtodefContainer other)
    {
        Fields = other.Fields.Select(x =>
                new KeyValuePair<string, ProtodefContainerField>(x.Key, (ProtodefContainerField)x.Value.Clone()))
            .ToDictionary();
        foreach (var item in Fields) item.Value.Parent = this;
    }


    public override void OnDeserialized()
    {
        foreach (var field in Fields)
        {
            field.Value.Parent = this;
            field.Value.Type.Parent = field.Value;
        }
    }

    IEnumerator<KeyValuePair<string, ProtodefType>> IPathTypeEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
        // var id = 0;
        //
        //
        // foreach (var item in fields)
        // {
        //     id++;
        //
        //     var name = string.IsNullOrEmpty(item.Name) ? $"anon_{id}" : item.Name;
        //
        //     yield return new KeyValuePair<string, ProtodefType>(name, item.Type);
        // }
    }

    public override object Clone()
    {
        return new ProtodefContainer(this);
    }
}