using ReactiveUI;

namespace HolyClient.ViewModels
{
	public class LoadingViewModel : ReactiveObject, IRoutableViewModel
	{
		public string? UrlPathSegment => "/loading";

		public IScreen HostScreen { get; }

		public string Text { get; }


		public LoadingViewModel(IScreen screen, string text)
		{
			HostScreen = screen;
			Text = text;
		}

	}
}
