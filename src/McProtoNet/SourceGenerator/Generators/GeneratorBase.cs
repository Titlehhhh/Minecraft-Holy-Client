using System.ComponentModel;
using System.Reflection;
using SourceGenerator.NetTypes;
using SourceGenerator.ProtoDefTypes;

namespace SourceGenerator.Generators;

public abstract class GeneratorBase
{
    public NetClass ServerPacketClass { get; set; }
    public abstract void Generate(ProtodefContainerField field, bool isSkip);
}