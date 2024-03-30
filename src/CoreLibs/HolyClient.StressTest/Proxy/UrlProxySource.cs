using HolyClient.Common;
using MessagePack;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	[MessagePackObject(keyAsPropertyName: true)]
	public class UrlProxySource : IProxySource
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public ProxyType Type { get; set; }
		public string Url { get; set; }

		[IgnoreMember]
		public string Name => this.Url;

		public async Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
		{
			List<ProxyInfo> proxies = new();
			try
			{
				using HttpClient httpClient = new HttpClient();

				for (int i = 0; i < 3; i++)
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

					using (StreamReader sr = new StreamReader(stream))
					{
						while (!sr.EndOfStream)
						{
							var line = await sr.ReadLineAsync();
							if (ProxyInfo.TryParse(line.Trim(), this.Type, out var proxy))
							{

								proxies.Add(proxy);
							}
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
		public UrlProxySource()
		{

		}
		public UrlProxySource(ProxyType type, string url)
		{
			Type = type;
			Url = url;
		}
	}

}
