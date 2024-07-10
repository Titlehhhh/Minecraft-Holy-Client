using System.Collections.Concurrent;
using System.ComponentModel;
using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;
using MessagePack;
using Serilog;

namespace HolyClient.StressTest;

public class ExceptionThrowCount
{
    public Type TypeException { get; set; }

    public int Count { get; set; }

    public Dictionary<string, int> Messages { get; set; }
}

[Union(0, typeof(StressTestProfile))]
public interface IStressTestProfile : INotifyPropertyChanged, INotifyPropertyChanging
{
    Guid Id { get; set; }
    string Name { get; set; }


    string Server { get; set; }


    string BotsNickname { get; set; }


    int NumberOfBots { get; set; }

    bool UseProxy { get; set; }
    int Version { get; set; }


    ISourceCache<IProxySource, Guid> Proxies { get; }


    IObservable<StressTestMetrik> Metrics { get; }

    ProxyCheckerOptions ProxyCheckerOptions { get; set; }

    bool ParallelCountCheckingCalculateAuto { get; set; }
    ConcurrentDictionary<Tuple<string, string>, ExceptionCounter> ExceptionCounter { get; }

    IStressTestBehavior Behavior { get; }
    StressTestServiceState CurrentState { get; }

    PluginTypeReference BehaviorRef { get; }
    bool OptimizeDNS { get; set; }

    void SetBehavior(IPluginSource pluginSource);
    void DeleteBehavior();

    Task Start(ILogger logger);
    Task Stop();

    Task Initialization(IPluginProvider pluginProvider);
}