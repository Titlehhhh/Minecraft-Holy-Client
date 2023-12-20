using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public sealed class InMemoryImportProxyDialogViewModel : ImportProxyViewModel
{
	[Reactive]
	public string Lines { get; set; }

	

	public InMemoryImportProxyDialogViewModel()
	{
		Init();
	}
	private void Init()
	{
		//TODO extract from clipboard
		//TopLevel.GetTopLevel().Clipboard.GetTextAsync();
	}
}
