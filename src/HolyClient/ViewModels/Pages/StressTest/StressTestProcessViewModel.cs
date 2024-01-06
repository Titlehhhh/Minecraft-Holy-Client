using HolyClient.Converters;
using HolyClient.StressTest;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;


namespace HolyClient.ViewModels;



public class StressTestProcessViewModel : ReactiveObject, IStressTestProcessViewModel, IActivatableViewModel
{
	#region Info Panel
	[Reactive]
	public string Host { get; private set; }
	[Reactive]
	public string Version { get; private set; }
	[Reactive]
	public string ParallelCount { get; private set; }
	#endregion

	#region Properties
	[Reactive]
	public int BotsOnline { get; private set; }
	[Reactive]
	public int CPS { get; private set; }

	[Reactive]
	public int PeakCPS { get; private set; }
	[Reactive]
	public string ProxyQuality { get; private set; }


	[Reactive]
	public IEnumerable<ISeries> Proxy_Series { get; private set; } = Enumerable.Empty<ISeries>();

	[Reactive]
	public ICommand CancelCommand { get; private set; }

	public ObservableCollection<LogEventViewModel> Logs { get; private set; }


	public Axis[] XAxes { get; set; } =
	{

		new Axis
		{
			Labeler = value => ToReadableString( TimeSpan.FromTicks((long)value)),


			UnitWidth = TimeSpan.FromSeconds(1).Ticks,

			MinStep = TimeSpan.FromSeconds(1).Ticks,
		}
	};
	private static string ToReadableString(TimeSpan span)
	{


		return span.ToString(@"mm\:ss");
	}
	public string? UrlPathSegment => null;

	public IScreen HostScreen { get; private set; }

	public ViewModelActivator Activator { get; } = new();
	#endregion



	public StressTestProcessViewModel(IScreen hostScreen, IStressTest stressTest, LoggerWrapper wrapper)
	{

		Logs = wrapper.Events;
		Host = stressTest.Server;
		Version = MinecraftVersionToStringConverter.McVerToString(stressTest.Version);
		ParallelCount = stressTest.NumberOfBots.ToString();


		//CancelCommand = new StopStressTestCommand(hostScreen, stressTest);



		this.WhenActivated(d =>
		{


			StressTestMetrik currentData = new();
			stressTest.Metrics
				.SubscribeOn(RxApp.MainThreadScheduler)
				.Subscribe(x =>
				{
					BotsOnline = x.BotsOnline;
					CPS = x.CPS;
					PeakCPS = Math.Max(CPS, PeakCPS);

					//ProxyQuality = Random.Shared.Next(0, 70) + "%";
				}).DisposeWith(d);




		});

	}





}
