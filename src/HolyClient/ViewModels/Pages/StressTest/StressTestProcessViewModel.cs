using Avalonia.Threading;
using HolyClient.Commands;
using HolyClient.Converters;
using HolyClient.StressTest;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
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
	public string NumberOfBots { get; private set; }
	#endregion

	#region Properties

	[Reactive]
	public IEnumerable<ISeries> CPS_Series { get; private set; } = Enumerable.Empty<ISeries>();
	[Reactive]
	public IEnumerable<ISeries> BotsOnline_Series { get; private set; } = Enumerable.Empty<ISeries>();
	[Reactive]
	public IEnumerable<ISeries> Proxy_Series { get; private set; } = Enumerable.Empty<ISeries>();

	[Reactive]
	public ICommand CancelCommand { get; private set; }

	public LabelVisual ProxyTitle { get; set; } =
	   new LabelVisual
	   {
		   Text = "Прокси",
		   TextSize = 16,
		   Padding = new LiveChartsCore.Drawing.Padding(15),
		   Paint = new SolidColorPaint(SKColors.White)
	   };
	public SolidColorPaint LegengTextPaint { get; } = new SolidColorPaint
	{
		Color = SKColors.White
	};


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

	#region Fields

	private readonly ObservableCollection<TimeSpanPoint> _cpsPoints = new();
	private readonly ObservableCollection<TimeSpanPoint> _botsOnlinePoints = new();
	private readonly int maxPoints = 15;
	private TimeSpan index = TimeSpan.Zero;
	#endregion

	public object CPS_Sync { get; } = new();
	public object Online_Sync { get; } = new();

	private DateTime? date = null;
	public StressTestProcessViewModel(IScreen hostScreen, IStressTest stressTest)
	{

		Host = stressTest.Server;
		Console.WriteLine("Server: " +stressTest.Server);
		Version = MinecraftVersionToStringConverter.McVerToString(stressTest.Version);
		NumberOfBots = stressTest.NumberOfBots.ToString();


		CancelCommand = new StopStressTestCommand(hostScreen, stressTest);

		OnActivate(hostScreen);


		this.WhenActivated(d =>
		{


			StressTestMetrik currentData = new();
			stressTest.Metrics.Subscribe(x =>
			{
				currentData = x;
			}).DisposeWith(d);


			DispatcherTimer.Run(() =>
			{
				try
				{
					var metrik = currentData;


					if (date is null)
					{
						date = DateTime.UtcNow;
					}
					TimeSpan delta = DateTime.UtcNow - date.Value;



					_cpsPoints.Add(new TimeSpanPoint(delta, metrik.CPS));
					if (_cpsPoints.Count >= maxPoints)
					{
						_cpsPoints.RemoveAt(0);
					}

					_botsOnlinePoints.Add(new TimeSpanPoint(delta, metrik.BotsOnline));
					if (_botsOnlinePoints.Count >= maxPoints)
					{
						_botsOnlinePoints.RemoveAt(0);
					}


				}
				catch (Exception ex)
				{
					Console.WriteLine("Exx " + ex);
				}
				finally
				{

				}
				return true;
			}, TimeSpan.FromSeconds(1)).DisposeWith(d);

		});

	}

	private void OnActivate(IScreen hostScreen)
	{

		{
			var from = SKColor.Parse("d51250");
			var to = SKColor.Parse("12d549");
			var start = new SKPoint(0.5f, 1);

			var end = new SKPoint(0.5f, 0);
			CPS_Series = new ISeries[]
			{
			new LineSeries<TimeSpanPoint>
			{
				Values = _cpsPoints,
				Fill = null,
				GeometrySize = 9,
				Stroke = new LinearGradientPaint(new[]{ from, to }, start,end) { StrokeThickness = 2 },
				GeometryStroke = new LinearGradientPaint(new[]{ from,to}, start,end) { StrokeThickness = 2 },
				Name ="Подключения в секунду"
			}
			};
		}
		{

			var from = SKColor.Parse("00a8f3");
			var to = SKColor.Parse("9415d9");
			var start = new SKPoint(0, 0.5F);

			var end = new SKPoint(1, 0.5F);

			BotsOnline_Series = new ISeries[]
			{
				new LineSeries<TimeSpanPoint>
				{
					Values = _botsOnlinePoints,
					GeometrySize = 9,
					Stroke = new LinearGradientPaint(new[]{ from, to }, start,end) { StrokeThickness = 2 },
					GeometryStroke = new LinearGradientPaint(new[]{ from,to}, start,end) { StrokeThickness = 2 },
					Fill = null,
					Name ="Количество ботов"
				}
			};
		}

		var proxy_series = new List<PieSeries<int>>();
		foreach (var pr in Enum.GetValues<ProxyType>())
		{


			//proxy_series.Add(new PieSeries<int>()
			//{

			//	//Values = new int[] { r.Next(1000, 5000) },
			//	//Name = pr.ToString(),
			//	//DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer,


			//}); ;
		}

		Proxy_Series = proxy_series;


		HostScreen = hostScreen;

	}






}
