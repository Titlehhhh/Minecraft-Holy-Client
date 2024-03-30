using HolyClient.Abstractions.StressTest;
using Serilog;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HolyClient.StressTest
{
	public class TidePVPBehaviorAttack : BaseStressTestBehavior
	{


		public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger, CancellationToken cancellationToken)
		{
			foreach (var bot in bots)
			{





				bot.Client.OnDisconnect.Subscribe(async x =>
				{
					bool need = true;
					do
					{
						try
						{
							await Task.Delay(5000);

							await bot.Restart(true);
							need = false;
						}
						catch
						{

						}
					} while (need);
				});


				var d2 = bot.Client.OnJoinGame.Subscribe(async x =>
				{

				});

				disposables.Add(d2);

				_ = bot.Restart(true);

			}
			return Task.CompletedTask;
		}
	}

}
