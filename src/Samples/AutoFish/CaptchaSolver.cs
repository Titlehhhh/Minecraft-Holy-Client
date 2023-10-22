using HolyClient.Core.Models.BotManager;
using HolyClient.SDK.Attributes;
using System.ComponentModel.Composition;
using System.Reactive.Disposables;

namespace AutoFish
{
	[PluginTitle("Captcha solver")]
	[PluginAuthor("Titlehhhh")]
	[PluginDescription("Плагин для решения каптч в анти-бот системах")]
	[Export(typeof(IBotPlugin))]
	public class CaptchaSolver : HolyClient.SDK.BotPlugin
	{
		public override void Activate(CompositeDisposable d)
		{
			
		}
	}
}