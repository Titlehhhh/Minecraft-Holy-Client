using QuickProxyNet;

namespace HolyClient.StressTest;

public interface IProxyProvider : IDisposable
{
    public ValueTask<IProxyClient> GetNextProxy();
}