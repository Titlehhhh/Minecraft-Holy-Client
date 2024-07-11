using System.Collections;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefContainer : ProtodefType, IEnumerable<ProtodefContainerField>, IPathTypeEnumerable,
    IJsonOnDeserialized
{
    private readonly List<ProtodefContainerField> fields = new();

    [JsonConstructor]
    public ProtodefContainer(List<ProtodefContainerField> fields)
    {
        this.fields = fields;
    }

    private ProtodefContainer(ProtodefContainer other)
    {
        fields = other.fields.Select(x => x.Clone()).Cast<ProtodefContainerField>().ToList();
    }

    public IEnumerator<ProtodefContainerField> GetEnumerator()
    {
        return fields.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return fields.GetEnumerator();
    }

    public override void OnDeserialized()
    {
        foreach (var field in fields)
        {
            field.Parent = this;
            field.Type.Parent = field;
        }
    }

    IEnumerator<KeyValuePair<string, ProtodefType>> IPathTypeEnumerable.GetEnumerator()
    {
        var id = 0;


        foreach (var item in fields)
        {
            id++;

            var name = string.IsNullOrEmpty(item.Name) ? $"anon_{id}" : item.Name;

            yield return new KeyValuePair<string, ProtodefType>(name, item.Type);
        }
    }

    public override object Clone()
    {
        return new ProtodefContainer(this);
    }
}