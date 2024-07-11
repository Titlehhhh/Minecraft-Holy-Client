using SourceGenerator.ProtoDefTypes;

public sealed class Protocol : ICloneable
{
    public VersionInfo Version { get; set; }
    public ProtodefProtocol JsonPackets { get; set; }


    public object Clone()
    {
        return (Protocol)MemberwiseClone();
    }
}