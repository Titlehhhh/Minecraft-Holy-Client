using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Common;
using McProtoNet;
using System.ComponentModel;

namespace HolyClient.StressTest
{
	[MessagePack.Union(0, typeof(StressTest))]
	public interface IStressTest : INotifyPropertyChanged, INotifyPropertyChanging
	{

		string Server { get; set; }


		string BotsNickname { get; set; }


		int NumberOfBots { get; set; }


		MinecraftVersion Version { get; set; }


		ISourceList<ProxyInfo> Proxies { get; }


		IObservable<StressTestMetrik> Metrics { get; }

		IStressTestBehavior Behavior { get; set; }

		Task Start(Serilog.ILogger logger);
		Task Stop();

	}

}
