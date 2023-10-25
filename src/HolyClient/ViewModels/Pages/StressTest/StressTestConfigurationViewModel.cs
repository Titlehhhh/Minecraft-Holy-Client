using DynamicData;
using HolyClient.Commands;
using HolyClient.Common;
using HolyClient.StressTest;

using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using McProtoNet.MultiVersion;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;

namespace HolyClient.ViewModels;
public class StressTestConfigurationViewModel : ReactiveObject, IRoutableViewModel, IActivatableViewModel
{
	[Reactive]
	public MinecraftVersion Version { get; set; } = MinecraftVersion.MC_1_16_5_Version;

	[Reactive]
	public string Server { get; set; }
	[Reactive]
	public string BotsNickname { get; set; }
	[Reactive]
	public int NumberOfBots { get; set; }

	[Reactive]
	public bool HasBehavior { get; set; }


	public ReadOnlyObservableCollection<ProxyInfo> _proxies;

	public ReadOnlyObservableCollection<ProxyInfo> Proxies => _proxies;

	public MinecraftVersion[] SupportedVersions { get; } = Enum.GetValues<MinecraftVersion>();

	public ICommand StartCommand { get; }


	[Reactive]
	public ICommand AddBehaviorCommand { get; private set; }
	[Reactive]
	public ICommand RemoveBehaviorCommand { get; private set; }

	public StressTestConfigurationViewModel(IScreen hostScreen, IStressTest state)
	{

		this.Server = state.Server;
		this.Version = state.Version;
		this.BotsNickname = state.BotsNickname;
		this.NumberOfBots = state.NumberOfBots;

		this.WhenAnyValue(x => x.Server)
			.BindTo(state, x => x.Server);

		this.WhenAnyValue(x => x.Version)
			.BindTo(state, x => x.Version);

		this.WhenAnyValue(x => x.BotsNickname)
			.BindTo(state, x => x.BotsNickname);

		this.WhenAnyValue(x => x.NumberOfBots)
			.BindTo(state, x => x.NumberOfBots);




		HostScreen = hostScreen;



		this.WhenActivated(d =>
		{




			state.Proxies.Connect()
				.ObserveOn(RxApp.MainThreadScheduler)
				.Transform(x => x)
				.Bind(out _proxies)
				.DisposeMany()
				.Subscribe()
				.DisposeWith(d);
			this.RaisePropertyChanged(nameof(Proxies));


		});


		StartCommand = new StartStressTestCommand(hostScreen, state);

		this.WhenActivated(d =>
		{


			ImportProxyCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				var vm = new ImportProxyViewModel(state);
				await ImportProxyDialog.Handle(vm);

			});


			var canExecuteDeleteAll = state.Proxies.CountChanged
				.ObserveOn(RxApp.MainThreadScheduler)
				.Select(x => x > 0);
			DeleteAllProxyCommand = ReactiveCommand.CreateFromTask(async () =>
			{
				if (await ConfirmDeleteProxyDialog.Handle(default))
				{
					state.Proxies.Clear();
				}
			}, canExecuteDeleteAll)
			.DisposeWith(d);




			DeleteProxyCommand = ReactiveCommand.CreateFromTask(async () =>
			{

				if (!await ConfirmDeleteProxyDialog.Handle(Unit.Default))
					return;
				if (SelectedProxies is { })
				{
					state.Proxies.RemoveMany(SelectedProxies.Items);
				}



			}, canExecuteDeleteAll).DisposeWith(d);




			AddBehaviorCommand = ReactiveCommand.Create(() =>
			{
				HasBehavior = true;
			}).DisposeWith(d);
			RemoveBehaviorCommand = ReactiveCommand.Create(() =>
			{
				HasBehavior = false;
			}).DisposeWith(d);


		});
	}


	#region Proxy


	public Interaction<ImportProxyViewModel, Unit> ImportProxyDialog { get; } = new();

	public Interaction<Unit, Unit> ExportProxyDialog { get; } = new();
	public Interaction<Unit, bool> ConfirmDeleteProxyDialog { get; } = new();

	[Reactive]
	public ICommand ImportProxyCommand { get; private set; }
	[Reactive]
	public ICommand ExportProxyCommand { get; private set; }
	[Reactive]
	public ICommand DeleteProxyCommand { get; private set; }
	[Reactive]
	public ICommand DeleteAllProxyCommand { get; private set; }



	[Reactive]
	public ProxyInfo? SelectedProxy { get; set; }

	[Reactive]
	public ISourceList<ProxyInfo> SelectedProxies { get; set; } = new SourceList<ProxyInfo>();

	#endregion


	public string? UrlPathSegment => throw new NotImplementedException();

	public IScreen HostScreen { get; }

	public ViewModelActivator Activator { get; } = new();
}



