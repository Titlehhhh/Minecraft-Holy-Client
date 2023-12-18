using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;

namespace HolyClient.StressTest
{
	public class TestBehavior : IStressTestBehavior
	{
		[System.ComponentModel.DisplayName("Spam text")]
		public string SpamText { get; set; } = "Hello";

		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			foreach (var bot in bots)
			{
				CancellationTokenSource cts = null;

				var d = bot.OnError.Subscribe(async x =>
				{
					
					try
					{
						if (cts is not null)
						{
							cts.Cancel();
							cts.Dispose();
							
						}
					}
					catch
					{

					}
					finally
					{
						cts = null;
					}
					
						await Task.Delay(1500);
						await bot.Restart(true);
					
				});

				disposables.Add(d);

				var d2 = bot.Client.OnJoinGame.Subscribe(async x =>
				{
					cts = new();
					try
					{
						await Task.Delay(500);
						await bot.Client.SendChat("/reg 21qwerty 21qwerty");

						while (!cts.IsCancellationRequested)
						{
							await bot.Client.SendChat(SpamText);
							await Task.Delay(1000);
						}
					}
					catch(Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
					finally
					{
						Console.WriteLine("cancel");
					}
				});

				disposables.Add(d2);

				_ = bot.Restart(true);

			}
			return Task.CompletedTask;
		}
	}

}
