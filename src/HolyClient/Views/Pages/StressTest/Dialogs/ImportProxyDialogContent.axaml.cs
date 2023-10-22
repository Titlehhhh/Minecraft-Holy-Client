using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using ReactiveUI;

namespace HolyClient.Views.Pages.StressTest.Dialogs;

public partial class ImportProxyDialogContent : ReactiveUserControl<ImportProxyViewModel>
{
	public ImportProxyDialogContent()
	{
		InitializeComponent();
		this.WhenActivated(d =>
		{
			this.ViewModel.ImportingTaskDialog.RegisterHandler(x =>
			{

			});
		});

	}
	private async void OpenFileButton_Click(object? sender, RoutedEventArgs e)
	{
		var topLevel = TopLevel.GetTopLevel(this);

		// Start async operation to open the dialog.
		var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			Title = "Открыть файл с прокси",
			AllowMultiple = false
		});
		if (files is { Count: 1 })
		{

			this.ViewModel.FilePath = files[0].TryGetLocalPath();
		}
	}
}