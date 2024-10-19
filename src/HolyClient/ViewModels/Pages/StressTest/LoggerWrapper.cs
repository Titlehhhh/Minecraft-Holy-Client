using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using DynamicData;
using ReactiveUI;
using Serilog;
using Serilog.Events;

namespace HolyClient.ViewModels;

public class LoggerWrapper : ILogger, IDisposable
{
    private IDisposable? _cleanUp;

    private readonly SourceList<LogEvent> _logs = new();
    

    public LoggerWrapper()
    {
        CompositeDisposable disp = new CompositeDisposable();
        _cleanUp = _logs
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Transform(x => new LogEventViewModel(x))
            .OnItemAdded(x =>
            {
                if (Events.Count >= 200) Events.RemoveAt(0);
                Events.Add(x);
            })
            .Subscribe()
            .DisposeWith(disp);
        _cleanUp = disp;
        //Events = events;
    }

    public ObservableCollection<LogEventViewModel> Events { get; } = new();

    public void Dispose()
    {
        Interlocked.Exchange(ref _cleanUp, null)?.Dispose();
    }

    public void Write(LogEvent logEvent)
    {
        _logs.Add(logEvent);
    }
}

public class LogEventViewModel
{
    public string Level { get; }
    public string Timestamp { get; }
    public Exception? Exception { get; }
    public string Message { get; }
    

    public LogEventViewModel(LogEvent logEvent)
    {
        Level = logEvent.Level.ToString();
        
        
        var time = logEvent.Timestamp.ToString("HH:mm:ss");
        Timestamp = time;
        Message = logEvent.MessageTemplate.ToString();
        Exception = logEvent.Exception;

        var ex = "";

        if (logEvent.Exception is not null) ex = $"\n{logEvent.Exception.Message}\n{logEvent.Exception.StackTrace}";

        Text = $"[{time} {logEvent.Level}] {logEvent.MessageTemplate}{ex}";
    }

    public string Text { get; }
}