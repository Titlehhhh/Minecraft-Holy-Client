using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using HolyClient.AppState;
using HolyClient.Services;
using HolyClient.StressTest;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace HolyClient.ViewModels;

public class StressTestViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
    private static int testId = 0;

    private readonly ReadOnlyObservableCollection<StressTestProfileViewModel> _profiles;

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
            .ObserveOn(RxApp.MainThreadScheduler)
            .Skip(1)
            .OnItemAdded(provider =>
            {
                if (provider is not null)
                    SelectedProfile = provider;
            })
            .Subscribe();

        SelectedProfile = _profiles.FirstOrDefault(x => x.Id == state.SelectedProfileId);



        this.WhenAnyValue(x => x.SelectedProfile)
            .Where(x => x is not null)
            .Select(profile => profile.Id)
            .Subscribe(id => state.SelectedProfileId = id);


        Add = ReactiveCommand.Create(() =>
        {
            var i = 1;
            var basename = "New profile ";
            var name = basename + i;
            while (state.Profiles.Items.ToArray().Any(x => x.Name == name)) name = basename + i++;
            var newProfile = new StressTestProfile
            {
                Name = name,
            };
            newProfile.SetBehavior(new DefaultPluginSource());
            state.Profiles.AddOrUpdate(newProfile);
        });

        Remove = ReactiveCommand.CreateFromTask(async () =>
        {
            var ok = await ConfirmRemoveDialog.Handle(Unit.Default);

            if (ok)
                if (SelectedProfile is not null)
                {
                    var id = SelectedProfile.Id;
                    state.Profiles.Remove(id);
                    SelectedProfile = Profiles.FirstOrDefault();
                }
        });
    }

    public Interaction<Unit, bool> ConfirmRemoveDialog { get; } = new();

    public ReadOnlyObservableCollection<StressTestProfileViewModel> Profiles => _profiles;

    [Reactive] public StressTestProfileViewModel? SelectedProfile { get; set; }


    public ICommand Add { get; set; }
    public ICommand Remove { get; set; }
    public ViewModelActivator Activator { get; } = new();

    public string? UrlPathSegment => throw new NotImplementedException();

    public IScreen HostScreen { get; }
}