using System.Collections;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefNamespace : ProtodefType, IEnumerable<KeyValuePair<string, ProtodefType>>
{
    public Dictionary<string, ProtodefType> Types { get; set; } = new();

    public IEnumerator<KeyValuePair<string, ProtodefType>> GetEnumerator()
    {
        return Types.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Types.GetEnumerator();
    }

    public override object Clone()
    {
        var owner = new ProtodefNamespace
        {
            Types = Types
                .Select(x => new KeyValuePair<string, ProtodefType>(x.Key, (ProtodefType)x.Value.Clone()))
                .ToDictionary()
        };

        foreach (var item in owner.Types) item.Value.Parent = owner;

        return owner;
    }
}