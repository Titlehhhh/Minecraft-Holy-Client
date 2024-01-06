using Avalonia.Threading;
using HolyClient.Converters;
using HolyClient.StressTest;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace HolyClient.ViewModels;



public class StressTestProcessViewModel : ReactiveObject, IStressTestProcessViewModel, IActivatableViewModel
{
	[Reactive]
	public ICommand CancelCommand { get; private set; }

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



	public ObservableCollection<LogEventViewModel> Logs { get; private set; }

	#region Metrics
	public ObservableCollection<ISeries> BotsOnlineSeries { get; set; }
	public ObservableCollection<ISeries> CPSSeries { get; set; }

	public object BotsOnline_Sync { get; } = new object();
	public object CPS_Sync { get; } = new object();

	public Axis[] BotsAxis { get; set; }
	public Axis[] CPSAxis { get; set; }
	public Margin DrawMargin { get; set; }
	#endregion

	public string? UrlPathSegment => null;

	public IScreen HostScreen { get; private set; }

	public ViewModelActivator Activator { get; } = new();
	#endregion


	private readonly DateTimeAxis _botsOnlineAxis;
	private readonly DateTimeAxis _cpsAxis;
	private readonly Random _random = new();

	private readonly List<DateTimePoint> _botsOnlineValues = new();
	private readonly List<DateTimePoint> _cpsValues = new();

	public StressTestProcessViewModel(ICommand cancel, IStressTestProfile stressTest, LoggerWrapper wrapper)
	{


		Logs = wrapper.Events;
		Host = stressTest.Server;
		Version = MinecraftVersionToStringConverter.McVerToString(stressTest.Version);
		ParallelCount = stressTest.NumberOfBots.ToString();


		CancelCommand = cancel;



		BotsOnlineSeries = new ObservableCollection<ISeries>
		{
			new LineSeries<DateTimePoint>
			{
				Values = _botsOnlineValues,
				Fill = null,
				//Stroke = new SolidColorPaint(SKColor.Parse("837aff")),
				GeometryFill = null,
				GeometryStroke = null
			}
		};
		CPSSeries = new ObservableCollection<ISeries>
		{
			new LineSeries<DateTimePoint>
			{
				Values = _cpsValues,
				Fill = null,
				Stroke = new SolidColorPaint(SKColor.Parse("d97aff")),
				GeometryFill = null,
				GeometryStroke = null
			}
		};


		_botsOnlineAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
		{

			CustomSeparators = GetSeparators(),
			AnimationsSpeed = TimeSpan.FromMilliseconds(0),
			SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100)),

		};

		_cpsAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
		{

			CustomSeparators = GetSeparators(),
			AnimationsSpeed = TimeSpan.FromMilliseconds(0),
			SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100)),

		};



		BotsAxis = new Axis[] {

			_botsOnlineAxis

		};
		CPSAxis = new Axis[] {
			_cpsAxis


		};

		BotsAxis[0].SharedWith = CPSAxis;
		CPSAxis[0].SharedWith = BotsAxis;

		DrawMargin = new Margin(70, Margin.Auto, Margin.Auto, Margin.Auto);

		this.WhenActivated(async d =>
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
					var now = DateTime.Now;

					_botsOnlineValues.Add(new DateTimePoint(now, x.BotsOnline));
					if (_botsOnlineValues.Count > 100) _botsOnlineValues.RemoveAt(0);

					// we need to update the separators every time we add a new point 
					_botsOnlineAxis.CustomSeparators = GetSeparators();


					_cpsValues.Add(new DateTimePoint(now, x.CPS));
					if (_cpsValues.Count > 100) _cpsValues.RemoveAt(0);

					// we need to update the separators every time we add a new point 
					_cpsAxis.CustomSeparators = GetSeparators();

				}).DisposeWith(d);




		});

	}


	private double[] GetSeparators()
	{
		var now = DateTime.Now;

		return new double[]
	   {
			now.AddSeconds(-25).Ticks,
			now.AddSeconds(-20).Ticks,
			now.AddSeconds(-15).Ticks,
			now.AddSeconds(-10).Ticks,
			now.AddSeconds(-5).Ticks,
			now.Ticks
	   };
	}

	private static string Formatter(DateTime date)
	{
		var secsAgo = (DateTime.Now - date).TotalSeconds;

		return secsAgo < 1
			? "now"
			: $"{secsAgo:N0}s ago";
	}





}
