using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.AppState;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using McProtoNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public sealed class StressTestProfileViewModel : ReactiveValidationObject, IRoutableViewModel, IActivatableViewModel
{
	private static string GetTr(string key)
	{
		return $"StressTest.Profile.GeneralSettings.Validation.{key}";
	}

	public Guid Id { get; private set; }

	public ViewModelActivator Activator { get; } = new();

	public string? UrlPathSegment => throw new NotImplementedException();

	public IScreen HostScreen { get; private set; }

	[Reactive]
	public string Name { get; set; }



	private SelectImportSourceProxyViewModel _selectProxyImportSourceViewModel = new();

	private IStressTestProfile _state;

	public StressTestProfileViewModel(IStressTestProfile state)
	{

		_state = state;

		#region Bind to state
		this.Id = state.Id;
		this.Name = state.Name;
		this.Server = state.Server;
		this.Version = state.Version;
		this.BotsNickname = state.BotsNickname;
		this.NumberOfBots = state.NumberOfBots;
		this.UseProxy = state.UseProxy;

		this.WhenAnyValue(x => x.Name)
			.BindTo(state, x => x.Name);

		this.WhenAnyValue(x => x.Server)
			.BindTo(state, x => x.Server);


		this.WhenAnyValue(x => x.Version)
			.BindTo(state, x => x.Version);

		this.WhenAnyValue(x => x.BotsNickname)
			.BindTo(state, x => x.BotsNickname);

		this.WhenAnyValue(x => x.NumberOfBots)
			.BindTo(state, x => x.NumberOfBots);

		this.WhenAnyValue(x => x.UseProxy)
			.BindTo(state, x => x.UseProxy);
		#endregion

		//HostScreen = hostScreen;
		#region Configure validation


		this.WhenActivated(d =>
		{



			this.ValidationRule(
			   viewModel => viewModel.Server,
			   name => !string.IsNullOrWhiteSpace(name),
			   GetTr("Address")).DisposeWith(d);



			IObservable<IValidationState> botsNicknameValid =
				this.WhenAnyValue(x => x.BotsNickname)
					.Select(name =>
					{
						if (string.IsNullOrWhiteSpace(name))
						{
							return ValidationState.Valid;
						}
						if (name.Length >= 14)
						{
							return new ValidationState(false, GetTr("BotsNickname.Long"));
						}
						return ValidationState.Valid;
					});

			this.ValidationRule(vm => vm.BotsNickname, botsNicknameValid).DisposeWith(d);



			StartCommand = ReactiveCommand.CreateFromTask(this.StartStressTest, this.IsValid());
		});
		#endregion




		#region Configure proxies

		this.WhenActivated(d =>
		{
			state.Proxies.Connect()
				.ObserveOn(RxApp.MainThreadScheduler)
				.Transform(x => new ProxySourceViewModel(x))
				.Bind(out _proxies)
				.DisposeMany()
				.Subscribe()
				.DisposeWith(d);
			this.RaisePropertyChanged(nameof(Proxies));
		});

		this.WhenActivated(d =>
		{


			AddSourceProxyCommand = ReactiveCommand.CreateFromTask(async () =>
			{

				bool ok = await SelectProxyImportSourceDialog.Handle(_selectProxyImportSourceViewModel);
				if (ok)
				{
					ImportSource source = _selectProxyImportSourceViewModel.SelectedSource.SourceType;

					IProxySource proxySource = null;

					ImportProxyViewModel importVm = null;



					if (source == ImportSource.InMemory)
					{
						InMemoryImportProxyDialogViewModel vm = new("ManualEntry");

						ok = await ImportProxyDialog.Handle(vm);

						proxySource = new InMemoryProxySource(vm.Type, vm.Lines);
						importVm = vm;

					}
					else if (source == ImportSource.File)
					{
						FileImportProxyDialogViewModel vm = new("File");

						ok = await ImportProxyDialog.Handle(vm);

						proxySource = new FileProxySource(vm.Type, vm.FilePath);
						importVm = vm;
					}
					else if (source == ImportSource.Url)
					{
						UrlImportProxyDialogViewModel vm = new("Url");

						ok = await ImportProxyDialog.Handle(vm);

						proxySource = new UrlProxySource(vm.Type, vm.URL);
						importVm = vm;
					}

					if (importVm.IsValid())
					{
						if (proxySource is not null)
						{
							state.Proxies.AddOrUpdate(proxySource);
						}
					}
				}

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
				if (SelectedProxy is { })
				{
					state.Proxies.Remove(SelectedProxy.Id);
				}



			}, canExecuteDeleteAll).DisposeWith(d);

		});



		#endregion

		#region Configure plugins



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


	private async Task StartStressTest()
	{


		var rootScreen = Locator.Current.GetService<IScreen>("Root");

		LoggerWrapper loggerWrapper = new LoggerWrapper();

		Serilog.ILogger logger = loggerWrapper;

		try
		{
			var cancelCommand = ReactiveCommand.Create(() =>
			{
				var mainVM = Locator.Current.GetService<MainViewModel>();

				rootScreen.Router.Navigate.Execute(mainVM);

			});



			StressTestProcessViewModel proccess = new StressTestProcessViewModel(cancelCommand, _state, loggerWrapper);
			await rootScreen.Router.Navigate.Execute(proccess);



			await _state.Start(logger);
			

		}
		catch (TaskCanceledException)
		{
			logger.Information("[STRESS TEST] Завершился из-за отмены");
		}
		catch (Exception ex)
		{
			logger.Error(ex, "[STRESS TEST] завершился с ошибкой");
		}
		finally
		{

		}
	}




	#region General Settings
	[Reactive]
	public string Server { get; set; }
	[Reactive]
	public string BotsNickname { get; set; }
	[Reactive]
	public int NumberOfBots { get; set; }

	[Reactive]
	public bool UseProxy { get; set; }

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


	public Interaction<SelectImportSourceProxyViewModel, bool> SelectProxyImportSourceDialog { get; } = new();
	public Interaction<ImportProxyViewModel, bool> ImportProxyDialog { get; } = new();
	public Interaction<Unit, Unit> ExportProxyDialog { get; } = new();
	public Interaction<Unit, bool> ConfirmDeleteProxyDialog { get; } = new();

	[Reactive]
	public ICommand AddSourceProxyCommand { get; private set; }
	[Reactive]
	public ICommand ExportProxyCommand { get; private set; }
	[Reactive]
	public ICommand DeleteProxyCommand { get; private set; }
	[Reactive]
	public ICommand DeleteAllProxyCommand { get; private set; }

	[Reactive]
	public ICommand StartCommand { get; private set; }

	[Reactive]
	public ProxySourceViewModel? SelectedProxy { get; set; }

	public ReadOnlyObservableCollection<ProxySourceViewModel> _proxies;

	public ReadOnlyObservableCollection<ProxySourceViewModel> Proxies => _proxies;

	#endregion
}
