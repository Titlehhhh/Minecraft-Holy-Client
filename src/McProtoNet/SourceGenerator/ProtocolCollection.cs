using SourceGenerator.ProtoDefTypes;

public sealed class ProtocolCollection
{
    public ProtocolCollection()
    {
        Protocols = new Dictionary<int, Protocol>();
    }

    private ProtocolCollection(ProtocolCollection other)
    {
        Protocols = other.Protocols.Select(x => new KeyValuePair<int, Protocol>(x.Key, (Protocol)x.Value.Clone()))
            .ToDictionary();
    }

    public Dictionary<int, Protocol> Protocols { get; }

    public void Add(int version, Protocol protocol)
    {
        if (!Protocols.ContainsKey(version)) Protocols.Add(version, protocol);
    }

    public ProtocolCollection Clone()
    {
        return new ProtocolCollection(this);
    }
}