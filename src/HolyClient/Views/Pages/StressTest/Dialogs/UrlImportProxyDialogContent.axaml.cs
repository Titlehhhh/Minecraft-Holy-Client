using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class UrlImportProxyDialogContent : ReactiveUserControl<UrlImportProxyDialogViewModel>
{
	public UrlImportProxyDialogContent()
	{
		InitializeComponent();
	}
}