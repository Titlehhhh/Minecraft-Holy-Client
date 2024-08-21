using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class AssemblyManagerView : ReactiveUserControl<AssemblyManagerViewModel>
{
    public AssemblyManagerView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel.OpenFileDialog.RegisterHandler(async d =>
            {
                var result = await OpenFileDialog();
                d.SetOutput(result);
            });
        });
    }

    public async Task<Uri?> OpenFileDialog()
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open assembly File",

            AllowMultiple = false
        });

        if (files.Count >= 1) return files[0].Path;
        return null;
    }
}