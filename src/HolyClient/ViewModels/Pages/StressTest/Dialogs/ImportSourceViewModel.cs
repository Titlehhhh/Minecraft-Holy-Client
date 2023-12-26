using FluentAvalonia.UI.Controls;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{
	public class ImportSourceViewModel
	{
		public string Icon { get; }
		public string Description { get; set; }
		public string Name { get; }
		public ImportSource SourceType { get; }

		public ImportSourceViewModel(string icon, string name, ImportSource sourceType)
		{
			Name = name;
			SourceType = sourceType;
			Icon = icon;
		}
	}
}
