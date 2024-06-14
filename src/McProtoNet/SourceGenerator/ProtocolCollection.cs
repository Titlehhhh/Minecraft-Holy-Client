public sealed class ProtocolCollection
{
    public ProtocolCollection()
    {
        Protocols = new Dictionary<int, Protocol>();
    }

    public Dictionary<int, Protocol> Protocols { get; }

    public void Add(int version, Protocol protocol)
    {
        if (!Protocols.ContainsKey(version)) Protocols.Add(version, protocol);
    }
}