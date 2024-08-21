using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using HolyClient.AppState;
using HolyClient.Core.Infrastructure;

namespace HolyClient.Models.ManagingExtensions;

public class AssemblyManager : IAssemblyManager
{
    private readonly Subject<IAssemblyFile> _fileUpdated = new();


    public SourceCache<IAssemblyFile, string> _references = new(x => x.Name);
    private readonly ExtensionManagerState _state;


    public AssemblyManager(ExtensionManagerState state)
    {
        _state = state;
    }

    public IObservableCache<IAssemblyFile, string> Assemblies => _references;
    public IObservable<IAssemblyFile> AssemblyFileUpdated => _fileUpdated;

    public async Task AddReference(string path)
    {
        var file = await InitPath(path);
        _state.AddReference(path);
        _references.AddOrUpdate(file);
    }

    public async Task Initialization()
    {
        var assemblies = await Task.WhenAll(_state.References.Select(InitPath));

        _references.AddOrUpdate(assemblies);
    }

    public async Task RemoveReference(string name)
    {
        var assembly = _references.Lookup(name);

        if (assembly.HasValue)
        {
            await assembly.Value.UnLoad();

            _references.RemoveKey(name);
            _state.RemoveReference(assembly.Value.FullPath);
        }
    }

    public Task RemoveAssembly(IAssemblyFile assembly)
    {
        return RemoveReference(assembly.Name);
    }

    private async Task<IAssemblyFile> InitPath(string path)
    {
        var file = new AssemblyFile(path);


        try
        {
            await file.Initialization();
        }
        catch (Exception ex)
        {
        }

        file.FileUpdated.Subscribe(x => { _fileUpdated.OnNext(file); });

        return file;
    }
}