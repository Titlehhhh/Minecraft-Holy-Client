using DynamicData;
using HolyClient.Common;
using QuickProxyNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HolyClient.Services
{
	public class ProxyLoaderService : IProxyLoaderService
	{
		public async Task<int> Load(Stream stream, ProxyType type, ISourceList<ProxyInfo> sourceList)
		{
			return await Task.Run(async () =>
			{
				var loadedProxies = new List<ProxyInfo>();
				using (StreamReader sr = new StreamReader(stream))
				{
					while (!sr.EndOfStream)
					{
						try
						{
							var line = await sr.ReadLineAsync();

							string[] HostPort = line.Trim().Split(':');

							string host = HostPort[0];
							ushort port = ushort.Parse(HostPort[1]);

							ProxyInfo proxy = new ProxyInfo()
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
				}
				int count = 0;
				List<ProxyInfo> uniqueProxies = new();
				var comparer = new ProxyComparer();

				sourceList.Edit(outProxies =>
				{
					var outProxiesHash = outProxies.ToHashSet(comparer);
					var loadedProxiesHash = loadedProxies.ToHashSet(comparer);

					foreach (var proxy in loadedProxiesHash)
					{
						if (outProxiesHash.Add(proxy))
						{
							proxy.Type = type;
							count++;
						}
						else
						{

						}

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

}
