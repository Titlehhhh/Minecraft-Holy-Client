using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FluentAvalonia.UI.Windowing;
using HolyClient.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace HolyClient.Views
{
	public sealed class ApplicationSplashScreen : IApplicationSplashScreen
	{
		private Window _owner;

		private SplashScreenViewModel SplashScreenViewModel = new();
		public ApplicationSplashScreen(Window owner)
		{
			_owner = owner;



			SplashScreenContent = new SplashContent
			{
				DataContext = SplashScreenViewModel
			};
		}

		public string AppName => "Holy client";

		public IImage AppIcon { get; } = new Bitmap(AssetLoader.Open(new Uri("avares://HolyClient/Assets/AppIcon.png")));

		public object SplashScreenContent { get; }

		public int MinimumShowTime => 2000;

		public async Task RunTasks(CancellationToken cancellationToken)
		{


			Subject<string> subject = new Subject<string>();

			CompositeDisposable disp = new();



			subject.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(x =>
				{
					SplashScreenViewModel.State = x;
					SplashScreenViewModel.Progress += 30;
				}, () =>
				{
					SplashScreenViewModel.Progress = 100;
				}).DisposeWith(disp);

			

			await BootStrap.Run(subject);

			disp.Dispose();
		}
	}
}
