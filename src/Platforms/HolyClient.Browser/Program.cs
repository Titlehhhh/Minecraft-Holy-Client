using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using HolyClient;
using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading.Tasks;

[assembly: SupportedOSPlatform("browser")]

namespace HolyClient.Browser;

internal partial class Program
{
	private static async Task Main(string[] args)
	{
		Subject<IDisposable> onStop = new();

		Interop.OnStop = onStop;

		await BuildAvaloniaApp()
			.WithInterFont()
			.AfterSetup(x =>
			{
				RxApp.SuspensionHost.ShouldPersistState = onStop;
				Locator.CurrentMutable.RegisterAnd<ISuspensionDriver>(() => new BrowserSuspensionDriver());

			})
			.UseReactiveUI()
			.UseSkia()
			.StartBrowserAppAsync("out");
	}

	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>();
}

