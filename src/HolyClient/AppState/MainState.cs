using HolyClient.Contracts.Models;
using HolyClient.Models;
using HolyClient.StressTest;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections;

namespace HolyClient.AppState;

[MessagePackObject()]
public class MainState : ReactiveObject
{
	[Reactive]
	[Key(0)]
	public SettingsState SettingsState { get; set; } = new();

	[Reactive]
	[Key(1)]
	public Page SelectedPage { get; set; }

	[Reactive]
	[Key(5)]
	public StressTestState StressTest { get; set; } = new();



	[Reactive]
	[Key(4)]
	public ExtensionManagerState ExtensionManagerState { get; set; } = new();
}
