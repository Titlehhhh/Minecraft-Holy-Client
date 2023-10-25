using DynamicData;
using HolyClient.Core.Infrastructure;
using HolyClient.Models.ManagingExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HolyClient.ViewModels
{
	public sealed class AssemblyManagerViewModel : ReactiveObject, IActivatableViewModel
	{

		[Reactive]
		public string Filter { get; set; }


		[Reactive]
		public ReadOnlyObservableCollection<IAssemblyViewModel> Assemblies { get; private set; }


		[Reactive]
		public IAssemblyViewModel? SelectedItem { get; set; }

		[Reactive]
		public IEnumerable<object> SelectedItems { get; set; }


		[Reactive]
		public ICommand AddAssemblyCommand { get; private set; }

		[Reactive]
		public ICommand RemoveAssemblyCommand { get; private set; }





		public Interaction<Unit, bool> ConfirmDeleteDialog { get; } = new();

		public Interaction<Unit, Uri?> OpenFileDialog { get; } = new();

		public ViewModelActivator Activator { get; } = new();
		private readonly ExtensionManager manager;
		public AssemblyManagerViewModel()
		{

			manager = Locator.Current.GetService<ExtensionManager>();


			var filter = this.WhenAnyValue(x => x.Filter).Select(this.BuildFilter);


			manager.AssemblyManager.Assemblies.Connect()
				.Transform(CreateVM)
				//.Filter(filter)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Bind(out var assemblies)
				.DisposeMany()
				.Subscribe();

			Assemblies = assemblies;

			AddAssemblyCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				var path = await OpenFileDialog.Handle(default);

				if (path is not null)
				{

					await manager.AssemblyManager.AddReference(path.LocalPath);
				}
			});





			SelectedItem = Assemblies.FirstOrDefault();



		}

		private IAssemblyViewModel CreateVM(IAssemblyFile model)
		{
			return new AssemblyViewModel(model);
		}

		private Func<IAssemblyViewModel, bool> BuildFilter(string filter)
		{
			if (string.IsNullOrWhiteSpace(filter))
				return assembly => true;

			return assembly => assembly.Name.Contains(filter, StringComparison.OrdinalIgnoreCase);
		}
	}






}
