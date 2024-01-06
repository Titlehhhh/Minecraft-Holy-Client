using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using McProtoNet;
using System.ComponentModel;

namespace HolyClient.StressTest
{

	[MessagePack.Union(0, typeof(StressTestProfile))]
	public interface IStressTestProfile : INotifyPropertyChanged, INotifyPropertyChanging
	{
		Guid Id { get; set; }
		string Name { get; set; }

		

		string Server { get; set; }


		string BotsNickname { get; set; }


		int NumberOfBots { get; set; }

		bool UseProxy { get; set; }
		MinecraftVersion Version { get; set; }


		ISourceCache<IProxySource, Guid> Proxies { get; }


		IObservable<StressTestMetrik> Metrics { get; }

		IStressTestBehavior Behavior { get; }
		StressTestServiceState CurrentState { get; }

		PluginTypeReference BehaviorRef { get; }

		void SetBehavior(IPluginSource pluginSource);
		void DeleteBehavior();

		Task Start(Serilog.ILogger logger);
		Task Stop();

		Task Initialization(IPluginProvider pluginProvider);

	}

}
