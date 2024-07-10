using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;

public enum Side
{
    Client,
    Server
}

public interface IFieldGenerator
{
}

public class TypesComparer : IEqualityComparer<KeyValuePair<string, ProtodefType>>
{
    public bool Equals(KeyValuePair<string, ProtodefType> x, KeyValuePair<string, ProtodefType> y)
    {
        if (x.Key == y.Key)
        {
            if (x.Value.Equals(y.Value))
            {
                return true;
            }
        }

        return false;
    }

    public int GetHashCode(KeyValuePair<string, ProtodefType> obj)
    {
        return HashCode.Combine(obj.Key, obj.Value);
    }
}

public sealed class ProtocolSourceGenerator
{
    private Side _side;
    private List<Protocol> _protocols;

    public ProtocolSourceGenerator(List<Protocol> protocols, Side side)
    {
        _protocols = protocols;
        _side = side;
    }

    private string GetVersion()
    {
        if (_protocols.Count == 1)
            return _protocols.First().Version.Version.ToString();
        else
        {
            return $"{_protocols.First().Version.Version}_{_protocols.Last().Version.Version}";
        }
    }

    public NetNamespace Generate()
    {
        var netNamespace = new NetNamespace();
        netNamespace.Name = "McProtoNet.Protocol" + _side + "_" + GetVersion();

        netNamespace.Usings.Add("McProtoNet.Serialization");
        netNamespace.Usings.Add("McProtoNet.Protocol");
        netNamespace.Usings.Add("McProtoNet.Abstractions");
        netNamespace.Usings.Add("McProtoNet.NBT");
        netNamespace.Usings.Add("System.Reactive.Subjects");

        var comparer = new TypesComparer();

        var typesIntersect = IntersectAll(_protocols.Select(x => x.JsonPackets.Types), comparer).ToDictionary();


        return netNamespace;
    }

    private List<T> IntersectAll<T>(IEnumerable<IEnumerable<T>> lists, IEqualityComparer<T> comparer)
    {
        HashSet<T> hashSet = new HashSet<T>(lists.First(), comparer);
        foreach (var list in lists.Skip(1))
        {
            hashSet.IntersectWith(list);
        }

        return hashSet.ToList();
    }
}