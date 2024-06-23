using System.ComponentModel;
using System.Reflection;
using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;

namespace SourceGenerator.Generators;


public sealed class GenerationContext
{
    public string? FieldName { get; set; }
    public ProtodefType FieldType { get; set; }
    
    
}

public sealed class GenerationResult
{
    public List<string> Instructions { get; }
}

public abstract class GeneratorBase
{
    public abstract void Generate(ProtodefContainerField field);
}

