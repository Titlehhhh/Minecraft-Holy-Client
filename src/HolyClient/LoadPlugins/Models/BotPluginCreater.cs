using System;
using HolyClient.AppState;

namespace HolyClient.LoadPlugins.Models;

public class BotPluginCreater : IBotPluginCreater
{
    private readonly Type _type;

    public BotPluginCreater(Type type, string assembly, string assemblyFile)
    {
        _type = type;
        Name = _type.FullName;
        Assembly = assembly;
        AssemblyFile = assemblyFile;
    }

    public string Name { get; }

    public string Assembly { get; }

    public string AssemblyFile { get; }


    public BehaviorKey Token => new(Name, Assembly);

    //public BotPlugin Create()
    //{
    //	//return (BotPlugin)Activator.CreateInstance(_type);
    //}
}