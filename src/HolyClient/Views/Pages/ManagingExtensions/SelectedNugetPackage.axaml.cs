using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class SelectedNugetPackage : ReactiveUserControl<NugetPackageViewModel>
{
    public SelectedNugetPackage()
    {
        InitializeComponent();
    }
}