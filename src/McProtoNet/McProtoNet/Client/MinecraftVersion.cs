namespace McProtoNet.Client;

public struct MinecraftVersion
{
    public readonly static int Latest = 767;

    public int ProtocolVersion { get; }
    public string MajorVersion { get; }
    public string MinorVersion { get; }
}