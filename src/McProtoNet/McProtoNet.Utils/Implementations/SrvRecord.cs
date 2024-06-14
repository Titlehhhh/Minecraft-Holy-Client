namespace McProtoNet.Utils;

public class SrvRecord
{
    public SrvRecord(string target, ushort port)
    {
        Target = target;
        Port = port;
    }

    public string Target { get; }

    public ushort Port { get; }
}