namespace McProtoNet.Utils;

public class SrvRecord
{
    public SrvRecord(string host, ushort port)
    {
        Host = host;
        Port = port;
    }

    public string Host { get; }

    public ushort Port { get; }
}