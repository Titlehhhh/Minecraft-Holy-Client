using System.ComponentModel;
using System.Reactive.Disposables;
using Avalonia.Controls;
using ReactiveUI;

namespace HolyClient.Abstractions.StressTest;

/// <summary>
/// Represents the behavior that bots will perform
/// </summary>
public interface IStressTestBehavior : INotifyPropertyChanged,INotifyPropertyChanging
{
    ReactiveObject? DefaultViewModel { get; }

    ReactiveObject? ProcessViewModel { get; }


    ResourceDictionary? Resources { get; }

    /// <summary>
    /// It is always called once when the application is launched or an assembly with this plugin has loaded. This can be used to load settings from disk.
    /// </summary>
    public Task Initialization(CancellationToken cancellationToken=default);
    /// <summary>
    /// It is always called once when the application shuts down or before unloading the assembly from the memory of this plugin. This can be used to write settings to disk.
    /// </summary>
    public Task Shutdown();
    public StressTestOptions Options { get; set; }

    /// <summary>
    /// Running a custom stress test script
    /// </summary>
    /// <param name="d">IDisposable list of all event subscriptions</param>
    /// <param name="bots">Bots that you control</param>
    /// <param name="logger">Logger</param>
    /// <param name="cancellationToken">A cancellation token to cancel the start of the behavior</param>
    /// <returns>The task that represents the start of the behavior</returns>
    public Task Start(
        CompositeDisposable d,
        IEnumerable<IStressTestBot> bots,
        Serilog.ILogger logger,
        CancellationToken cancellationToken);

    public Task Stop();
}

public struct StressTestOptions
{
    public string BotsNickanems { get;  }
    public string TargetHost { get; }
    
}