using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient;

public partial class RootView : ReactiveUserControl<RootViewModel>
{
    public RootView()
    {
        InitializeComponent();
    }
}