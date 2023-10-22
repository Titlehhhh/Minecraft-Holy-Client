using ReactiveUI;
using System.ComponentModel;

namespace HolyClient.ViewModels
{
	public class ErrorViewModel : IRoutableViewModel
	{
		public string? UrlPathSegment => "/error";
		public string ErrorText { get; }
		public string Description { get; }
		public IScreen HostScreen { get; }

		public ErrorViewModel(IScreen hostScreen, string description, string errorText)
		{
			HostScreen = hostScreen;
			ErrorText = errorText;
			Description = description;

		}

		public event PropertyChangedEventHandler? PropertyChanged;
		public event PropertyChangingEventHandler? PropertyChanging;

		public void RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			throw new System.NotImplementedException();
		}

		public void RaisePropertyChanging(PropertyChangingEventArgs args)
		{
			throw new System.NotImplementedException();
		}
	}



}
