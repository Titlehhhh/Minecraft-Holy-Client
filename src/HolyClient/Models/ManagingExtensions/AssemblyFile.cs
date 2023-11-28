using HolyClient.Abstractions.StressTest;
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

		public string Name => Path.GetFileName(_path);
		private Subject<Unit> _fileUpdate = new();
		public IObservable<Unit> FileUpdated => _fileUpdate;

		public Version Version { get; private set; }


		public IEnumerable<Type> StressTestPlugins { get; private set; } = Enumerable.Empty<Type>();

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
					.Where(x =>
					{
						Console.WriteLine("Type: "+x);

						var g= !x.IsAbstract && typeof(IStressTestBehavior).IsAssignableFrom(x);
						return g;
					})
					.ToArray();
			}
			this.watcher = new FileSystemWatcher()
			{
				Path = directory
			};

			watcher.NotifyFilter = NotifyFilters.LastWrite;


			watcher.Changed += OnChanged;
			watcher.Created += OnCreated;
			watcher.Deleted += OnDeleted;
			watcher.Renamed += OnRenamed;
			//watcher.Error += OnError;

			watcher.Filter = "*.dll";
			watcher.IncludeSubdirectories = true;
			watcher.EnableRaisingEvents = true;




		}


		private DateTime lastWriteTime = DateTime.MinValue;
		private async void OnChanged(object sender, FileSystemEventArgs e)
		{
			try
			{
				await Task.Run(() =>
				{
					var name = AssemblyName.GetAssemblyName(_path);

				});
				this._fileUpdate.OnNext(default);
			}
			catch (Exception ex)
			{

			}
		}

		private void OnCreated(object sender, FileSystemEventArgs e)
		{

		}

		private void OnDeleted(object sender, FileSystemEventArgs e)
		{

		}


		private void OnRenamed(object sender, RenamedEventArgs e)
		{
		}

		public async Task UnLoad()
		{
			await wrapper.UnLoad();
			watcher.Changed -= OnChanged;
			watcher.Created -= OnCreated;
			watcher.Deleted -= OnDeleted;
			watcher.Renamed -= OnRenamed;
			watcher.Dispose();
		}
	}
}
