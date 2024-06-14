using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using HolyClient.Converters;
using HolyClient.StressTest;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;

namespace HolyClient.ViewModels;

public class StressTestProcessViewModel : ReactiveObject, IStressTestProcessViewModel, IActivatableViewModel
{
    private readonly DateTimeAxis _botsOnlineAxis;

    private readonly List<DateTimePoint> _botsOnlineValues = new();
    private readonly DateTimeAxis _cpsAxis;
    private readonly List<DateTimePoint> _cpsValues = new();
    private readonly Random _random = new();

    private int colorId = 0;

    public StressTestProcessViewModel(ICommand cancel, IStressTestProfile stressTest, LoggerWrapper wrapper)
    {
        Logs = wrapper.Events;
        Host = stressTest.Server;
        Version = MinecraftVersionToStringConverter.McVerToString(stressTest.Version);
        ParallelCount = stressTest.NumberOfBots.ToString();


        CancelCommand = cancel;


        #region Confirgure charts

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
            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100))
        };

        _cpsAxis = new DateTimeAxis(TimeSpan.FromSeconds(1), Formatter)
        {
            CustomSeparators = GetSeparators(),
            AnimationsSpeed = TimeSpan.FromMilliseconds(0),
            SeparatorsPaint = new SolidColorPaint(SKColors.Black.WithAlpha(100))
        };


        BotsAxis = new Axis[]
        {
            _botsOnlineAxis
        };
        CPSAxis = new Axis[]
        {
            _cpsAxis
        };

        BotsAxis[0].SharedWith = CPSAxis;
        CPSAxis[0].SharedWith = BotsAxis;

        DrawMargin = new Margin(70, Margin.Auto, Margin.Auto, Margin.Auto);

        #endregion

        #region Configure exceptions

        #endregion

        this.WhenActivated(async d =>
        {
            SourceCache<ExceptionInfoViewModel, Tuple<string, string>> exceptions = new(x => x.Key);


            exceptions
                .Connect()
                .Sort(SortExpressionComparer<ExceptionInfoViewModel>.Descending(p => p.Count))
                .Bind(out var _exceptions)
                .Subscribe()
                .DisposeWith(d);

            Exceptions = _exceptions;

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

            Observable.Interval(TimeSpan.FromMilliseconds(1000), RxApp.TaskpoolScheduler)
                .Select(x =>
                {
                    return stressTest.ExceptionCounter.ToArray().Select(x =>
                        new ExceptionInfoViewModel(x.Key.Item1, x.Key.Item2, x.Value.Count));
                })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(data =>
                {
                    //Console.WriteLine("Second");
                    exceptions.AddOrUpdate(data);
                }).DisposeWith(d);
        });
    }


    #region Exceptions

    [Reactive] public ReadOnlyObservableCollection<ExceptionInfoViewModel> Exceptions { get; private set; }

    #endregion

    public SolidColorPaint LegendTextPaint { get; set; } =
        new()
        {
            Color = new SKColor(50, 50, 50),
            SKTypeface = SKTypeface.FromFamilyName("Courier New")
        };

    public SolidColorPaint LegendBackgroundPaint { get; set; } = new(new SKColor(240, 240, 240));

    [Reactive] public ICommand CancelCommand { get; }


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

    #region Info Panel

    [Reactive] public string Host { get; }

    [Reactive] public string Version { get; }

    [Reactive] public string ParallelCount { get; }

    #endregion

    #region Properties

    [Reactive] public int BotsOnline { get; private set; }

    [Reactive] public int CPS { get; private set; }

    [Reactive] public int PeakCPS { get; private set; }

    [Reactive] public string ProxyQuality { get; }


    [Reactive] public IEnumerable<ISeries> Proxy_Series { get; } = Enumerable.Empty<ISeries>();


    public ObservableCollection<LogEventViewModel> Logs { get; }

    #region Metrics

    public ObservableCollection<ISeries> BotsOnlineSeries { get; set; }
    public ObservableCollection<ISeries> CPSSeries { get; set; }

    public object BotsOnline_Sync { get; } = new();
    public object CPS_Sync { get; } = new();

    public Axis[] BotsAxis { get; set; }
    public Axis[] CPSAxis { get; set; }
    public Margin DrawMargin { get; set; }

    #endregion

    public string? UrlPathSegment => null;

    public IScreen HostScreen { get; }

    public ViewModelActivator Activator { get; } = new();

    #endregion
}

public class CustomLegend : IChartLegend<SkiaSharpDrawingContext>
{
    private static readonly int s_zIndex = 10050;

    private readonly SolidColorPaint _fontPaint = new(new SKColor(30, 20, 30))
    {
        SKTypeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold),
        ZIndex = s_zIndex + 1
    };

    private readonly StackPanel<RoundedRectangleGeometry, SkiaSharpDrawingContext> _stackPanel = new();

    public void Draw(Chart<SkiaSharpDrawingContext> chart)
    {
        var legendPosition = chart.GetLegendPosition();

        _stackPanel.X = legendPosition.X;
        _stackPanel.Y = legendPosition.Y;

        chart.AddVisual(_stackPanel);
        if (chart.LegendPosition == LegendPosition.Hidden) chart.RemoveVisual(_stackPanel);
    }

    public LvcSize Measure(Chart<SkiaSharpDrawingContext> chart)
    {
        _stackPanel.Orientation = ContainerOrientation.Vertical;
        _stackPanel.MaxWidth = double.MaxValue;
        _stackPanel.MaxHeight = chart.ControlSize.Height;

        // clear the previous elements.
        foreach (var visual in _stackPanel.Children.ToArray())
        {
            _ = _stackPanel.Children.Remove(visual);
            chart.RemoveVisual(visual);
        }

        var theme = LiveCharts.DefaultSettings.GetTheme<SkiaSharpDrawingContext>();

        foreach (var series in chart.Series.Where(x => x.IsVisibleAtLegend))
        {
            var panel = new StackPanel<RectangleGeometry, SkiaSharpDrawingContext>
            {
                Padding = new Padding(12, 6),
                VerticalAlignment = Align.Middle,
                HorizontalAlignment = Align.Middle,
                Children =
                {
                    new SVGVisual
                    {
                        Path = SKPath.ParseSvgPathData(SVGPoints.Star),
                        Width = 25,
                        Height = 25,
                        ClippingMode = ClipMode.None, // required on legends 
                        Fill = new SolidColorPaint(theme.GetSeriesColor(series).AsSKColor())
                        {
                            ZIndex = s_zIndex + 1
                        }
                    },
                    new LabelVisual
                    {
                        Text = series.Name ?? string.Empty,
                        Paint = _fontPaint,
                        TextSize = 15,
                        ClippingMode = ClipMode.None, // required on legends 
                        Padding = new Padding(8, 0, 0, 0),
                        VerticalAlignment = Align.Start,
                        HorizontalAlignment = Align.Start
                    }
                }
            };

            panel.PointerDown += GetPointerDownHandler(series);
            _stackPanel.Children.Add(panel);
        }

        return _stackPanel.Measure(chart);
    }

    private static VisualElementHandler<SkiaSharpDrawingContext> GetPointerDownHandler(
        IChartSeries<SkiaSharpDrawingContext> series)
    {
        return (visual, args) => { series.IsVisible = !series.IsVisible; };
    }
}