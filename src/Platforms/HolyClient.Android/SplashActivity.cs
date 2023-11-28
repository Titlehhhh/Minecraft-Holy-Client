using Android.App;
using Android.Content;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using HolyClient.AppState;
using HolyClient.Services;
using Java.Interop;
using ReactiveUI;
using Splat;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Application = Android.App.Application;

namespace HolyClient.Android
{
	internal static class Internal
	{
		public static readonly Subject<IDisposable> OnStop = new();



		public static void SaveSettings()
		{
			using (var manual = new ManualResetEvent(false))
			{
				OnStop.OnNext(Disposable.Create(() => manual.Set()));
				manual.WaitOne();
			}
		}
	}

	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AvaloniaSplashActivity<App>
	{
		protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
		{
			return base.CustomizeAppBuilder(builder)
				.WithInterFont()
				.AfterSetup(x =>
				{
					

					RxApp.SuspensionHost.ShouldPersistState = Internal.OnStop;
					Locator.CurrentMutable.RegisterAnd<ISuspensionDriver>(()=> new DefaultSuspensionDriver<MainState>());
					AppDomain.CurrentDomain.UnhandledException += (s, e) =>
					{
						Internal.SaveSettings();
					};
				})
				.UseReactiveUI();
		}

		protected override void OnCreate(Bundle? savedInstanceState)
		{			
			base.OnCreate(savedInstanceState);
		}

		protected override void OnResume()
		{			
			base.OnResume();

			StartActivity(new Intent(Application.Context, typeof(MainActivity)));
		}

		
	}

}