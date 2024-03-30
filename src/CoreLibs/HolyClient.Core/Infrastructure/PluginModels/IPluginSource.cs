using HolyClient.Abstractions.StressTest;

namespace HolyClient.Core.Infrastructure
{
	public interface IPluginSource
	{
		PluginMetadata Metadata { get; }
		PluginTypeReference Reference { get; }

		T CreateInstance<T>() where T : IStressTestBehavior;
	}


}
