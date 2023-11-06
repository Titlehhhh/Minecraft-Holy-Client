using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Commands;
using HolyClient.Common;
using HolyClient.Core.Infrastructure;
using HolyClient.Localization;
using HolyClient.StressTest;
using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using McProtoNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;
using Splat;
using Stateless.Graph;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;

namespace HolyClient.ViewModels;
public class StressTestConfigurationViewModel : ReactiveValidationObject, IRoutableViewModel, IActivatableViewModel
{

	private static string GetTr(string key)
	{
		return $"StressTest.Configuration.GeneralSettings.Validation.{key}";
	}

	public string? UrlPathSegment => throw new NotImplementedException();
	public IScreen HostScreen { get; }
	public ViewModelActivator Activator { get; } = new();


	public ICommand StartCommand { get; }


	#region General Settings
	[Reactive]
	public string Server { get; set; }
	[Reactive]
	public string BotsNickname { get; set; }
	[Reactive]
	public int NumberOfBots { get; set; }

	public MinecraftVersion[] SupportedVersions { get; } = Enum.GetValues<MinecraftVersion>();

	[Reactive]
	public MinecraftVersion Version { get; set; } = MinecraftVersion.MC_1_16_5_Version;

	#endregion

	#region Behavior


	[Reactive]
	public StressTestPluginViewModel? SelectedBehavior { get; set; }

	[Reactive]
	public ReadOnlyObservableCollection<StressTestPluginViewModel> AvailableBehaviors { get; private set; }


	#endregion

	[Reactive]
	public IStressTestBehavior? CurrentBehavior { get; private set; }

	[Reactive]
	public IPluginSource? InstalledBehavior { get; private set; }
	#region Proxy

	public ReadOnlyObservableCollection<ProxyInfo> _proxies;

	public ReadOnlyObservableCollection<ProxyInfo> Proxies => _proxies;
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

	public StressTestConfigurationViewModel(IScreen hostScreen, IStressTest state)
	{


		#region Bind to state
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
		#endregion


		#region Configure validation


		this.ValidationRule(
		   viewModel => viewModel.Server,
		   name => !string.IsNullOrWhiteSpace(name),
		   GetTr("Address"));



		IObservable<IValidationState> botsNickaneValid =
			this.WhenAnyValue(x => x.BotsNickname)
				.Select(name => string.IsNullOrEmpty(name)
					? new ValidationState(false, GetTr("BotsNickname"))

					: (name.Length <= 14 
							? ValidationState.Valid : 
							new ValidationState(false,GetTr("BotsNickname.Long"))));

		this.ValidationRule(vm => vm.BotsNickname, botsNickaneValid);


		#endregion

		HostScreen = hostScreen;




		StartCommand = new StartStressTestCommand(hostScreen, state, this.IsValid());

		#region Configure proxies

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

		});



		#endregion

		#region Configure plugins

		Console.WriteLine("CurrBeh: " + state.Behavior);

		this.CurrentBehavior = state.Behavior;

		state.WhenAnyValue(x => x.Behavior)
			.BindTo(this, x => x.CurrentBehavior);




		var pluginProvider = Locator.Current.GetService<IPluginProvider>();

		pluginProvider.AvailableStressTestPlugins
			.Connect()
			.Transform(x => new StressTestPluginViewModel(x, state))
			.Bind(out var plugins)
			.DisposeMany()
			.Subscribe();

		AvailableBehaviors = plugins;


		SelectedBehavior = plugins.FirstOrDefault();




		#endregion
	}




}
