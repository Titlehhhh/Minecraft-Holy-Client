using DynamicData;

namespace HolyClient.Core.Infrastructure;

public interface IAssemblyManager
{
    IObservableCache<IAssemblyFile, string> Assemblies { get; }

    IObservable<IAssemblyFile> AssemblyFileUpdated { get; }

    Task AddReference(string path);

    Task RemoveReference(string path);
    Task RemoveAssembly(IAssemblyFile assembly);
    Task Initialization();
}