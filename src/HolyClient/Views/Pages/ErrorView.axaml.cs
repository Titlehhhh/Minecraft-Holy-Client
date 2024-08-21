using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class ErrorView : ReactiveUserControl<ErrorViewModel>
{
    public ErrorView()
    {
        InitializeComponent();
    }
}