using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace HolyClient.ViewModels
{

	public sealed class ManagingExtensionsViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
	{


		#region Tabs
		public OverviewNugetPackagesViewModel OverviewNugetPackages { get; } = new();

		public AssembliesViewModel Assemblies { get; } = new();
		#endregion


		public string? UrlPathSegment => throw new System.NotImplementedException();


		public IScreen HostScreen { get; }

		[Reactive]
		public int SelectedTab { get; set; }




		public ViewModelActivator Activator { get; } = new();

		public ManagingExtensionsViewModel()
		{
			System.Console.WriteLine("CTOR MEVM");
			HostScreen = Locator.Current.GetService<IScreen>("Main");
		}



	}




}
