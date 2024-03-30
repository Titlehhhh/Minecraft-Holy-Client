namespace HolyClient.ViewModels;

public sealed class FileImportProxyDialogViewModel : ImportProxyViewModel
{

	public string FilePath
	{
		get;
		set;
	}



	public FileImportProxyDialogViewModel(string title) : base(title)
	{

	}

	public override bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(FilePath);
	}
}
