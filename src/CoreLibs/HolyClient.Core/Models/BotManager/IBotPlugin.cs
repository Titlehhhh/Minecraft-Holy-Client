using System.Reactive.Disposables;
using McProtoNet.Client;
using Serilog;

namespace HolyClient.Core.Models.BotManager;

public interface IBotPlugin
{
    ILogger Logger { get; set; }
    MinecraftClient Client { get; set; }
    void Activate(CompositeDisposable d);
}