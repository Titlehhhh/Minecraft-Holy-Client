using System.Collections;
using System.Text.Json.Serialization;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefContainer : ProtodefType, IPathTypeEnumerable,
    IJsonOnDeserialized
{
    public List<ProtodefContainerField> Fields { get; set; } = new();

    [JsonConstructor]
    public ProtodefContainer(List<ProtodefContainerField> fields)
    {
        Fields = fields;
    }

    private ProtodefContainer(ProtodefContainer other)
    {
        foreach (var field in other.Fields)
        {
            var fieldClone = (ProtodefContainerField)field.Clone();
            fieldClone.Parent = this;
            Fields.Add(fieldClone);
        }
    }


    public override void OnDeserialized()
    {
        foreach (var field in Fields)
        {
            field.Parent = this;
            field.Type.Parent = field;
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

    public void SetPassable(string name)
    {
        
    }
    public ProtodefType this[string name]
    {
        get
        {
            foreach (var item in Fields)
            {
                if (item.Name == name)
                    return item.Type;
            }

            throw new KeyNotFoundException();
        }
    }

    public override object Clone()
    {
        return new ProtodefContainer(this);
    }
}