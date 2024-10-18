using System.ComponentModel;
using System.Reactive.Disposables;
using Avalonia.Controls;
using ReactiveUI;

namespace HolyClient.Abstractions.StressTest;

public abstract class BaseStressTestBehavior : ReactiveObject, IStressTestBehavior
{
    [Browsable(false)] public ReactiveObject? DefaultViewModel { get; protected set; }
    [Browsable(false)] public ReactiveObject? ProcessViewModel { get; protected set; }
    [Browsable(false)] public ResourceDictionary? Resources { get; protected set; }
    [Browsable(false)] public StressTestOptions Options { get; set; }

    
    public virtual Task Initialization(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task Shutdown() => Task.CompletedTask;


    public abstract Task Start(CompositeDisposable d, IEnumerable<IStressTestBot> bots,
        Serilog.ILogger logger,
        CancellationToken cancellationToken);

    public virtual Task Stop() => Task.CompletedTask;
}