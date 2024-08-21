using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public sealed class InMemoryImportProxyDialogViewModel : ImportProxyViewModel
{
    public InMemoryImportProxyDialogViewModel(string title) : base(title)
    {
        Init();
    }

    [Reactive] public string Lines { get; set; } = "";

    private void Init()
    {
        //TODO extract from clipboard
        //TopLevel.GetTopLevel().Clipboard.GetTextAsync();
    }

    public override bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Lines);
    }
}