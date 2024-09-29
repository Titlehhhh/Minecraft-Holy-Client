using QuickProxyNet;

namespace HolyClient.Proxy;

public interface IProxyProvider : IDisposable
{
    public ValueTask<IProxyClient> GetNextProxy();
}