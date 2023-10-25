using Avalonia;
using Avalonia.ReactiveUI;
using HolyClient.AppState;
using HolyClient.Services;
using ReactiveUI;
using Splat;
using System;

namespace HolyClient.Desktop
{
	internal class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{

			BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);
		}

		public static AppBuilder BuildAvaloniaApp()
		{
			return AppBuilder.Configure<App>()
				   .UsePlatformDetect()
				   .LogToTrace()

				   .AfterSetup(x =>
				   {
					   var lifetime = App.Current.ApplicationLifetime;
					   var helper = new AutoSuspendHelper(lifetime);

					   Locator.CurrentMutable.RegisterAnd<ISuspensionDriver>(() => new DefaultSuspensionDriver<MainState>());

				   })
				   .UseReactiveUI();



		}

	}

}