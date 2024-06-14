using System;
using System.Linq;
using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.Models.ManagingExtensions;
using HolyClient.StressTest;
using Splat;

namespace HolyClient.Services;

public class PluginProvider : IPluginProvider
{
    private readonly SourceCache<IPluginSource, PluginTypeReference> _stressTestPlugins = new(x => x.Reference);

    public PluginProvider()
    {
        var extensionManager = Locator.Current.GetService<ExtensionManager>();

        foreach (var assembly in extensionManager.AssemblyManager.Assemblies.Items) CreatePluginSources(assembly);

        extensionManager.AssemblyManager.Assemblies
            .Connect()
            .OnItemAdded(CreatePluginSources)
            .OnItemRemoved(OnRemovedAssembly)
            .Subscribe();

        _stressTestPlugins.AddOrUpdate(new DefaultPluginSource());

        _stressTestPlugins.AddOrUpdate(new CustomPluginSource(() => { return new TidePVPBehaviorAttack(); },
            new PluginMetadata("Titlehhhh", "TidePVP Loader", "TidePVP"),
            new PluginTypeReference("HolyClient", "TidePVP")));
    }

    public IObservableCache<IPluginSource, PluginTypeReference> AvailableStressTestPlugins => _stressTestPlugins;

    private void OnRemovedAssembly(IAssemblyFile assembly)
    {
        var keys = assembly.StressTestPlugins
            .Select(x => new PluginTypeReference(assembly.Name, x.FullName));


        _stressTestPlugins.RemoveKeys(keys);
    }

    private void CreatePluginSources(IAssemblyFile assembly)
    {
        foreach (var stressTestPlugin in assembly.StressTestPlugins)
        {
            PluginTypeReference reference = new(assembly.Name, stressTestPlugin.FullName);

            var source = new PluginSource(stressTestPlugin, reference);

            _stressTestPlugins.AddOrUpdate(source);
        }
    }
}

public class DefaultPluginSource : IPluginSource
{
    public PluginMetadata Metadata { get; } = new("Titlehhhh", "Spam hello bots", "HolyClient default behavior");

    public PluginTypeReference Reference { get; } = new();

    public T CreateInstance<T>() where T : IStressTestBehavior
    {
        IStressTestBehavior beh = new DefaultBehavior();
        return (T)beh;
    }
}

public sealed class CustomPluginSource : IPluginSource
{
    private readonly Func<IStressTestBehavior> _factory;

    public CustomPluginSource(Func<IStressTestBehavior> factory, PluginMetadata metadata, PluginTypeReference reference)
    {
        _factory = factory;
        Metadata = metadata;
        Reference = reference;
    }

    public PluginMetadata Metadata { get; }

    public PluginTypeReference Reference { get; }

    public T CreateInstance<T>() where T : IStressTestBehavior
    {
        return (T)_factory();
    }
}