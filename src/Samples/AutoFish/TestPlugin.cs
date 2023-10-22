using HolyClient.Core.Models.BotManager;
using System.ComponentModel.Composition;
using System.Reactive.Disposables;

namespace AutoFish
{
	[Export(typeof(IBotPlugin))]
	public class TestPlugin : HolyClient.SDK.BotPlugin
	{
		public override void Activate(CompositeDisposable d)
		{
		}
	}
}