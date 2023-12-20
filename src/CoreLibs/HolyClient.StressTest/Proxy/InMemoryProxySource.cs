using HolyClient.Common;
using MessagePack;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	[MessagePackObject(keyAsPropertyName: true)]
	public class InMemoryProxySource : IProxySource
	{
		public ProxyType Type { get; set; }

		public string Proxies { get; set; }

		public Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
		{
			return Task.Run(() =>
			{
				List<ProxyInfo> proxies = new();
				try
				{
					foreach (var line in Proxies.Split('\n'))
					{
						if(ProxyInfo.TryParse(line,this.Type, out var proxy))
						{
							proxies.Add(proxy);
						}
					}
				}
				catch
				{

				}
				return (IEnumerable<ProxyInfo>)proxies;
			});
		}
		public InMemoryProxySource()
		{

		}
		
		public InMemoryProxySource(ProxyType type, string lines)
		{
			Type = type;

		}
	}

}
