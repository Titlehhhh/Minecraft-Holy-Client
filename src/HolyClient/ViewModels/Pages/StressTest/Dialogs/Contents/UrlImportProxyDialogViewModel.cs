namespace HolyClient.ViewModels;
public sealed class UrlImportProxyDialogViewModel : ImportProxyViewModel
{
	public UrlImportProxyDialogViewModel(string title) : base(title)
	{
	}

	public string URL { get; set; }

	public override bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(URL);
	}
}
