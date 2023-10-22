using Fody;
using HolyClient.Abastractions.StressTest;
using McProtoNet.MultiVersion;
using System.Reactive.Disposables;

namespace HolyClient.Core.StressTest
{
	[ConfigureAwait(false)]
	class DefaultStressTestBehavior : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			foreach (var item in bots)
			{
				if (cancellationToken.IsCancellationRequested)
					return Task.CompletedTask;
				InitBot(disposables, item, cancellationToken);
			}
			return Task.CompletedTask;

		}

		private async void InitBot(CompositeDisposable disp, IStressTestBot bot, CancellationToken cancellationToken)
		{
			bot.OnError.Subscribe(async x =>
			{
				try
				{
					if (cancellationToken.IsCancellationRequested)
						return;
					await Task.Delay(3000, cancellationToken);
					await bot.Restart(true);
				}
				catch
				{

				}
			}).DisposeWith(disp);

			bot.Client.OnJoinGame.Subscribe(async x =>
			{
			
				try
				{
					//await Task.Delay(1500);
					while (bot.Client.State == ClientState.Play)
					{
						const string SpamText = "SPAM SPAM SPAM SPAM";
						await bot.Client.SendChat(SpamText);
						await Task.Delay(3000);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}).DisposeWith(disp);

			await bot.Restart(true);



		}
	}

}
