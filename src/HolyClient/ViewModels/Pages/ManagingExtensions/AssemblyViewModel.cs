using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;

namespace HolyClient.ViewModels
{
	public enum ConfirmDeleteAssemblyAnswer
	{
		None,
		WaitAndDelete,
		ForceDelete
	}
	public class AssemblyViewModel : IAssemblyViewModel, IDisposable
	{

		public ICommand ReloadCommand { get; }

		public ICommand DeleteCommand { get; }

		public Interaction<Unit, ConfirmDeleteAssemblyAnswer> ConfirmDeleteDialog { get; } = new();


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
		public AssemblyViewModel(IAssemblyFile assembly, IAssemblyManager assemblyManager)
		{
			CompositeDisposable d = new();



			Name = System.IO.Path.GetFileName(assembly.FullPath);
			Path = assembly.FullPath;

			Types = assembly.StressTestPlugins
				.Select(x => new PluginViewModel(x.FullName))
				.ToList();


			DeleteCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				var answer = await ConfirmDeleteDialog.Handle(default);

				if (answer == ConfirmDeleteAssemblyAnswer.ForceDelete)
				{
					IStressTest stressTest = Locator.Current.GetService<IStressTest>();

					if (stressTest.CurrentState != StressTestServiceState.None)
					{
						await stressTest.Stop();
					}

					stressTest.DeleteBehavior();


					await assemblyManager.RemoveAssembly(assembly);

				}
				else if (answer == ConfirmDeleteAssemblyAnswer.WaitAndDelete)
				{
					throw new NotImplementedException("Удаление сборки после остановки всех плагинов пока не поддерживается");
				}

			}).DisposeWith(d);




			_cleanUp = d;
		}

		public void Dispose()
		{
			Interlocked.Exchange(ref _cleanUp, null)?.Dispose();
		}
	}







}
