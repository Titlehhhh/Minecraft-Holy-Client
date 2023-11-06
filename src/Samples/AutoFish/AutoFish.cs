using HolyClient.SDK;
using System.Reactive.Disposables;

namespace AutoFish
{

	public class AutoFish : BotPlugin
	{
		public override void Activate(CompositeDisposable d)
		{
			this.Logger.Information("asd");
			this.Client.OnChatMessage.Subscribe(m =>
			{
				if (m.Message.Contains("hi"))
				{
					this.Client.SendChat("Привет");
				}
			}).DisposeWith(d);
			Logger.Information("Sub1");
		}
	}
}