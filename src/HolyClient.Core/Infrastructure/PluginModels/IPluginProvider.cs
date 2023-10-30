using DynamicData;

namespace HolyClient.Core.Infrastructure
{
	public interface IPluginProvider
	{
		IConnectableCache<IPluginSource, PluginTypeReference> AvailableStressTestPlugins { get; }
	}
}
