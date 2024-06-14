namespace SourceGenerator.ProtoDefTypes.Attributes;

public sealed class NetTypeAttribute : Attribute
{
    public NetTypeAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}