using DynamicData;
using HolyClient.Common;
using HolyClient.Core.Models.BotManager;
using HolyClient.StressTest;
using McProtoNet;
using McProtoNet.Events;
using QuickProxyNet;
using Serilog.Core;
using System.IO;

internal partial class Program
{

	private static async Task Main(string[] args)
	{



		IStressTest stressTest = new StressTest()
		{
			BotsNickname = "Title_",
			Server = args[0],
			Version = McProtoNet.MinecraftVersion.MC_1_16_5_Version,
			NumberOfBots = 2000

		};

		stressTest.SetBehavior(new PluginSource());

		stressTest.Metrics.Subscribe(x =>
		{
			Console.WriteLine($"Online: {x.BotsOnline}\tCPS: {x.CPS}");
		});

		SourceList<ProxyInfo> proxies = new();

		ProxyLoaderService proxyLoader = new();

		HttpClient httpClient = new();
		Console.WriteLine("LoadProxy");


		using (var fs = File.OpenRead("http.txt"))
		{
			await proxyLoader.Load(fs, ProxyType.HTTP, proxies);
		}

		using (var stream = await httpClient.GetStreamAsync("https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/http.txt"))
		{
			await proxyLoader.Load(stream, ProxyType.HTTP, proxies);
		}
		using (var stream = await httpClient.GetStreamAsync("https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/socks4.txt"))
		{
			await proxyLoader.Load(stream, ProxyType.SOCKS4, proxies);
		}
		using (var stream = await httpClient.GetStreamAsync("https://raw.githubusercontent.com/jetkai/proxy-list/main/online-proxies/txt/proxies-socks4.txt"))
		{
			await proxyLoader.Load(stream, ProxyType.SOCKS4, proxies);
		}
		



		stressTest.Proxies.AddRange(proxies.Items);

		await stressTest.Start(Logger.None);


		await Task.Delay(-1);
	}


}
