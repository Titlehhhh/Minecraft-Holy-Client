using HolyClient.Core.Models.BotManager;
using McProtoNet.MultiVersion;
using System.Reactive.Disposables;

namespace HolyClient.SDK
{
	public abstract class BotPlugin : IBotPlugin
	{
		public Serilog.ILogger Logger { get; set; }
		public MinecraftClient Client { get; set; }

		public abstract void Activate(CompositeDisposable d);
	}
}
