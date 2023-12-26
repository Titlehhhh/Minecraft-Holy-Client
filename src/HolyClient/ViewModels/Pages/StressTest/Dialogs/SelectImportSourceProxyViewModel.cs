using FluentAvalonia.UI.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{
	public sealed class SelectImportSourceProxyViewModel : ReactiveObject
	{

		public ImportSourceViewModel[] Sources { get; } = new ImportSourceViewModel[]
		{
			new ImportSourceViewModel("ManualEntry","Буфер обмена", ImportSource.InMemory),
			new ImportSourceViewModel("FileSource","Файл", ImportSource.File),
			new ImportSourceViewModel("UrlSource","URL", ImportSource.Url)
		};
		[Reactive]
		public ImportSourceViewModel? SelectedSource { get; set; }

		public SelectImportSourceProxyViewModel()
		{
			SelectedSource = Sources.FirstOrDefault();
		}

	}
}
