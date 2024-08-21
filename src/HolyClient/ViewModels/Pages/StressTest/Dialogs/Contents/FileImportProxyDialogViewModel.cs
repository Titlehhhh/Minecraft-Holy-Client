namespace HolyClient.ViewModels;

public sealed class FileImportProxyDialogViewModel : ImportProxyViewModel
{
    public FileImportProxyDialogViewModel(string title) : base(title)
    {
    }

    public string FilePath { get; set; }

    public override bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(FilePath);
    }
}