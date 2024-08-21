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

public sealed class PassableType : ProtodefType
{
    public PassableType(ProtodefType type)
    {
        if (type is PassableType pass)
            Type = pass.Type;
        else
            Type = type;
        
        //Type.Parent = this;
    }
    
    public ProtodefType Type { get; set; }
    public override object Clone()
    {
        var g = new PassableType((ProtodefType)this.Type.Clone())
        {

        };

        return g;
    }
}