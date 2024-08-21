using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace HolyClient.ViewModels;

public sealed class ManagingExtensionsViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
    public ManagingExtensionsViewModel()
    {
        HostScreen = Locator.Current.GetService<IScreen>("Main");
    }

    [Reactive] public int SelectedTab { get; set; }


    public ViewModelActivator Activator { get; } = new();


    public string? UrlPathSegment => throw new NotImplementedException();


    public IScreen HostScreen { get; }


    #region Tabs

    public OverviewNugetPackagesViewModel OverviewNugetPackages { get; } = new();

    public AssemblyManagerViewModel Assemblies { get; } = new();

    #endregion
}