using HolyClient.Common;
using MessagePack;
using QuickProxyNet;

namespace HolyClient.StressTest;

[Union(0, typeof(InMemoryProxySource))]
[Union(1, typeof(FileProxySource))]
[Union(2, typeof(UrlProxySource))]
public interface IProxySource
{
    Guid Id { get; }
    string Name { get; }

    ProxyType Type { get; set; }


    Task<IEnumerable<ProxyInfo>> GetProxiesAsync();
}