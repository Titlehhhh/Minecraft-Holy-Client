using HolyClient.AppState;
using HolyClient.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive.Linq;

namespace HolyClient.ViewModels;
public class MainViewModel : ReactiveObject, IActivatableViewModel, IScreen
{


	[Reactive]
	public Page SelectedPage { get; set; }

	public ViewModelActivator Activator { get; } = new();

	public RoutingState Router { get; }

	public MainViewModel()
	{
		Router = new RoutingState();
		Locator.CurrentMutable.RegisterConstant<IScreen>(this, "Main");
	}

	public void OnLoadState(MainState mainState)
	{
		SelectedPage = mainState.SelectedPage;
		this.WhenAnyValue(x => x.SelectedPage)
			.Subscribe(x =>
			{
				mainState.SelectedPage = x;
				var viewModel = Locator.Current.GetService<IRoutableViewModel>(x.ToString());
				Router.NavigateAndReset.Execute(viewModel);

			});
	}


}

