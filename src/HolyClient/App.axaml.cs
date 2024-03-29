using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Avalonia.Styling;
using HolyClient.AppState;
using HolyClient.Localization;
using HolyClient.Models;
using HolyClient.ViewModels;
using HolyClient.Views;
using ReactiveUI;
using Splat;
using System;
using System.Reflection;
using System.Threading;

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

			ThreadPool.GetMinThreads(out var min, out var cpt);

			ThreadPool.SetMinThreads(1, cpt);

			


			RootViewModel root = new();
			Locator.CurrentMutable.RegisterConstant<IScreen>(root, "Root");

			Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

			
			try
			{
				

				RootView rootView = new()
				{
					DataContext = root
				};

				if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
				{
					var wnd = new MainWindow()
					{
						Content = rootView
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
