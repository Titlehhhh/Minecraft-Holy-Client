using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using HolyClient.AppState;
using HolyClient.Services;
using ReactiveUI;
using Splat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading;

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