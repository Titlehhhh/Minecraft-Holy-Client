using SourceGenerator.ProtoDefTypes;

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

    public void CompareAll()
    {
    }
}

public static class Helper
{
    public static NamespacesIntersection Intersection(Namespace ns)
    {
        return new NamespacesIntersection(ns);
    }

    public static NamespacesIntersection Intersection(params Namespace[] namespaces)
    {
        NamespacesIntersection first = Intersection(namespaces[0]);

        for (int i = 1; i < namespaces.Length; i++)
        {
            Namespace second = namespaces[i];
            first = Intersection(first, second);
        }

        return first;
    }


    private static NamespacesIntersection Intersection(NamespacesIntersection ns1, Namespace ns)
    {
        throw null;
    }
}

public sealed class NamespacesIntersection
{
    
    
    /// <summary>
    /// One namespace
    /// </summary>
    /// <param name="ns"></param>
    internal NamespacesIntersection(Namespace ns)
    {
        foreach (var item in ns)
        {
        }
    }
}


public sealed class PacketIntersection
{
    
}