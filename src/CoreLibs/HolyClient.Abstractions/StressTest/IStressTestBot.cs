using McProtoNet.Abstractions;
using McProtoNet.Client;

namespace HolyClient.Abstractions.StressTest;

public interface IStressTestBot : IDisposable
{
    public MinecraftClient Client { get; }
    public ProtocolBase Protocol { get; }
    public Task Restart(bool changeNickAndProxy);
    void ConfigureAutoRestart(Action<IStressTestBot> action);
    public void Stop();
}