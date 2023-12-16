﻿using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;

namespace TestStressTestBehavior
{

	public class SpammerHelloBehavior : IStressTestBehavior
	{
		[System.ComponentModel.DisplayName("Spam text")]
		public string SpamText { get; set; } = "Hello";

		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			foreach (var bot in bots)
			{
				var d = bot.OnError.Subscribe(async x =>
				{
					Console.WriteLine(x.Message);

					await Task.Delay(1500);
					await bot.Restart(true);
				});

				disposables.Add(d);

				var d2 = bot.Client.OnJoinGame.Subscribe(async x =>
				{
					await Task.Delay(1000);

					try
					{
						while (true)
						{
							await bot.Client.SendChat(SpamText);
							await Task.Delay(1000);
						}
					}
					catch
					{

					}
				});

				disposables.Add(d2);

				_ = bot.Restart(true);

			}
			return Task.CompletedTask;
		}
	}

}
