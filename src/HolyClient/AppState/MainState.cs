using HolyClient.Contracts.Models;
using HolyClient.Core.StressTest;
using HolyClient.Models;
using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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
	[Key(2)]
	public IBotManager BotManagerState { get; set; } = new BotManager();


	[Reactive]
	[Key(3)]
	public IStressTest StressTestState { get; set; } = new StressTest();


	[Reactive]
	[Key(4)]
	public ExtensionManagerState ExtensionManagerState { get; set; } = new();
}

