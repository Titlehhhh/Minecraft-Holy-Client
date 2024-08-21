using System;
using System.Reflection;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.SDK.Attributes;

namespace HolyClient.Services;

public class PluginSource : IPluginSource
{
    private readonly Type type;

    public PluginSource(Type type, PluginTypeReference typeReference)
    {
        this.type = type;
        Reference = typeReference;

        var author = type.GetCustomAttribute<PluginAuthorAttribute>()?.Author;
        var title = type.GetCustomAttribute<PluginTitleAttribute>()?.Title;
        var description = type.GetCustomAttribute<PluginDescriptionAttribute>()?.Description;

        Metadata = new PluginMetadata(author, description, title);
    }

    public PluginTypeReference Reference { get; }

    public PluginMetadata Metadata { get; }

    public T CreateInstance<T>() where T : IStressTestBehavior
    {
        var plugin = Activator.CreateInstance(type);
        return (T)plugin;
    }
}