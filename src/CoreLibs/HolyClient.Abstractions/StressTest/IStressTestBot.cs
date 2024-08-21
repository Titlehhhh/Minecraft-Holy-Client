using McProtoNet.Client;

namespace HolyClient.Abstractions.StressTest;

public interface IStressTestBot
{
    public MinecraftClient Client { get; }
    public Task Restart(bool changeNickAndProxy);
}