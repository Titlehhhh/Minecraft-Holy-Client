using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}