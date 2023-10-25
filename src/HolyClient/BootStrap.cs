using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using HolyClient.AppState;
using HolyClient.Contracts.Models;
using HolyClient.Contracts.Services;
using HolyClient.Localization;
using HolyClient.Models;
using HolyClient.Models.ManagingExtensions;
using HolyClient.Services;
using HolyClient.StressTest;
using HolyClient.ViewModels;
using HolyClient.Views;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;

namespace HolyClient
{
	public static class BootStrap
	{
		public static async Task Run(IObserver<string> progress)
		{


			await Task.Run(async () =>
		   {
			   Subject<Unit> startApp = new();

			   RxApp.SuspensionHost.IsLaunchingNew = startApp;



			   //var driver = Locator.Current.GetService<ISuspensionDriver>();
			   RxApp.SuspensionHost.CreateNewAppState = () => new MainState();
			   RxApp.SuspensionHost.SetupDefaultSuspendResume();

			   progress.OnNext("Загрузка состояния");

			   startApp.OnNext(Unit.Default);

			   var state = RxApp.SuspensionHost.GetAppState<MainState>();

			   Loc.Instance.CurrentLanguage = state.SettingsState.Language;

			   progress.OnNext("Загрузка плагинов");




			   ExtensionManager extensionManager = new ExtensionManager(state.ExtensionManagerState);

			   await extensionManager.Initialization();



			   Locator.CurrentMutable.RegisterConstant<ExtensionManager>(extensionManager);

			   Locator.CurrentMutable.RegisterConstant<IPluginProvider>(new PluginProvider());


			   await state.BotManagerState.Initialization();

			   progress.OnNext("Почти готово");


			   RegisterAppServices();
			   RegisterStates(state);
			   RegisterPages();

			   RegisterViewModels();

			   progress.OnNext("Готово");



			   await Dispatcher.UIThread.InvokeAsync(() =>
			   {


				   INotificationManager notificationManager =
					new WindowNotificationManager(TopLevel.GetTopLevel(Locator.Current.GetService<MainView>()))
					{
						Position = NotificationPosition.BottomRight
					};


				   Locator.CurrentMutable.RegisterConstant<INotificationManager>(notificationManager);

				   extensionManager.AssemblyManager.AssemblyFileUpdated.ObserveOn(RxApp.MainThreadScheduler).Subscribe(x =>
				   {


					   notificationManager.Show(new Avalonia.Controls.Notifications.Notification(
						   "Менеджер сборок",
						   $"Сборка {x.NameWithExtension} была обновлена"));



				   });
			   });

			   progress.OnCompleted();

		   });
		}

		private static void RegisterViewModels()
		{

			Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());


		}

		private static void RegisterStates(MainState state)
		{
			Locator.CurrentMutable.RegisterConstant<MainState>(state);
			Locator.CurrentMutable.RegisterConstant<IBotManager>(state.BotManagerState);
			Locator.CurrentMutable.RegisterConstant<SettingsState>(state.SettingsState);
			Locator.CurrentMutable.RegisterConstant<IStressTest>(state.StressTestState);
		}

		private static void RegisterPages()
		{

			Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new HomeViewModel(), nameof(Page.Home));
			Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new BotManagerViewModel(), nameof(Page.BotManager));
			Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new SettingsViewModel(), nameof(Page.Settings));
			Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new StressTestViewModel(), nameof(Page.StressTest));
			Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new ManagingExtensionsViewModel(), nameof(Page.ManagingExtensions));
		}



		public static void RegisterAppServices()
		{


			Locator.CurrentMutable.RegisterConstant<INugetClient>(new NugetClient());



			Locator.CurrentMutable.RegisterConstant<IProxyLoaderService>(new ProxyLoaderService());


		}

	}
}
