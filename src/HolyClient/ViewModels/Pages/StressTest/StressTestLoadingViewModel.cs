using HolyClient.Commands;
using HolyClient.StressTest;
using ReactiveUI;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public class StressTestLoadingViewModel : ReactiveObject, IRoutableViewModel
{
	public string? UrlPathSegment => null;

	public IScreen HostScreen { get; }
	public ICommand CancelCommand { get; }
	public StressTestLoadingViewModel(IScreen hostScreen, IStressTest stressTest)
	{
		HostScreen = hostScreen;
		CancelCommand = new StopStressTestCommand(hostScreen, stressTest);
	}
}
