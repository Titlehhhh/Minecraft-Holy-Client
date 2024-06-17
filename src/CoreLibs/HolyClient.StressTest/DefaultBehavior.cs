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


        foreach (var bot in bots)
        {
            Protocol_754 proto = new Protocol_754(bot.Client);

            proto.OnKeepAlivePacket.Subscribe(x => { proto.SendKeepAlive(x.KeepAliveId); });

            proto.OnLoginPacket.Subscribe(async x =>
            {
                try
                {
                    await Task.Delay(500);
                    await proto.SendChat("/register 21qwerty 21qwerty");
                    while (true)
                    {
                        await Task.Delay(3000);

                        //if (Random.Shared.NextDouble() >= 0.5)
                        {
                            //await proto.SendChat("Вы негры");
                        }
                       // else
                        {
                           // await proto.SendChat("https://discord.com/invite/5Huju3Ka5P");
                        }
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
                    logger.Error(args.Error,"Disconnect Error");
                    await Task.Delay(3000);
                    await bot.Restart(true);
                }
            };

            _ = bot.Restart(true);
        }

        return Task.CompletedTask;
    }
}