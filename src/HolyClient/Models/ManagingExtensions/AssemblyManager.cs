using DynamicData;
using DynamicData.Kernel;
using HolyClient.AppState;
using HolyClient.Core.Infrastructure;
using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace HolyClient.Models.ManagingExtensions
{
	public class AssemblyManager : IAssemblyManager
	{

		public IObservableCache<IAssemblyFile, string> Assemblies => _references;
		public IObservable<IAssemblyFile> AssemblyFileUpdated => _fileUpdated;


		public SourceCache<IAssemblyFile, string> _references = new(x => x.Name);

		private Subject<IAssemblyFile> _fileUpdated = new();
		private ExtensionManagerState _state;


		public AssemblyManager(ExtensionManagerState state)
		{
			_state = state;
		}

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
		private async Task<IAssemblyFile> InitPath(string path)
		{
			var file = new AssemblyFile(path);


			try
			{
				await file.Initialization();
			}
			catch
			{

			}
			file.FileUpdated.Subscribe(x =>
			{
				this._fileUpdated.OnNext(file);
			});

			return file;
		}

		public async Task RemoveReference(string name)
		{
			Optional<IAssemblyFile> assembly = this._references.Lookup(name);

			if (assembly.HasValue)
			{
				await assembly.Value.UnLoad();
			}

		}

		public Task RemoveAssembly(IAssemblyFile assembly)
		{
			return RemoveReference(assembly.Name);
		}
	}
}
