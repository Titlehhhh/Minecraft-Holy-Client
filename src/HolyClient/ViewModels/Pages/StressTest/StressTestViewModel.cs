using HolyClient.StressTest;
using ReactiveUI;
using Splat;

namespace HolyClient.ViewModels;


public class StressTestViewModel : ReactiveObject, IRoutableViewModel, IScreen
{


	public string? UrlPathSegment => null;

	public IScreen HostScreen { get; }

	public StressTestViewModel()
	{
		var state = Locator.Current.GetService<IStressTest>();




		HostScreen = Locator.Current.GetService<IScreen>("Main");
		var config = new StressTestConfigurationViewModel(this, state);
		Router.NavigateAndReset.Execute(config);
	}

	public RoutingState Router { get; } = new();
}

