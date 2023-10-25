using DynamicData;

namespace HolyClient.Core.Infrastructure
{

	public interface IAssemblyManager
	{
		IConnectableCache<IAssemblyFile, string> Assemblies { get; }

		IObservable<IAssemblyFile> AssemblyFileUpdated { get; }

		Task AddReference(string path);
		Task Initialization();
	}
}
