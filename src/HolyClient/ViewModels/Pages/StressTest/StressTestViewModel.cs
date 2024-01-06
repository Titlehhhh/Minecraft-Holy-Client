using DynamicData;
using DynamicData.Binding;
using HolyClient.AppState;
using HolyClient.StressTest;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public class StressTestViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
	public ViewModelActivator Activator { get; } = new();

	public string? UrlPathSegment => throw new NotImplementedException();

	public IScreen HostScreen { get; private set; }

	private ReadOnlyObservableCollection<StressTestProfileViewModel> _profiles;

	public Interaction<Unit, bool> ConfirmRemoveDialog { get; } = new();

	private static int testId = 0;
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
			.Subscribe(x =>
			{
				Console.WriteLine("Added");
			});

		//outputCollectionChanges
		//	//.OnItemRemoved(provider => SelectedProfile = null)			
		//	.Subscribe();

		this.WhenAnyValue(x => x.SelectedProfile)
			.Subscribe(x =>
			{
				Console.WriteLine($"Change Profile {testId++}: {(x is null ? "NULL" : x.Name)}");
			});

		this.WhenAnyValue(x => x.SelectedProfile)
			.Skip(1)
			.Select(profile => profile?.Id ?? Guid.Empty)
			.Subscribe(id => state.SelectedProfileId = id);


		Add = ReactiveCommand.Create(() =>
		{
			int i = 1;
			string basename = "New profile ";
			string name = basename + i;
			while (state.Profiles.Items.ToArray().Any(x=>x.Name == name))
			{
				name = basename + (i++);
			}

			state.Profiles.AddOrUpdate(new StressTestProfile()
			{
				Name = name
			});
		});

		Remove = ReactiveCommand.CreateFromTask(async () =>
		{
			bool ok = await ConfirmRemoveDialog.Handle(Unit.Default);

			if (ok)
			{
				if (SelectedProfile is not null)
				{
					var id = SelectedProfile.Id;
					SelectedProfile = Profiles.FirstOrDefault();
					state.Profiles.Remove(id);
					
				}
			}
		});



	}

	public ReadOnlyObservableCollection<StressTestProfileViewModel> Profiles => _profiles;

	[Reactive]
	public StressTestProfileViewModel SelectedProfile { get; set; }


	public ICommand Add { get; set; }
	public ICommand Remove { get; set; }


}
