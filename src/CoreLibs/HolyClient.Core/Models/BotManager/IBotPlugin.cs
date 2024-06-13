using System.Reactive.Disposables;

namespace HolyClient.Core.Models.BotManager
{
	public interface IBotPlugin
	{
		Serilog.ILogger Logger { get; set; }
		MinecraftClient Client { get; set; }
		void Activate(CompositeDisposable d);
	}
}
