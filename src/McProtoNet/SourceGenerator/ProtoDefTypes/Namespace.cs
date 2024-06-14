using System.Collections;

namespace SourceGenerator.ProtoDefTypes;

public sealed class Namespace : ProtodefType, IEnumerable<KeyValuePair<string, ProtodefType>>
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
}