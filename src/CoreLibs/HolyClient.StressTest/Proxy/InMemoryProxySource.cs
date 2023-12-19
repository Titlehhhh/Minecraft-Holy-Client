using HolyClient.Common;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	public class InMemoryProxySource : IProxySource
	{
		public ProxyType Type { get; set; }

		public IEnumerable<ProxyInfo> Proxies { get; set; } 

		public Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
		{
			return Task.FromResult(this.Proxies);
		}
		public InMemoryProxySource()
		{

		}
		public InMemoryProxySource(ProxyType type, IEnumerable<ProxyInfo> proxies)
		{
			Type = type;
			Proxies = proxies;
		}
	}

}
