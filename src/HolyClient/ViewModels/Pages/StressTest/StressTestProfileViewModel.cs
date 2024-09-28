using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using HolyClient.StressTest;
using HolyClient.ViewModels.Pages.StressTest.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using ReactiveUI.Validation.States;
using Splat;
using ILogger = Serilog.ILogger;

namespace HolyClient.ViewModels;

public sealed class StressTestProfileViewModel : ReactiveValidationObject, IRoutableViewModel, IActivatableViewModel
{
    private readonly SelectImportSourceProxyViewModel _selectProxyImportSourceViewModel = new();

    private readonly IStressTestProfile _state;

    public StressTestProfileViewModel(IStressTestProfile state)
    {
        _state = state;

        #region Bind to state

        state.PropertyChanged += (s, e) => { _ = App.SaveState(); };
        Id = state.Id;
        Name = state.Name;
        Server = state.Server;
        Version = SupportedVersions.FirstOrDefault(x => x.ProtocolVersion == state.Version) ??
                  SupportedVersions.First();

        BotsNickname = state.BotsNickname;
        NumberOfBots = state.NumberOfBots;
        UseProxy = state.UseProxy;
        CheckDNS = state.OptimizeDNS;

        this.WhenAnyValue(x => x.Name)
            .BindTo(state, x => x.Name);

        this.WhenAnyValue(x => x.CheckDNS)
            .BindTo(state, x => x.OptimizeDNS);

        this.WhenAnyValue(x => x.Server)
            .BindTo(state, x => x.Server);


        this.WhenAnyValue(x => x.Version)
            .Subscribe(x => { state.Version = x.ProtocolVersion; });


        this.WhenAnyValue(x => x.BotsNickname)
            .BindTo(state, x => x.BotsNickname);

        this.WhenAnyValue(x => x.NumberOfBots)
            .Subscribe(x =>
            {
                try
                {
                    state.NumberOfBots = Convert.ToInt32(x);
                }
                catch
                {
                }
            });

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


            var botsNicknameValid =
                this.WhenAnyValue(x => x.BotsNickname)
                    .Select(name =>
                    {
                        if (string.IsNullOrWhiteSpace(name)) return ValidationState.Valid;
                        if (name.Length >= 14) return new ValidationState(false, GetTr("BotsNickname.Long"));
                        return ValidationState.Valid;
                    });

            this.ValidationRule(vm => vm.BotsNickname, botsNicknameValid).DisposeWith(d);


            StartCommand = ReactiveCommand.CreateFromTask(StartStressTest, this.IsValid());
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
                var ok = await SelectProxyImportSourceDialog.Handle(_selectProxyImportSourceViewModel);
                if (ok)
                {
                    var source = _selectProxyImportSourceViewModel.SelectedSource.SourceType;

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
                        if (proxySource is not null)
                            state.Proxies.AddOrUpdate(proxySource);
                }
            });


            var canExecuteDeleteAll = state.Proxies.CountChanged
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => x > 0);

            DeleteAllProxyCommand = ReactiveCommand.CreateFromTask(async () =>
                {
                    if (await ConfirmDeleteProxyDialog.Handle(default)) state.Proxies.Clear();
                }, canExecuteDeleteAll)
                .DisposeWith(d);

            DeleteProxyCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!await ConfirmDeleteProxyDialog.Handle(Unit.Default))
                    return;
                if (SelectedProxy is not null) state.Proxies.Remove(SelectedProxy.Id);
            }, canExecuteDeleteAll).DisposeWith(d);
        });

        #endregion

        #region Configure plugins

        CurrentBehavior = state.Behavior;

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

    public Guid Id { get; private set; }

    [Reactive] public string Name { get; set; }

    [Reactive] public IStressTestBehavior? CurrentBehavior { get; set; }

    [Reactive] public IPluginSource? InstalledBehavior { get; set; }

    public ViewModelActivator Activator { get; } = new();

    public string? UrlPathSegment => "Profile";

    public IScreen HostScreen { get; }

    private static string GetTr(string key)
    {
        return $"StressTest.Profile.GeneralSettings.Validation.{key}";
    }


    private async Task StartStressTest()
    {
        await App.SaveState();
        var rootScreen = Locator.Current.GetService<IScreen>("Root");

        var loggerWrapper = new LoggerWrapper();

        ILogger logger = loggerWrapper;

        try
        {
            var cancelCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var mainVM = Locator.Current.GetService<MainViewModel>();

                await rootScreen.Router.Navigate.Execute(mainVM);
                await _state.Stop();
            });


            var proccess = new StressTestProcessViewModel(cancelCommand, _state, loggerWrapper);
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
    }


    #region General Settings

    [Reactive] public string Server { get; set; }

    [Reactive] public string BotsNickname { get; set; }

    [Reactive] public decimal? NumberOfBots { get; set; }

    [Reactive] public bool UseProxy { get; set; }

    [Reactive] public bool CheckDNS { get; set; }

    public MinecraftVersionVM[] SupportedVersions { get; } = MinecraftVersionVM.GetAll();

    [Reactive] public MinecraftVersionVM Version { get; set; }

    #endregion

    #region Behavior

    [Reactive] public StressTestPluginViewModel? SelectedBehavior { get; set; }

    [Reactive] public ReadOnlyObservableCollection<StressTestPluginViewModel> AvailableBehaviors { get; private set; }

    #endregion

    #region Proxy

    public Interaction<SelectImportSourceProxyViewModel, bool> SelectProxyImportSourceDialog { get; } = new();
    public Interaction<ImportProxyViewModel, bool> ImportProxyDialog { get; } = new();
    public Interaction<Unit, Unit> ExportProxyDialog { get; } = new();
    public Interaction<Unit, bool> ConfirmDeleteProxyDialog { get; } = new();

    [Reactive] public ICommand AddSourceProxyCommand { get; private set; }

    [Reactive] public ICommand ExportProxyCommand { get; set; }

    [Reactive] public ICommand DeleteProxyCommand { get; private set; }

    [Reactive] public ICommand DeleteAllProxyCommand { get; private set; }

    [Reactive] public ICommand StartCommand { get; private set; }

    [Reactive] public ProxySourceViewModel? SelectedProxy { get; set; }

    public ReadOnlyObservableCollection<ProxySourceViewModel> _proxies;

    public ReadOnlyObservableCollection<ProxySourceViewModel> Proxies => _proxies;

    #endregion
}