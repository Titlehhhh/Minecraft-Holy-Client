using DynamicData;
using HolyClient.Commands;
using HolyClient.Contracts.Models;
using HolyClient.Contracts.Services;
using HolyClient.LoadPlugins.Models;
using HolyClient.Models;
using McProtoNet.MultiVersion;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Windows.Input;

namespace HolyClient.ViewModels
{
	public class BotProfileViewModel : ReactiveObject, IBotProfileViewModel, IDisposable
	{
		#region Commands
		public StartStopBotCommand StartStopCommand { get; }

		public ICommand AddPluginCommand { get; }

		public ICommand RemovePluginCommand { get; }

		#endregion
		#region Properties

		public bool IsActivate { get; private set; }
		public ViewModelActivator Activator { get; } = new();
		public MinecraftVersion[] SupportedVersions { get; } = Enum.GetValues<MinecraftVersion>();
		public ConsoleViewModel Console { get; }
		public Guid Id => _profile.Id;
		public string Name
		{
			get => _profile.Name;
			set
			{
				_profile.Name = value;
				this.RaisePropertyChanged();
			}
		}

		public string Server
		{
			get => _profile.Server;
			set => _profile.Server = value;
		}

		public string Nickname
		{
			get => _profile.Nickname;
			set => _profile.Nickname = value;
		}

		public MinecraftVersion Version
		{
			get => _profile.Version;
			set => _profile.Version = value;
		}

		public int SelectedTab
		{
			get => _profile.SelectedTab;
			set => _profile.SelectedTab = value;
		}


		[Reactive]
		public ReadOnlyObservableCollection<BotPluginViewModel> AvailablePlugins { get; private set; }


		[Reactive]
		public ObservableCollection<BotPluginViewModel> LoadedPlugins { get; private set; } = new();

		[Reactive]
		public BotPluginViewModel? SelectedAvailablePlugin { get; set; }
		[Reactive]
		public BotPluginViewModel? SelectedLoadedPlugin { get; set; }
		#endregion

		private IBotProfile _profile;
		private IDisposable? _cleanUp;

		public BotProfileViewModel(IBotProfile profile)
		{
			try
			{
				CompositeDisposable d = new CompositeDisposable();

				_profile = profile;

				Name = _profile.Name;



				Console = new ConsoleViewModel();

				Serilog.ILogger logger = this.Console;


				this.StartStopCommand = new StartStopBotCommand(_profile, logger);

				Func<IBotPluginCreater, BotPluginViewModel> createVM = x => new BotPluginViewModel(x);



				LoadedPlugins = new(profile.PluignStore.Plugins.Select(createVM));

				Subject<Unit> filterAvailablePlugins = new();

				filterAvailablePlugins.DisposeWith(d);


				var pluginProvider = Locator.Current.GetService<IPluginProvider>();


				pluginProvider.AvailableBotPlugins
					.Connect()
					.ObserveOn(RxApp.MainThreadScheduler)
					.Filter(predicateChanged: filterAvailablePlugins.Select(x => this.BuildFilterAvailablePlugins()))
					.Transform(createVM)

					.Bind(out var plugins)
					.Subscribe()
					.DisposeWith(d);

				AvailablePlugins = plugins;

				filterAvailablePlugins.OnNext(default);


				var canExectuteAddPlugin = this
					.WhenAnyValue(x => x.SelectedAvailablePlugin)
					.Select(x => x is not null);

				AddPluginCommand = ReactiveCommand.Create(() =>
				{
					var pl = pluginProvider.AvailableBotPlugins.Lookup(this.SelectedAvailablePlugin.Token);

					if (pl.HasValue)
					{
						this._profile.PluignStore.AddPlugin(pl.Value);

						LoadedPlugins.Add(this.SelectedAvailablePlugin);
					}

					filterAvailablePlugins.OnNext(default);

				}, canExecute: canExectuteAddPlugin).DisposeWith(d);


				RemovePluginCommand = ReactiveCommand.Create(() =>
				{

					this._profile.PluignStore.RemovePlugin(this.SelectedLoadedPlugin.Token);

					LoadedPlugins.Remove(this.SelectedLoadedPlugin);

					filterAvailablePlugins.OnNext(default);

				}, canExecute: this.WhenAnyValue(x => x.SelectedLoadedPlugin).Select(x => x is not null));

				_cleanUp = d;
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex);
			}
		}

		private Func<IBotPluginCreater, bool> BuildFilterAvailablePlugins()
		{

			return FilterAvailablePlugins;
		}
		private bool FilterAvailablePlugins(IBotPluginCreater creater)
		{
			if (this._profile.PluignStore.Contains(creater.Token))
			{
				return false;
			}
			return true;
		}

		public void Dispose()
		{
			System.Console.WriteLine("Dispose Bot Profile View Model");
			Interlocked.Exchange(ref _cleanUp, null)?.Dispose();
		}
	}


	public class BotPluginViewModel : ReactiveObject
	{
		public BotPluginReference Token => _model.Token;

		public string Name => _model.Name;

		private IBotPluginCreater _model;

		public BotPluginViewModel(IBotPluginCreater model)
		{
			_model = model;
		}
	}
}
