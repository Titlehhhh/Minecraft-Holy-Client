using System.Collections;
using System.Net.Sockets;

namespace QuickProxyNet.ProxyChecker;

public interface IProxyChecker : IDisposable
{
    Task Start();

    ValueTask<IProxyClient> GetNextProxy(CancellationToken cancellationToken);
}