namespace SourceGenerator.ProtoDefTypes;

public sealed class ProtodefCustomType : ProtodefType
{
    public ProtodefCustomType(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }

    public override object Clone()
    {
        return new ProtodefCustomType(Name);
    }
}