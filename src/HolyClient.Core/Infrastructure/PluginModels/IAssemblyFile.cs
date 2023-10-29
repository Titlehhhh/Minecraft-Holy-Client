using DynamicData;
using System.Reactive;

namespace HolyClient.Core.Infrastructure
{
	public interface IPluginProvider
	{
		IConnectableCache<IPluginSource, PluginTypeReference> AvailableStressTestPlugins { get; }
	}
	public interface IAssemblyFile
	{
		string FullPath { get; }

		string NameWithExtension { get; }

		IEnumerable<Type> StressTestPlugins { get; }

		Version Version { get; }
		IObservable<Unit> FileUpdated { get; }

		Task Initialization();

	}
}
