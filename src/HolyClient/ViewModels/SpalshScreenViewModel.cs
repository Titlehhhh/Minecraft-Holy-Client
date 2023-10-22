using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.ViewModels;

public class SpalshScreenViewModel : ReactiveObject
{
	[Reactive]
	public double Progress { get; set; }
	[Reactive]
	public string State { get; set; }
}

