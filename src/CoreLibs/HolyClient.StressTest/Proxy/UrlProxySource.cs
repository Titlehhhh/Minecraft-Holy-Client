using HolyClient.Common;
using MessagePack;
using QuickProxyNet;

namespace HolyClient.StressTest;

[MessagePackObject(true)]
public class UrlProxySource : IProxySource
{
    public UrlProxySource()
    {
    }

    public UrlProxySource(ProxyType? type, string url)
    {
        Type = type;
        Url = url;
    }

    public string Url { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public ProxyType? Type { get; set; }

    [IgnoreMember] public string Name => Url;

    public async Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
    {
        List<ProxyInfo> proxies = new();
        try
        {
            using var httpClient = new HttpClient();

            for (var i = 0; i < 3; i++)
            {
                Stream? stream = null;
                try
                {
                    stream = await httpClient.GetStreamAsync(Url);
                }
                catch
                {
                    continue;
                }

                using (var sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        if (ProxyInfo.TryParse(line.Trim(), Type, out var proxy)) proxies.Add(proxy);
                    }
                }

                break;
            }
        }
        catch
        {
        }

        return proxies;
    }
}