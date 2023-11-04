using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using HolyClient.AppState;
using HolyClient.Contracts.Models;
using HolyClient.Contracts.Services;
using HolyClient.Core.Infrastructure;
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
using System.Collections.Generic;
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

			   progress.OnNext("Bootstrap.LoadingState.LoadState");

			   startApp.OnNext(Unit.Default);

			   var state = RxApp.SuspensionHost.GetAppState<MainState>();

			   Loc.Instance.CurrentLanguage = state.SettingsState.Language;

			   progress.OnNext("Bootstrap.LoadingState.LoadPlugins");




			   ExtensionManager extensionManager = new ExtensionManager(state.ExtensionManagerState);

			   await extensionManager.Initialization();



			   Locator.CurrentMutable.RegisterConstant<ExtensionManager>(extensionManager);

			   Locator.CurrentMutable.RegisterConstant<IAssemblyManager>(extensionManager.AssemblyManager);

			   Locator.CurrentMutable.RegisterConstant<IPluginProvider>(new PluginProvider());


			   await state.BotManagerState.Initialization();



			   await state.StressTestState.Initialization(Locator.Current.GetService<IPluginProvider>());

			   progress.OnNext("Bootstrap.LoadingState.AlmostDone");


			   RegisterAppServices();
			   RegisterStates(state);
			   RegisterPages();

			   RegisterViewModels();

			   progress.OnNext("Bootstrap.LoadingState.Complete");



			   await Dispatcher.UIThread.InvokeAsync(() =>
			   {


				   INotificationManager notificationManager =
					new WindowNotificationManager(TopLevel.GetTopLevel(Locator.Current.GetService<MainView>()))
					{
						Position = NotificationPosition.BottomRight
					};


				   Locator.CurrentMutable.RegisterConstant<INotificationManager>(notificationManager);

				   List<IAssemblyFile> openFiles = new();

				   extensionManager.AssemblyManager.AssemblyFileUpdated.ObserveOn(RxApp.MainThreadScheduler).Subscribe(x =>
				   {
					   if (openFiles.Contains(x))
						   return;

					   openFiles.Add(x);

					   notificationManager.Show(new Avalonia.Controls.Notifications.Notification(
						   Loc.Tr("ManagingExtension.AssemblyUpdatedNotification.Title"),
						   string.Format(Loc.Tr("ManagingExtension.AssemblyUpdatedNotification"), x.Name),
						   onClose: () =>
						   {
							   openFiles.Remove(x);
						   }));



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
