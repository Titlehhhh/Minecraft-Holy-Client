using System.Text.Json.Serialization;
using SourceGenerator.ProtoDefTypes.Converters;

namespace SourceGenerator.ProtoDefTypes;

[JsonConverter(typeof(DataTypeConverter))]
public abstract class ProtodefType : IJsonOnDeserialized, ICloneable
{
    public ProtodefType? Parent { get; set; }


    public abstract object Clone();

    public virtual void OnDeserialized()
    {
        if (this is IPathTypeEnumerable enums)
            foreach (var item in enums)
                item.Value.Parent = this;
    }

    public virtual string? GetNetType()
    {
        return null;
    }
}