using HolyClient.Common;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	[MessagePack.Union(0, typeof(InMemoryProxySource))]
	[MessagePack.Union(1, typeof(FileProxySource))]
	[MessagePack.Union(2, typeof(UrlProxySource))]
	public interface IProxySource
	{
		Guid Id { get; }
		string Name { get;}

		ProxyType Type { get; set; }



		Task<IEnumerable<ProxyInfo>> GetProxiesAsync();
	}

}
