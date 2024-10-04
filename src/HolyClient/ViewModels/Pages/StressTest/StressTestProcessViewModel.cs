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

public class StressTestProcessViewModel : ReactiveObject, IStressTestProcessViewModel, IDisposable
{
    private readonly DateTimeAxis _botsOnlineAxis;

    private List<DateTimePoint> _botsOnlineValues = new();
    private DateTimeAxis _cpsAxis;
    private List<DateTimePoint> _cpsValues = new();
    private ICommand _cancel;


    public StressTestProcessViewModel(ICommand cancel, IStressTestProfile stressTest, LoggerWrapper wrapper)
    {
        Logs = wrapper.Events;
        Host = stressTest.Server;
        // Version = MinecraftVersionToStringConverter.McVerToString(stressTest.Version);
        Version = stressTest.Version.ToString();
        ParallelCount = stressTest.NumberOfBots.ToString();


        _cancel = cancel;


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
                .Subscribe(data => { exceptions.AddOrUpdate(data); }).DisposeWith(d);
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

    public ICommand CancelCommand => _cancel;


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

    [Reactive] public string Host { get; set; }

    [Reactive] public string Version { get; set; }

    [Reactive] public string ParallelCount { get; set; }

    #endregion

    #region Properties

    [Reactive] public int BotsOnline { get; private set; }

    [Reactive] public int CPS { get; private set; }

    [Reactive] public int PeakCPS { get; private set; }

    [Reactive] public string ProxyQuality { get; private set; }


    [Reactive] public IEnumerable<ISeries> Proxy_Series { get; private set; } = Enumerable.Empty<ISeries>();


    public ObservableCollection<LogEventViewModel> Logs { get; private set; }

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

    public ViewModelActivator Activator { get; set; } = new();

    #endregion

    public void Dispose()
    {
        Console.WriteLine("Disp");
        Activator.Dispose();
        this.Activator = null;
        _botsOnlineValues = null;
        _cpsAxis = null;
        _cpsValues = null;
        _cancel = null;
        Proxy_Series = null;
        LegendTextPaint?.Dispose();
        LegendTextPaint = null;
        LegendBackgroundPaint?.Dispose();
        LegendBackgroundPaint = null;
        BotsOnlineSeries.Clear();
        CPSSeries.Clear();
        Logs = null;
        Proxy_Series = null;
        Exceptions = null;
        GC.SuppressFinalize(this);
    }
}