using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;

namespace TestStressTestBehavior
{

	public class SpammerHelloBehavior : IStressTestBehavior
	{
		public string Test { get; set; }

		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			foreach (var bot in bots)
			{
				var d = bot.OnError.Subscribe(async x =>
				{
					await Task.Delay(1500);
					await bot.Restart(true);
				});

				disposables.Add(d);

				var d2 = bot.Client.OnJoinGame.Subscribe(x =>
				{
					bot.Client.SendChat("Hi");
				});

				disposables.Add(d2);

				_ = bot.Restart(true);

			}
			return Task.CompletedTask;
		}
	}

}
