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
    public static NamespacesIntersection Intersection(ProtodefNamespace ns)
    {
        return new NamespacesIntersection(ns);
    }

    public static NamespacesIntersection Intersection(params ProtodefNamespace[] namespaces)
    {
        var first = Intersection(namespaces[0]);

        for (var i = 1; i < namespaces.Length; i++)
        {
            var second = namespaces[i];
            first = Intersection(first, second);
        }

        return first;
    }


    private static NamespacesIntersection Intersection(NamespacesIntersection ns1, ProtodefNamespace ns)
    {
        throw null;
    }
}

public sealed class NamespacesIntersection
{
    /// <summary>
    ///     One namespace
    /// </summary>
    /// <param name="ns"></param>
    internal NamespacesIntersection(ProtodefNamespace ns)
    {
        foreach (var item in ns)
        {
        }
    }
}


public sealed class PacketIntersection
{
}