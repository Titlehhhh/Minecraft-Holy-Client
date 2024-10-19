using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DotNext;
using HolyClient.ViewModels;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using Serilog.Events;
using Serilog.Parsing;

namespace HolyClient.DesignTime;

public class DesignStressTestProcessViewModel : ReactiveObject, IStressTestProcessViewModel
{
    public DesignStressTestProcessViewModel()
    {
        BotsOnline_Series = new List<ISeries>();
        CPS_Series = new List<ISeries>();
        Exceptions = new List<ExceptionInfoViewModel>();
    }

    public IEnumerable<ISeries> BotsOnline_Series { get; set; }

    public IEnumerable<ISeries> CPS_Series { get; set; }

    public Axis[] XAxes { get; set; }

    public ICommand CancelCommand { get; set; } = ReactiveCommand.Create(() => { });
    public ObservableCollection<ISeries> BotsOnlineSeries { get; } = new();
    public ObservableCollection<ISeries> CPSSeries { get; } = new();
    public object BotsOnline_Sync { get; } = new();
    public object CPS_Sync { get; } = new();
    public Axis[] BotsAxis { get; } = [];
    public Axis[] CPSAxis { get; } = [];
    public Margin DrawMargin { get; } = new();

    public int WarningCount => 0;

    public int ErrorCount => 0;

    public IEnumerable<ExceptionInfoViewModel> Exceptions { get; }

    public string? UrlPathSegment => "";

    public IScreen HostScreen { get; set; }

    public ViewModelActivator Activator { get; set; } = new();

    public IEnumerable<ISeries> Proxy_Series => Enumerable.Empty<ISeries>();

    public string Host => "DDOS: example.org";

    public string Version => "Version: 1.16.5";

    public string ParallelCount => "1000";

    public int BotsOnline => 75;

    public int CPS => 5;

    public int PeakCPS => 0;

    public string ProxyQuality => "100%";

    public ObservableCollection<LogEventViewModel> Logs { get; } = CreateLogs();

    private static ObservableCollection<LogEventViewModel> CreateLogs()
    {
        ObservableCollection<LogEventViewModel> logs = new();
        Random r = new Random(71);
        for (int i = 0; i < 25; i++)
        {
            DateTimeOffset offset = DateTimeOffset.Now;
            LogEventLevel level = (LogEventLevel)r.Next(0, 6);
            Exception? ex = null;
            if (level == LogEventLevel.Error)
            {
                try
                {
                    throw new InvalidOperationException("Test Exception");
                }
                catch (Exception e)
                {
                    ex = e;
                }
            }
            LogEvent logEvent = new LogEvent(offset, level, ex,
                new MessageTemplate($"Text #{i}", Array.Empty<MessageTemplateToken>()),
                Array.Empty<LogEventProperty>());
            logs.Add(new LogEventViewModel(logEvent));
        }

        return logs;
    }
}