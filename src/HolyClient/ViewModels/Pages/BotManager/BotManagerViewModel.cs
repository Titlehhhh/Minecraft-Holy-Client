using DynamicData;
using DynamicData.Binding;
using HolyClient.Contracts.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HolyClient.ViewModels;


public class BotManagerViewModel : ReactiveObject, IRoutableViewModel, IBotManagerViewModel, IActivatableViewModel
{



	public string? UrlPathSegment => "/botmanager";

	public IScreen HostScreen { get; }



	[Reactive]
	public ReadOnlyObservableCollection<IBotProfileViewModel> Profiles { get; private set; }

	[Reactive]
	public ReactiveCommand<Unit, Unit> CreateProfileCommand { get; private set; }
	[Reactive]
	public ReactiveCommand<Unit, Unit> RemoveProfileCommand { get; private set; }



	[Reactive]
	public IBotProfileViewModel SelectedProfile { get; set; }

	public ViewModelActivator Activator { get; } = new();

	public Interaction<Unit, bool> Dialog { get; } = new();

	public BotManagerViewModel()
	{
		var state = Locator.Current.GetService<IBotManager>();

		CreateProfileCommand =
			ReactiveCommand.Create(() =>
			{
				var newProfile = state.CreateAndAddBot();
				newProfile.Name = "New Profile";
			});

		RemoveProfileCommand =
			ReactiveCommand.CreateFromTask(async () =>
			{
				if (await Dialog.Handle(Unit.Default) && SelectedProfile is { })
				{
					state.RemoveBot(SelectedProfile.Id);
					SelectedProfile = Profiles.FirstOrDefault();
				}
				else
				{

				}


			});


		state.Profiles
			.Connect()
			.Transform(x =>
			{
				return (IBotProfileViewModel)new BotProfileViewModel(x);
			})
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out var _profiles)
			.DisposeMany()
			.Subscribe();



		Profiles = _profiles;
		var outputCollectionChanges = Profiles
		.ToObservableChangeSet(x => x.Id)
		.Publish()
		.RefCount();





		outputCollectionChanges
			.ObserveOn(RxApp.MainThreadScheduler)
			.OnItemAdded(profile => SelectedProfile = profile)
			.Subscribe();


		outputCollectionChanges
			.ObserveOn(RxApp.MainThreadScheduler)
			.OnItemRemoved(profile =>
			{
				SelectedProfile = Profiles.FirstOrDefault();
			})
			.Subscribe();



		this.WhenActivated(d =>
		{
			SelectedProfile = _profiles.FirstOrDefault(x => x.Id == state.SelectedProfile, _profiles.FirstOrDefault());
			this.WhenAnyValue(x => x.SelectedProfile)
				.Where(x => x is { })
				.Subscribe(x =>
				{
					Console.WriteLine("Select: " + x.Id);
					state.SelectedProfile = x.Id;
				})
				.DisposeWith(d);
		});
	}


}


