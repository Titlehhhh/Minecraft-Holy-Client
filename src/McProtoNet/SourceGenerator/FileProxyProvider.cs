using McProtoNet.MultiVersion.Proxy;
using QuickProxyNet;

namespace SourceGenerator
{
    class FileProxyProvider : IProxyProvider
    {
        public async Task<IEnumerable<IProxyClient>> GetProxiesAsync(CancellationToken token)
        {
            var result = new List<IProxyClient>();
            using (StreamReader sr = new StreamReader("S4Proxies.txt"))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        var line = (await sr.ReadLineAsync()).Trim();
                        string[] HostPort = line.Split(':');
                        result.Add(new Socks4Client(HostPort[0], ushort.Parse(HostPort[1])));
                    }
                    catch
                    {

                    }
                }

            }
            return result;
        }
    }
}
