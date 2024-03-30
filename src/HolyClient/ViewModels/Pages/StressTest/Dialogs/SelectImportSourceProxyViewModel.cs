using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{
	public sealed class SelectImportSourceProxyViewModel : ReactiveObject
	{

		public ImportSourceViewModel[] Sources { get; } = new ImportSourceViewModel[]
		{
			new ImportSourceViewModel("ManualEntry", ImportSource.InMemory),
			new ImportSourceViewModel("File", ImportSource.File),
			new ImportSourceViewModel("Url", ImportSource.Url)
		};
		[Reactive]
		public ImportSourceViewModel? SelectedSource { get; set; }

		public SelectImportSourceProxyViewModel()
		{
			SelectedSource = Sources.FirstOrDefault();
		}

	}
}
