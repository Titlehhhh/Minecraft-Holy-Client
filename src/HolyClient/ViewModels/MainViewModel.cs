using System;
using System.Reactive.Disposables;
using HolyClient.AppState;
using HolyClient.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace HolyClient.ViewModels;

public class MainViewModel : ReactiveObject, IActivatableViewModel, IScreen, IRoutableViewModel
{
    public MainViewModel(MainState mainState)
    {
        Router = new RoutingState();

        this.WhenActivated(d =>
        {
            SelectedPage = mainState.SelectedPage;
            this.WhenAnyValue(x => x.SelectedPage)
                .Subscribe(x =>
                {
                    mainState.SelectedPage = x;
                    var viewModel = Locator.Current.GetService<IRoutableViewModel>(x.ToString());
                    Router.NavigateAndReset.Execute(viewModel);
                })
                .DisposeWith(d);
        });
    }


    [Reactive] public Page SelectedPage { get; set; }

    public ViewModelActivator Activator { get; } = new();

    public string? UrlPathSegment => throw new NotImplementedException();

    public IScreen HostScreen { get; }

    public RoutingState Router { get; }
}