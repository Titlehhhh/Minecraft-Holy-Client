using HolyClient.Common;
using MessagePack;
using QuickProxyNet;

namespace HolyClient.StressTest;

[MessagePackObject(true)]
public class InMemoryProxySource : IProxySource
{
    private int _lines;

    public InMemoryProxySource()
    {
    }

    public InMemoryProxySource(ProxyType type, string lines)
    {
        Type = type;
        Proxies = lines;
    }

    public string Proxies { get; set; }
    public ProxyType Type { get; set; }


    public Guid Id { get; set; } = Guid.NewGuid();


    [IgnoreMember] public string Name => "Offline source";

    public Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
    {
        return Task.Run(() =>
        {
            List<ProxyInfo> proxies = new();
            try
            {
                foreach (var line in Proxies.Split('\n'))
                    if (ProxyInfo.TryParse(line, Type, out var proxy))
                        proxies.Add(proxy);
            }
            catch
            {
            }

            return (IEnumerable<ProxyInfo>)proxies;
        });
    }
}