using System.ComponentModel;
using System.Reactive.Disposables;
using Avalonia.Controls;
using ReactiveUI;

namespace HolyClient.Abstractions.StressTest;

public abstract class BaseStressTestBehavior : IStressTestBehavior
{
    [Browsable(false)]
    public ReactiveObject? DefaultViewModel { get; protected set; }
    [Browsable(false)]
    public ReactiveObject? ProcessViewModel { get; protected set; }
    [Browsable(false)]
    public ResourceDictionary? Resources { get; protected set; }

    public abstract Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, Serilog.ILogger logger,
        CancellationToken cancellationToken);
}