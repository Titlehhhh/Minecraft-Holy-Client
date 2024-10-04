using System;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using HolyClient.AppState;
using HolyClient.DiscordRpc;
using HolyClient.Localization;
using HolyClient.ViewModels;
using HolyClient.Views;
using ReactiveUI;
using Splat;

[assembly: XmlnsDefinition("https://github.com/avaloniaui", "HolyClient.Assets.Fonts.Roboto")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "HolyClient.Localization")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "HolyClient.CustomControls")]

namespace HolyClient;

public class App : Application
{
    public static string AppName = "HolyClient";

    public static Window MainWindow { get; }

    public static async Task SaveState()
    {
        var driver = Locator.Current.GetService<ISuspensionDriver>();
        await driver.SaveState(Locator.Current.GetService<MainState>());
    }

    public override void Initialize()
    {
        GC.KeepAlive(typeof(Tr).Assembly);
        Languages.Init();
        AvaloniaXamlLoader.Load(this);
    }


    public override void OnFrameworkInitializationCompleted()
    {
        ThreadPool.GetMinThreads(out var min, out var cpt);

        ThreadPool.SetMinThreads(1, cpt);

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            File.WriteAllText("error.txt", e.ExceptionObject.ToString());
        };


        RootViewModel root = new();
        Locator.CurrentMutable.RegisterConstant<IScreen>(root, "Root");
        var service = new DiscordRpcService();
        Locator.CurrentMutable.RegisterConstant<DiscordRpcService>(service);
        service.Start();
        Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
        MainView main = new MainView();
        Locator.CurrentMutable.RegisterConstant<IViewFor<MainViewModel>>(main);
        

        try
        {
            RootView rootView = new()
            {
                DataContext = root
            };

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Exit += (sender, args) => { Locator.Current.GetService<DiscordRpcService>().Stop(); };
                var wnd = new MainWindow
                {
                    Content = rootView
                };

                desktop.MainWindow = wnd;
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime single)
            {
                //	single.MainView = view;
            }

            Current.RequestedThemeVariant = ThemeVariant.Dark;
        }
        catch (Exception e)
        {
            if (ApplicationLifetime is ISingleViewApplicationLifetime single)
                single.MainView = new TextBlock
                {
                    Text = $"Err: {e}",
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
        }
        finally
        {
            base.OnFrameworkInitializationCompleted();
        }
    }
}