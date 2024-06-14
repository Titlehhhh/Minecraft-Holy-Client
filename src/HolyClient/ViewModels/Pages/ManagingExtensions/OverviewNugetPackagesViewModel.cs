using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using DynamicData;
using HolyClient.Contracts.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace HolyClient.ViewModels;

public sealed class OverviewNugetPackagesViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly INugetClient _client;

    private CancellationTokenSource CTS;

    public OverviewNugetPackagesViewModel()
    {
        var client = Locator.Current.GetService<INugetClient>();
        _client = client;

        this.WhenActivated(d =>
        {
            OnScrollEndCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                IsLoadingNewItems = true;
                await client.LoadNewItems();
                IsLoadingNewItems = false;
            }, client.WhenAnyValue(x => x.CanLoadNew)).DisposeWith(d);

            RefreshCommand = ReactiveCommand.Create(Refresh).DisposeWith(d);


            var searchCommand = ReactiveCommand.Create(Refresh).DisposeWith(d);

            client.WhenAnyValue(x => x.IsLoading)
                .BindTo(this, x => x.IsLoading)
                .DisposeWith(d);

            this.WhenAnyValue(x => x.Filter)
                .BindTo(client, x => x.Filter)
                .DisposeWith(d);

            this.WhenAnyValue(x => x.Filter)
                .Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .Merge(searchCommand.Select(x => Filter).Distinct())
                .Subscribe(x => { Refresh(); })
                .DisposeWith(d);

            this.WhenAnyValue(x => x.EnablePreviewVersions)
                .BindTo(client, x => x.EnablePreviewVersions)
                .DisposeWith(d);

            this.WhenAnyValue(x => x.EnablePreviewVersions)
                .Skip(1)
                .Subscribe(x => { Refresh(); })
                .DisposeWith(d);

            client.Packages
                .Connect()
                .Transform(p => { return new NugetPackageViewModel(p); })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out var packages)
                .Subscribe()
                .DisposeWith(d);

            Packages = packages;
            SearchCommand = searchCommand;
            Refresh();
        });
    }


    [Reactive] public ReadOnlyObservableCollection<NugetPackageViewModel> Packages { get; private set; }

    [Reactive] public NugetPackageViewModel? SelectedPackage { get; set; }


    [Reactive] public ICommand RefreshCommand { get; private set; }

    [Reactive] public ICommand SearchCommand { get; private set; }

    [Reactive] public ICommand OnScrollEndCommand { get; private set; }

    [Reactive] public bool IsLoading { get; set; } = false;

    [Reactive] public bool IsLoadingNewItems { get; set; }

    [Reactive] public bool EnablePreviewVersions { get; set; }

    [Reactive] public string Filter { get; set; }

    public ViewModelActivator Activator { get; } = new();


    private async void Refresh()
    {
        SelectedPackage = null;
        try
        {
            await _client.Refresh();
        }
        catch (Exception ex)
        {
        }
    }
}