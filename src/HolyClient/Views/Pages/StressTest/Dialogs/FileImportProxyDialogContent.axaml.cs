using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class FileImportProxyDialogContent : ReactiveUserControl<FileImportProxyDialogViewModel>
{
    public FileImportProxyDialogContent()
    {
        InitializeComponent();
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
        if (files is { Count: 1 }) ViewModel.FilePath = files[0].TryGetLocalPath();
    }
}