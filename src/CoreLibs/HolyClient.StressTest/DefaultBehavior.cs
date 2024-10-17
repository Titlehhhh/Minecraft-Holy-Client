using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using HolyClient.Abstractions.StressTest;
using McProtoNet;
using McProtoNet.Abstractions;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Net;
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

    [DisplayName("Reconnect timeout")] public int ReconnectTimeout { get; set; } = 5000;

    [DisplayName("Reconnect count")] public int Reconnects { get; set; } = 1;

    [DisplayName("Spam Nocom")] public bool SpamNocom { get; set; } = false;

    public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger,
        CancellationToken cancellationToken)
    {
        logger.Information("Start default behavior");
        foreach (var bot in bots)
        {
            bot.ConfigureAutoRestart(AutoRestartAction);
            MultiProtocol proto = bot.Protocol as MultiProtocol;

            proto.OnLogin.Subscribe(async x =>
            {
                try
                {
                    await Task.Delay(2000);
                    await proto.SendChatPacket("/register 21qwerty 21qwerty");
                    CancellationTokenSource cts = new CancellationTokenSource();
                    Disposable.Create(() =>
                    {
                        try
                        {
                            cts.Cancel();
                        }
                        finally
                        {
                            cts.Dispose();
                        }
                    }).DisposeWith(disposables);
                    
                    while (!cts.IsCancellationRequested)
                    {
                        await proto.SendChatPacket(SpamText);
                        await Task.Delay(SpamTimeout, cts.Token);
                    }
                }
                catch
                {
                    // ignored
                }
            }).DisposeWith(disposables);
            bot.Restart(true);
        }

        return Task.CompletedTask;
    }

    private async void AutoRestartAction(IStressTestBot b)
    {
        await Task.Delay(ReconnectTimeout);
        try
        {
            await b.Restart(true);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }


    private const string lagMessage =
        "null梭꿿苁ᢣ쬨?Ꮥ軉㚵龁ꥰ䙏껎娇敲\uf1fa\u2731衍\uf118쵌虎\u256d䁉횋祅\u2bd4澙봮ꆃᱨ䡮捺좢\uf7ca告쪐읭籡\u0304৫錫埅᠂\u2813蹃㵺븥鰜恂?勔\uf197盡ᑐ紒㺏\u1758筏信뉪?祉\ue160让ᐢ촯쨵\u0c01濳녂ｽ낌㾅뱝㲈敮ᱤ\u25ef风쓫谘쁫荬놙쁁\u2b39堾ἢ蟜ㅝ\u222e褉竨陼翲䗅륹\uf30e홴Ҵሾㅪ莍ආ爥果둰?蜮㟯\u032d崍퀒낢려设牢睟\ue605\u0f31෯꿍팃ꦤ愮旓ᅗⲰ틬㼰뜍둲\u206c쉬᠓\u2540靅\u1a56蠒睩咸\u17f2쉬ⶬ쭉糷ᧈ츷博苾机ꁘ굝샕ߜ杲ꏄ꾤曍ቈ㥬褞脴戰\uf876쑍뮴\u139e\uebd2鉞\ue3f9\u2205삧\uea3a綎鰞뚢嗝贫읲ᰌ퓟勘爱痟駵駙\u187a뒤\u2003ꡢ坱蔊ꊭ\uf72b滨孃㝬鿙鞸";

    private const string payload =
        "{a:[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[[}";
}