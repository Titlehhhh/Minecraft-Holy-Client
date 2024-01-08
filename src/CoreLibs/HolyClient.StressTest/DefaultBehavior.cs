using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HolyClient.StressTest
{
	public class DefaultBehavior : IStressTestBehavior
	{
		[System.ComponentModel.DisplayName("Spam text")]
		public string SpamText { get; set; } = "!Hello from Minecraft Holy Client";

		[System.ComponentModel.DisplayName("Spam timeout")]
		public int SpamTimeout { get; set; } = 5000;


		private static Regex SayVerifyRegex = new(@"\.say \/verify (\d+)");

		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			foreach (var bot in bots)
			{
				CancellationTokenSource cts = null;

				var d = bot.Client.State.Subscribe(s =>
				{

				}, async ex =>
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

					
				}, () =>
				{

				});

				disposables.Add(d);

				var d2 = bot.Client.OnJoinGame.Subscribe(async x =>
				{
					cts = new();
					try
					{


						await Task.Delay(500);

						await bot.Client.SendChat("/register 21qwerty 21qwerty");

						try
						{
							using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
							

							var m = await bot.Client.OnChatMessage
								.Where(x => x.Message.Contains("verify"))
								.Skip(3)
								.FirstAsync()
								.ToTask(cts.Token);

							var code = SayVerifyRegex.Match(m.Message).Value;

							await bot.Client.SendChat(code);
							
						}
						catch (Exception ex)
						{
							
						}

						while (!cts.IsCancellationRequested)
						{
							await bot.Client.SendChat(SpamText);
							if (SpamTimeout <= 0)
								await Task.Delay(1000);
							else
								await Task.Delay(SpamTimeout);
						}
					}
					catch (Exception ex)
					{
						
					}
					finally
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
