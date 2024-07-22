using SourceGenerator.ProtoDefTypes;

public sealed class Protocol : ICloneable
{
    public VersionInfo Version { get; set; }
    public ProtodefProtocol JsonPackets { get; set; }


    public object Clone()
    {
        return new Protocol()
        {
            Version = new VersionInfo()
            {
                MajorVersion = this.Version.MajorVersion,
                MinecraftVersion = this.Version.MinecraftVersion,
                Version = this.Version.Version
            },
            JsonPackets = (ProtodefProtocol)this.JsonPackets.Clone()
        };
    }
}

