using DynamicData;
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
		public SourceCache<IAssemblyFile, string> _references = new(x => x.NameWithExtension);
		public IConnectableCache<IAssemblyFile, string> Assemblies => _references;
		private Subject<IAssemblyFile> _fileUpdated = new();
		public IObservable<IAssemblyFile> AssemblyFileUpdated => _fileUpdated;
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



			await file.Initialization();
			file.FileUpdated.Subscribe(x =>
			{
				this._fileUpdated.OnNext(file);
			});

			return file;
		}
	}
}
