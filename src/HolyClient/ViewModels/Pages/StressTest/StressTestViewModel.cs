using DynamicData;
using DynamicData.Binding;
using HolyClient.AppState;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public class StressTestViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();

	public string? UrlPathSegment => throw new NotImplementedException();

	public IScreen HostScreen { get; private set; }

	private ReadOnlyObservableCollection<StressTestProfileViewModel> _profiles;

	public StressTestViewModel()
	{
		HostScreen = null;

		var state = Locator.Current.GetService<StressTestState>();



		state.Profiles
			.Connect()
			.Transform(x => new StressTestProfileViewModel(x))
			.ObserveOn(RxApp.MainThreadScheduler)			
			.Bind(out _profiles)
			.DisposeMany()
			.Subscribe();

		var outputCollectionChanges = Profiles
		   .ToObservableChangeSet(x => x.Id)
		   .Publish()
		   .RefCount();

		outputCollectionChanges
			.Filter(provider => provider.Id == state.SelectedProfileId)
			.ObserveOn(RxApp.MainThreadScheduler)
			.OnItemAdded(provider => SelectedProfile = provider)
			.Subscribe();

		outputCollectionChanges
			.OnItemRemoved(provider => SelectedProfile = null)
			.Subscribe();

		this.WhenAnyValue(x => x.SelectedProfile)
			.Skip(1)
			.Select(provider => provider?.Id ?? Guid.Empty)
			.Subscribe(id => state.SelectedProfileId = id);


		Add = ReactiveCommand.Create(() =>
		{
			state.Profiles.AddOrUpdate(new StressTestProfileState());
		});

		Remove = ReactiveCommand.Create(() =>
		{
			if(SelectedProfile is not null)
			{
				state.Profiles.Remove(SelectedProfile.Id);
			}
		});



	}

	public ReadOnlyObservableCollection<StressTestProfileViewModel> Profiles => _profiles;

	[Reactive]
	public StressTestProfileViewModel SelectedProfile { get; set; }


	public ICommand Add { get; set; }
	public ICommand Remove { get; set; }


}
