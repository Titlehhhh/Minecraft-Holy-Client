using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using ReactiveUI;

namespace HolyClient.Views;

public partial class SelectImportSourceProxyDialogContent : ReactiveUserControl<SelectImportSourceProxyViewModel>
{



	public SelectImportSourceProxyDialogContent()
	{
		InitializeComponent();
		this.WhenActivated(d =>
		{

		});

	}
	private async void OpenFileButton_Click(object? sender, RoutedEventArgs e)
	{
		//var topLevel = TopLevel.GetTopLevel(this);

		//// Start async operation to open the dialog.
		//var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		//{
		//	Title = "",
		//	AllowMultiple = false
		//});
		//if (files is { Count: 1 })
		//{

		//	this.ViewModel.FilePath = files[0].TryGetLocalPath();
		//}
	}
}