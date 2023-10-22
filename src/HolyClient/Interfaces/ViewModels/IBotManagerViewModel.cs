using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace HolyClient.ViewModels
{
	public interface IBotManagerViewModel
	{
		public Interaction<Unit, bool> Dialog { get; }

		ViewModelActivator Activator { get; }
		ReactiveCommand<Unit, Unit> CreateProfileCommand { get; }
		ReactiveCommand<Unit, Unit> RemoveProfileCommand { get; }

		ReadOnlyObservableCollection<IBotProfileViewModel> Profiles { get; }

		IBotProfileViewModel SelectedProfile { get; set; }
		string? UrlPathSegment { get; }
	}
}