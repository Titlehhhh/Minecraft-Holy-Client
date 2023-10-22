using HolyClient.ViewModels;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;

namespace HolyClient.DesignTime
{
	public class DesignBotManagerViewModel : IBotManagerViewModel
	{
		public DesignBotManagerViewModel()
		{
			ObservableCollection<IBotProfileViewModel> profiles = new()
			{
				new DesignBotProfileViewModel()
				{
					Name = "New Profile"
				}
			};
			Profiles = new(profiles);
			SelectedProfile = Profiles.First();

		}

		public ViewModelActivator Activator { get; } = new();

		public ReactiveCommand<Unit, Unit> CreateProfileCommand { get; }

		public IScreen HostScreen => null;

		public ReadOnlyObservableCollection<IBotProfileViewModel> Profiles { get; }

		public RoutingState Router { get; set; }


		public IBotProfileViewModel SelectedProfile { get; set; }

		public string? UrlPathSegment => "botManager";

		public Interaction<Unit, bool> Dialog => throw new NotImplementedException();

		public ReactiveCommand<Unit, Unit> RemoveProfileCommand => throw new NotImplementedException();

		public event PropertyChangedEventHandler? PropertyChanged;
		public event PropertyChangingEventHandler? PropertyChanging;

		public void RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			throw new NotImplementedException();
		}

		public void RaisePropertyChanging(PropertyChangingEventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
