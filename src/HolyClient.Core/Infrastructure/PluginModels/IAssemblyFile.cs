using System.Reactive;

namespace HolyClient.Core.Infrastructure
{
	public interface IAssemblyFile
	{
		string FullPath { get; }

		string Name { get; }

		IEnumerable<Type> StressTestPlugins { get; }

		Version Version { get; }
		IObservable<Unit> FileUpdated { get; }

		Task Initialization();

	}
}
