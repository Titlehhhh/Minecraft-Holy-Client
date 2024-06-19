using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using HolyClient.Abstractions.StressTest;
using McProtoNet;
using McProtoNet.Abstractions;
using McProtoNet.Protocol;
using McProtoNet.Protocol754;
using ReactiveUI;
using Serilog;

namespace HolyClient.StressTest;

public class DefaultBehavior : BaseStressTestBehavior
{
    private static Regex SayVerifyRegex = new(@"\.say \/verify (\d+)");

    private IObservable<long> StaticSpam;

    [DisplayName("Spam text")] public string SpamText { get; set; } = "!Spam Spam Spam";

    [DisplayName("Spam timeout")] public int SpamTimeout { get; set; } = 2500;

    [DisplayName("Reconnect timeout")] public int ReconnectTimeout { get; set; } = 1000;

    [DisplayName("Reconnect count")] public int Reconnects { get; set; } = 1;

    [DisplayName("Spam Nocom")] public bool SpamNocom { get; set; } = false;

    public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger,
        CancellationToken cancellationToken)
    {
        logger.Information("Start default behavior");
        StaticSpam = Observable.Interval(TimeSpan.FromMilliseconds(SpamTimeout), RxApp.TaskpoolScheduler);

        ConcurrentBag<int> entities = new();
        foreach (var bot in bots)
        {
            Protocol_754 proto = new Protocol_754(bot.Client);

            proto.OnKeepAlivePacket.Subscribe(x => { proto.SendKeepAlive(x.KeepAliveId); });
            proto.OnSpawnEntityPacket.Subscribe(x => { entities.Add(x.EntityId); });
            proto.OnLoginPacket.Subscribe(async x =>
            {
                try
                {
                    await Task.Delay(500);
                    await proto.SendChat("/register 21qwerty 21qwerty");

                    while (true)
                    {
                        await Task.Delay(100);

                        var pos = new Position(Random.Shared.Next(-100_000, 100_000),
                            Random.Shared.Next(-100_000, 100_000),
                            Random.Shared.Next(0, 128));
                        
                        await proto.SendBlockDig(0, pos, 2);
                        await proto.SendBlockDig(2, pos, 0);
                        await proto.SendArmAnimation(0);
                    }
                }
                catch
                {
                }
            });

            bot.Client.StateChanged += async (sender, args) =>
            {
                if (args.State == MinecraftClientState.Errored)
                {
                    //logger.Error(args.Error,"Disconnect Error");
                    await Task.Delay(3000);
                    await bot.Restart(true);
                }
            };

            _ = bot.Restart(true);
        }

        return Task.CompletedTask;
    }
}