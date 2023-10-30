using HolyClient.Core.Infrastructure;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace HolyClient.ViewModels
{
	public class AssemblyViewModel : IAssemblyViewModel, IDisposable
	{

		public ICommand ReloadCommand { get; }




		[Reactive]
		public string Description { get; set; }
		[Reactive]
		public Version Version { get; set; }
		[Reactive]
		public string Path { get; set; }
		[Reactive]
		public string Name { get; set; }

		[ReactiveDependency(nameof(Name))]
		public string NameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(Name);




		[Reactive]
		public string Author { get; set; }

		public List<PluginViewModel> Types { get; set; }

		private IDisposable? _cleanUp;
		public AssemblyViewModel(string name, Version version, string author)
		{
			Name = name;
			Version = version;
			Author = author;
			Path = "C:\\Users\\Title\\source\\repos\\HolyClient\\HolyClient.Desktop\\bin\\Release\\net8.0";
		}
		public AssemblyViewModel(IAssemblyFile assembly)
		{
			CompositeDisposable d = new();



			Name = System.IO.Path.GetFileName(assembly.FullPath);
			Path = assembly.FullPath;

			Types = assembly.StressTestPlugins
				.Select(x => new PluginViewModel(x.FullName))
				.ToList();






			_cleanUp = d;
		}

		public void Dispose()
		{

		}
	}


	




}
