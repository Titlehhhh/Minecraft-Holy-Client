using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;

public sealed class ProtocolSourceGenerator
{
    private readonly List<Protocol> _protocols;
    private readonly Side _side;

    public ProtocolSourceGenerator(List<Protocol> protocols, Side side)
    {
        _protocols = protocols;
        MinVersion = _protocols.First().Version.Version.ToString();
        MaxVersion = _protocols.Last().Version.Version.ToString();
        _side = side;
        MainNamespace = new NetNamespace();
        MainNamespace.Name = "McProtoNet.Protocol" + _side + "_" + GetVersionForName();

        MainNamespace.Usings.Add("McProtoNet.Serialization");
        MainNamespace.Usings.Add("McProtoNet.Protocol");
        MainNamespace.Usings.Add("McProtoNet.Abstractions");
        MainNamespace.Usings.Add("McProtoNet.NBT");
        MainNamespace.Usings.Add("System.Reactive.Subjects");
    }

    private string MinVersion { get; }

    private string MaxVersion { get; }

    public NetNamespace MainNamespace { get; }

    private string GetVersionForName()
    {
        if (_protocols.Count == 1)
            return MinVersion;
        return MinVersion + "_" + MaxVersion;
    }


    public void Generate()
    {
    }
}

public class SerializeGenerator
{
    private const string readerName = "reader";
    private const string writername = "writer";


    public string SerializeInstructions { get; }
    public string DeserializeInstructions { get; }


    public void GenerateSerialize(NetClass packet, (ProtodefContainer packetContainer, string version)[] packets)
    {
    }

    public void GenerateDeserialize(NetClass packet, (ProtodefContainer packetContainer, string version)[] packets)
    {
    }
}