using System.Collections;

namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefBitField : ProtodefType, IEnumerable<ProtodefBitFieldNode>, IEnumerable
{
    //public override string TypeName => "Bitfield";
    private readonly List<ProtodefBitFieldNode> nodes = new();

    public ProtodefBitField(List<ProtodefBitFieldNode> nodes)
    {
        this.nodes = nodes;
    }

    private ProtodefBitField(ProtodefBitField other)
    {
        nodes = other.nodes
            .Select(x => x.Clone())
            .Cast<ProtodefBitFieldNode>()
            .ToList();
    }

    public IEnumerator<ProtodefBitFieldNode> GetEnumerator()
    {
        return nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return nodes.GetEnumerator();
    }

    public override object Clone()
    {
        return new ProtodefBitField(this);
    }
}