﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using HolyClient.AppState;
using HolyClient.Contracts.Models;
using HolyClient.Core.Infrastructure;
using HolyClient.Localization;
using HolyClient.Models;
using HolyClient.Models.ManagingExtensions;
using HolyClient.Services;
using HolyClient.ViewModels;
using HolyClient.Views;
using ReactiveUI;
using Splat;
using INotifyPropertyChanged = PropertyModels.ComponentModel.INotifyPropertyChanged;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace HolyClient;

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


            var extensionManager = new ExtensionManager(state.ExtensionManagerState);

            await extensionManager.Initialization();


            Locator.CurrentMutable.RegisterConstant(extensionManager);

            Locator.CurrentMutable.RegisterConstant(extensionManager.AssemblyManager);

            Locator.CurrentMutable.RegisterConstant<IPluginProvider>(new PluginProvider());


            await state.StressTest.Initialization(Locator.Current.GetService<IPluginProvider>());

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


                Locator.CurrentMutable.RegisterConstant(notificationManager);

                List<IAssemblyFile> openFiles = new();

                extensionManager.AssemblyManager.AssemblyFileUpdated.ObserveOn(RxApp.MainThreadScheduler).Subscribe(x =>
                {
                    if (openFiles.Contains(x))
                        return;

                    openFiles.Add(x);

                    notificationManager.Show(new Notification(
                        Loc.Tr("ManagingExtension.AssemblyUpdatedNotification.Title"),
                        string.Format(Loc.Tr("ManagingExtension.AssemblyUpdatedNotification"), x.Name),
                        onClose: () => { openFiles.Remove(x); }));
                });
            });

            progress.OnCompleted();
        });

        var mainState = Locator.Current.GetService<MainState>();

        var mainViewModel = new MainViewModel(mainState);

        Locator.CurrentMutable.RegisterConstant<IScreen>(mainViewModel, "Main");
        Locator.CurrentMutable.RegisterConstant(mainViewModel);

        var root = Locator.Current.GetService<IScreen>("Root");

        await root.Router.NavigateAndReset.Execute(mainViewModel);
    }

    private static void RegisterViewModels()
    {
    }

    private static void RegisterStates(MainState state)
    {
        Locator.CurrentMutable.RegisterConstant(state);
        Locator.CurrentMutable.RegisterConstant(state.SettingsState);
        Locator.CurrentMutable.RegisterConstant(state.StressTest);
    }

    private static void RegisterPages()
    {
        Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new HomeViewModel(), nameof(Page.Home));
        Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new SettingsViewModel(), nameof(Page.Settings));
        Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new StressTestViewModel(), nameof(Page.StressTest));
        Locator.CurrentMutable.RegisterConstant<IRoutableViewModel>(new ManagingExtensionsViewModel(),
            nameof(Page.ManagingExtensions));
    }


    public static void RegisterAppServices()
    {
        Locator.CurrentMutable.RegisterConstant<INugetClient>(new NugetClient());


        Locator.CurrentMutable.RegisterConstant<IProxyLoaderService>(new ProxyLoaderService());
    }
}
