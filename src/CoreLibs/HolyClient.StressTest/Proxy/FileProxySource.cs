﻿using HolyClient.Common;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	[MessagePackObject(keyAsPropertyName: true)]
	public class FileProxySource : IProxySource
	{
		public ProxyType Type { get; set; }
		public string FilePath { get; set; }
		public async Task<IEnumerable<ProxyInfo>> GetProxiesAsync()
		{
			List<ProxyInfo> proxies = new();
			try
			{
				using (StreamReader sr = new StreamReader(FilePath))
				{
					while (!sr.EndOfStream)
					{
						var line = await sr.ReadLineAsync();
						if (ProxyInfo.TryParse(line.Trim(),this.Type, out var proxy))
						{
							proxies.Add(proxy);
						}
					}
				}
			}
			catch
			{

			}
			return proxies;
		}
		public FileProxySource()
		{

		}
		public FileProxySource(ProxyType type, string filePath)
		{
			Type = type;
			FilePath = filePath;
		}
	}

}
