using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using HolyClient.Common;
using QuickProxyNet;

namespace HolyClient.Services;

public class ProxyLoaderService : IProxyLoaderService
{
    public async Task<int> Load(Stream stream, ProxyType type, ISourceList<ProxyInfo> sourceList)
    {
        return await Task.Run(async () =>
        {
            var loadedProxies = new List<ProxyInfo>();
            using (var sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                    try
                    {
                        var line = await sr.ReadLineAsync();

                        var HostPort = line.Trim().Split(':');

                        var host = HostPort[0];
                        var port = ushort.Parse(HostPort[1]);

                        var proxy = new ProxyInfo
                        {
                            Host = host,
                            Port = port,
                            Type = type
                        };
                        loadedProxies.Add(proxy);
                    }
                    catch
                    {
                    }
            }

            var count = 0;
            List<ProxyInfo> uniqueProxies = new();


            sourceList.Edit(outProxies =>
            {
                var outProxiesHash = outProxies.ToHashSet();
                var loadedProxiesHash = loadedProxies.ToHashSet();

                foreach (var proxy in loadedProxiesHash)
                    if (outProxiesHash.Add(proxy))
                    {
                        //proxy.Type = type;
                        count++;
                    }


                outProxies.Clear();
                outProxies.AddRange(outProxiesHash);

                loadedProxiesHash.Clear();
                outProxiesHash.Clear();
            });


            return count;
        });
    }
}