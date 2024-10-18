using System.ComponentModel;
using System.Reactive.Disposables;
using HolyClient.Abstractions.StressTest;
using Serilog;

namespace HolyClient.StressTest;

[Description("Bots with physics, captches, chunks and other")]
[DisplayName("Advance behavior")]
public class AdvancedBehavior : BaseStressTestBehavior
{
    
    public override async Task Start(CompositeDisposable d, 
        IEnumerable<IStressTestBot> bots, 
        ILogger logger, 
        CancellationToken cancellationToken)
    {
         
    }

    public override Task Stop()
    {
        return base.Stop();
    }
}