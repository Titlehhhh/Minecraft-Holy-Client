using HolyClient.Common;
using MessagePack;
using QuickProxyNet;

namespace HolyClient.StressTest;

[MessagePackObject(true)]
public class FileProxySource : IProxySource
{
    public FileProxySource()
    {
    }

    public FileProxySource(ProxyType? type, string filePath)
    {
        Type = type;
        FilePath = filePath;
    }

    public string FilePath { get; set; }
    public ProxyType? Type { get; set; }

    [IgnoreMember] public string Name => FilePath;

    public Guid Id { get; set; } = Guid.NewGuid();


    public async Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
    {
        List<ProxyInfo> proxies = new();
        try
        {
            using (var sr = new StreamReader(FilePath))
            {
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (ProxyInfo.TryParse(line.Trim(), Type, out var proxy)) proxies.Add(proxy);
                }
            }
        }
        catch
        {
        }

        return proxies;
    }
}