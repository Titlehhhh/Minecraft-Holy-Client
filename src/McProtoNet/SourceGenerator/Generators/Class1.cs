using System.ComponentModel;
using System.Reflection;
using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;

namespace SourceGenerator.Generators;


public class GeneratorBase
{
    public NetClass ServerPacket { get; set; }
    public NetMethod SendMethod { get; set; }
    public ProtodefContainer Container { get; set; }

    public void Generate(ProtodefContainerField field)
    {
        
    }
}

public class CustomGenerator : GeneratorBase
{
    
}

public class VarIntGenerator : GeneratorBase
{
    
}

public class VarLongGenerator : GeneratorBase
{
    
}