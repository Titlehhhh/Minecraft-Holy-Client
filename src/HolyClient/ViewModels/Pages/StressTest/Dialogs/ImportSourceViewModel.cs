using FluentAvalonia.UI.Controls;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{
	public class ImportSourceViewModel
	{
		public Symbol Icon { get; }
		public string Name { get; }
		public ImportSource SourceType { get; }

		public ImportSourceViewModel(Symbol icon, string name, ImportSource sourceType)
		{
			Name = name;
			SourceType = sourceType;
			Icon = icon;
		}
	}
}
