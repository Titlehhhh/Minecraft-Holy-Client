using System.ComponentModel;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using HolyClient.Abstractions.StressTest;
using Serilog;
using Zennolab.CapMonsterCloud;

namespace HolyClient.StressTest;

public class DefaultBehavior : BaseStressTestBehavior
{
    private static Regex SayVerifyRegex = new(@"\.say \/verify (\d+)");

    private IObservable<long> StaticSpam;
    [DisplayName("Spam Enable")] public bool SpamEnable { get; set; } = true;

    [DisplayName("Spam text")]
    public BindingList<string> SpamTexts { get; set; } = new BindingList<string>
    {
        "!Hello from Minecraft Holy Client"
    };

    [DisplayName("Fisrt text send")]
    public BindingList<string> FirstText { get; set; } = new()
    {
        "/register 21qwerty 21qwerty"
    };

    [DisplayName("Spam timeout")] public int SpamTimeout { get; set; } = 1000;
    [DisplayName("Start bot timeout")] public int StartTimeout { get; set; } = 100;

    [DisplayName("Reconnect timeout")] public int ReconnectTimeout { get; set; } = 5000;

    [DisplayName("Reconnect count")] public int Reconnects { get; set; } = 1;

    [DisplayName("Spam Nocom")] public bool SpamNocom { get; set; } = false;

    [DisplayName("Crash Completion")] public bool CrashCompletion { get; set; } = false;


    private ILogger _logger;
    private ICapMonsterCloudClient _capMonsterClient;

    public DefaultBehavior()
    {
    }

    private volatile int IsCaptcha = 0;

    public override Task Start(CompositeDisposable d, IEnumerable<IStressTestBot> bots, ILogger logger,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    

}