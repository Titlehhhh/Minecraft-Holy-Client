using System.Reactive.Disposables;
using Avalonia.Controls;
using ReactiveUI;


namespace HolyClient.Abstractions.StressTest;

public interface IStressTestBehavior
{
    ReactiveObject? DefaultViewModel { get; }

    ReactiveObject? ProcessViewModel { get; }


    ResourceDictionary? Resources { get; }

    public Task Activate(
        CompositeDisposable disposables,
        IEnumerable<IStressTestBot> bots,
        Serilog.ILogger logger,
        CancellationToken cancellationToken);
}