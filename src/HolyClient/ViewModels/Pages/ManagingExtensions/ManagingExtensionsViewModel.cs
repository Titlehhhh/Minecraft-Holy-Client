﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace HolyClient.ViewModels
{

	public sealed class ManagingExtensionsViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
	{


		#region Tabs
		public OverviewNugetPackagesViewModel OverviewNugetPackages { get; } = new();

		public AssemblyManagerViewModel Assemblies { get; } = new();
		#endregion


		public string? UrlPathSegment => throw new System.NotImplementedException();


		public IScreen HostScreen { get; }

		[Reactive]
		public int SelectedTab { get; set; }




		public ViewModelActivator Activator { get; } = new();

		public ManagingExtensionsViewModel()
		{
			HostScreen = Locator.Current.GetService<IScreen>("Main");
		}



	}




}
