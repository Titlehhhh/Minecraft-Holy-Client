using DynamicData;
using HolyClient.Abastractions.StressTest;
using McProtoNet.MultiVersion;
using System.ComponentModel;

namespace HolyClient.Core.StressTest
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
