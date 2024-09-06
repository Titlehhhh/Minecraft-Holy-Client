using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;

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