using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Avalonia.Styling;
using HolyClient.AppState;
using HolyClient.Localization;
using HolyClient.ViewModels;
using HolyClient.Views;
using Splat;
using System;

[assembly: XmlnsDefinition("https://github.com/avaloniaui", "HolyClient.Assets.Fonts.Roboto")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "HolyClient.Localization")]
[assembly: XmlnsDefinition("https://github.com/avaloniaui", "HolyClient.CustomControls")]
namespace HolyClient
{

	public partial class App : Application
	{

		public static string AppName = "HolyClient";

		public static Window MainWindow { get; private set; }
		public override void Initialize()
		{
			GC.KeepAlive(typeof(HolyClient.Localization.Tr).Assembly);
			Languages.Init();
			AvaloniaXamlLoader.Load(this);
		}


		public override void OnFrameworkInitializationCompleted()
		{



			MainViewModel mainViewModel = new();
			Locator.CurrentMutable.RegisterConstant<MainViewModel>(mainViewModel);
			try
			{
				MainView view = new MainView
				{
					DataContext = mainViewModel
				};
				Locator.CurrentMutable.RegisterConstant(view);
				if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
				{
					var wnd = new MainWindow()
					{
						Content = view
					};
					wnd.Opened += (s, e) =>
					{
						var state = Locator.Current.GetService<MainState>();
						mainViewModel.OnLoadState(state);
					};

					desktop.MainWindow = wnd;
				}
				else if (ApplicationLifetime is ISingleViewApplicationLifetime single)
				{
					//	single.MainView = view;
				}

				Application.Current.RequestedThemeVariant = ThemeVariant.Dark;



			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				if (ApplicationLifetime is ISingleViewApplicationLifetime single)
				{
					single.MainView = new TextBlock()
					{
						Text = $"Err: {e}",
						TextWrapping = Avalonia.Media.TextWrapping.WrapWithOverflow
					};
				}
			}
			finally
			{
				base.OnFrameworkInitializationCompleted();
			}
		}
	}
}
