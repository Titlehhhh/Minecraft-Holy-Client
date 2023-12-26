using FluentAvalonia.UI.Controls;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{
	public class ImportSourceViewModel
	{
		public string Icon { get; }
		public string Description { get; }
		public string Name { get; }
		public ImportSource SourceType { get; }

		public ImportSourceViewModel(string icon, ImportSource sourceType)
		{
			
			SourceType = sourceType;
			Icon = icon;
			string baseTr = $"StressTest.Configuration.Proxy.Dialog.SelectSource.{icon}";
			Description = $"{baseTr}.Description";
			Name = baseTr;
		}
	}
}
