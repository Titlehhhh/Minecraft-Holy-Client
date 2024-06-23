using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using McProtoNet;
using McProtoNet.Client;
using McProtoNet.Protocol;
using McProtoNet.Protocol754;
using Serilog;





string host = args[0];



StressTestProfile stressTestProfile = new StressTestProfile();

stressTestProfile.Version = 754;
stressTestProfile.BotsNickname = "_Title";
stressTestProfile.UseProxy = true;

stressTestProfile.Server = host;

stressTestProfile.ProxyCheckerOptions = new ProxyCheckerOptions()
{
    ParallelCount = 30_000
};
stressTestProfile.SetBehavior(new DefaultPluginSource());
stressTestProfile.NumberOfBots = 300;

var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

await stressTestProfile.Start(logger);

await Task.Delay(-1);

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