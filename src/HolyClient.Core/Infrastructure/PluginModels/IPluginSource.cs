using HolyClient.Abstractions.StressTest;

namespace HolyClient.Core.Infrastructure
{
	public interface IPluginSource
	{
		PluginTypeReference Reference { get; }

		T CreateInstance<T>() where T : IStressTestBehavior;
	}
}
