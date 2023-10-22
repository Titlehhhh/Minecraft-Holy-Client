using DynamicData;
using HolyClient.Core.StressTest;

using QuickProxyNet;
using Serilog.Core;
using System.Diagnostics.CodeAnalysis;

internal class Program
{
	public class ProxyComparer : IEqualityComparer<ProxyInfo>
	{
		public bool Equals(ProxyInfo? x, ProxyInfo? y)
		{
			if (x is null)
			{
				return false;
			}
			if (y is null)
			{
				return false;
			}



			return x.Host == y.Host && x.Port == y.Port;
		}

		public int GetHashCode([DisallowNull] ProxyInfo obj)
		{
			unchecked
			{
				int hash = 27;
				hash = (13 * hash) + obj.Host.GetHashCode();
				hash = (13 * hash) + obj.Port.GetHashCode();

				return hash;
			}
		}
	}
	public class ProxyLoaderService 
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
	

	private static async Task Main(string[] args)
	{

		var proxyLoader = new ProxyLoaderService();

		IStressTest stressTest = new StressTest()
		{
			 BotsNickname = "Title_",
			  NumberOfBots = 2000,
			   Server = "itski.aternos.me", 
			    Version = McProtoNet.MultiVersion.MinecraftVersion.MC_1_16_5_Version
				
		};

		using(var fs = File.OpenRead(@"C:\Users\Title\Downloads\socks4.txt"))
		{
			await proxyLoader.Load(fs, QuickProxyNet.ProxyType.SOCKS4, stressTest.Proxies);
		}
		using (var fs = File.OpenRead(@"C:\Users\Title\Downloads\socks5.txt"))
		{
			await proxyLoader.Load(fs, QuickProxyNet.ProxyType.SOCKS5, stressTest.Proxies);
		}
		using (var fs = File.OpenRead(@"C:\Users\Title\Downloads\http.txt"))
		{
			await proxyLoader.Load(fs, QuickProxyNet.ProxyType.HTTP, stressTest.Proxies);
		}

		stressTest.Metrics.Subscribe(x =>
		{
			Console.WriteLine($"Online: {x.BotsOnline} CPS: {x.CPS}");
		});
		await stressTest.Start(Logger.None);

		await Task.Delay(-1);

	}
}