using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class HomeView : ReactiveUserControl<IHomeViewModel>
{
    public HomeView()
    {
        InitializeComponent();
    }
}