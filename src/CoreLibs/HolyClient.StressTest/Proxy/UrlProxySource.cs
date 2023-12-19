using HolyClient.Common;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	public class UrlProxySource : IProxySource
	{
		public ProxyType Type { get; set; }
		public string Url { get; set; }

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
							if (ProxyInfo.TryParse(line.Trim(), out var proxy))
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
	}

}
