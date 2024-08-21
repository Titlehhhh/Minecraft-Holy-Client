using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs;

public sealed class SelectImportSourceProxyViewModel : ReactiveObject
{
    public SelectImportSourceProxyViewModel()
    {
        SelectedSource = Sources.FirstOrDefault();
    }

    public ImportSourceViewModel[] Sources { get; } =
    {
        new("ManualEntry", ImportSource.InMemory),
        new("File", ImportSource.File),
        new("Url", ImportSource.Url)
    };

    [Reactive] public ImportSourceViewModel? SelectedSource { get; set; }
}