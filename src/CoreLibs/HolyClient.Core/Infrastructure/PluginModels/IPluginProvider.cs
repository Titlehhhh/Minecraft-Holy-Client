using DynamicData;

namespace HolyClient.Core.Infrastructure
{
	public interface IPluginProvider
	{
		IObservableCache<IPluginSource, PluginTypeReference> AvailableStressTestPlugins { get; }

		
	}
}
