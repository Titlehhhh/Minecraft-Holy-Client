using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public class SplashScreenViewModel : ReactiveObject
{
	[Reactive]
	public double Progress { get; set; }
	[Reactive]
	public string State { get; set; }
}


