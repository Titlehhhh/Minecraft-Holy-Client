using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.AppState;
using HolyClient.Core.Infrastructure;
using HolyClient.LoadPlugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;

namespace HolyClient.Models.ManagingExtensions
{
	public class AssemblyFile : IAssemblyFile
	{
		private string _path;
		public AssemblyFile(string path)
		{
			_path = path;
			wrapper = new AssemblyWrapper(_path);
		}
		public string FullPath => _path;

		public string NameWithExtension => Path.GetFileName(_path);
		private Subject<Unit> _fileUpdate = new();
		public IObservable<Unit> FileUpdated => _fileUpdate;

		public Version Version { get; private set; }


		public IEnumerable<Type> StressTestPlugins { get; private set; }

		private AssemblyWrapper wrapper;

		private FileSystemWatcher watcher;
		public async Task Initialization()
		{
			string directory = Path.GetDirectoryName(_path);



			await wrapper.Load();

			if (wrapper.CurrentState == PluginState.Loaded)
			{
				this.StressTestPlugins = wrapper.CurrentAssembly
					.GetExportedTypes()
					.Where(x => !x.IsAbstract && typeof(IStressTestBehavior).IsAssignableFrom(x))
					.ToArray();
			}
			//this.watcher = new FileSystemWatcher()
			//{
			//	Path = directory
			//};

			//watcher.NotifyFilter = NotifyFilters.LastWrite;


			//watcher.Changed += OnChanged;
			//watcher.Created += OnCreated;
			//watcher.Deleted += OnDeleted;
			//watcher.Renamed += OnRenamed;
			//watcher.Error += OnError;

			//watcher.Filter = "*.dll";
			//watcher.IncludeSubdirectories = true;
			//watcher.EnableRaisingEvents = true;
			//Console.WriteLine("GG");



		}


		private DateTime lastWriteTime = DateTime.MinValue;
		private async void OnChanged(object sender, FileSystemEventArgs e)
		{
			try
			{

				await Task.Run(() =>
				{
					var name = AssemblyName.GetAssemblyName(_path);
					Console.WriteLine(name);
				});
				Console.WriteLine("Файл изменился, открыть можно");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Файл изменился но нельзя его открыть: " + ex);
			}
		}

		private void OnCreated(object sender, FileSystemEventArgs e)
		{

			string value = $"Created: {e.FullPath}";
			Console.WriteLine(value);
		}

		private void OnDeleted(object sender, FileSystemEventArgs e) =>
			Console.WriteLine($"Deleted: {e.FullPath}");

		private void OnRenamed(object sender, RenamedEventArgs e)
		{
			Console.WriteLine($"Renamed:");
			Console.WriteLine($"    Old: {e.OldFullPath}");
			Console.WriteLine($"    New: {e.FullPath}");
		}

		private void OnError(object sender, ErrorEventArgs e) =>
			PrintException(e.GetException());

		private void PrintException(Exception? ex)
		{
			if (ex != null)
			{
				Console.WriteLine($"Message: {ex.Message}");
				Console.WriteLine("Stacktrace:");
				Console.WriteLine(ex.StackTrace);
				Console.WriteLine();
				PrintException(ex.InnerException);
			}
		}
	}
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
	public class ExtensionManager
	{
		public IAssemblyManager AssemblyManager { get; }



		private ExtensionManagerState _state;
		public ExtensionManager(ExtensionManagerState state)
		{
			_state = state;
			AssemblyManager = new AssemblyManager(state);


		}


		public async Task Initialization()
		{
			await AssemblyManager.Initialization();
		}
	}
}
