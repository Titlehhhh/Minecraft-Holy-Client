using HolyClient.AppState;
using HolyClient.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HolyClient.ViewModels;
public class MainViewModel : ReactiveObject, IActivatableViewModel, IScreen, IRoutableViewModel
{


	[Reactive]
	public Page SelectedPage { get; set; }

	public ViewModelActivator Activator { get; } = new();

	public RoutingState Router { get; }

	public string? UrlPathSegment => throw new NotImplementedException();

	public IScreen HostScreen { get; private set; }

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



}

