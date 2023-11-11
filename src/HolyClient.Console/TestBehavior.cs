using HolyClient.Abstractions.StressTest;
using McProtoNet;
using System.Reactive.Disposables;

public class TestBehavior : IStressTestBehavior
{
	public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
	{
		List<Bot> wrappers = new();
		foreach (var bot in bots)
		{
			Bot botWrapper = new(bot, disposables, cancellationToken);
			wrappers.Add(botWrapper);
		}
		wrappers.ForEach(x => x.Run());

		return Task.CompletedTask;
	}

	class Bot
	{

		private CancellationTokenSource? _cts = new();
		private IStressTestBot _bot;

		private Vector3 _currentPosition = default;
		private Rotation _currentRotation = default;

		private bool _firstPositionRotationPacket = true;



		public Bot(IStressTestBot bot,
			CompositeDisposable d,
			CancellationToken cancellation)
		{

			_bot = bot;

			bot.OnError.Subscribe(async x =>
			{
				if (_cts is not null)
					await _cts.CancelAsync();
				_cts = new();

				_firstPositionRotationPacket = true;
				await Task.Delay(1500);
				await bot.Restart(true);
			}).DisposeWith(d);



			bot.Client.OnPlayerPositionRotation.Subscribe(async x =>
			{

				_currentPosition = x.GetPosition(_currentPosition);
				_currentRotation = x.GetRotation();

				if (_firstPositionRotationPacket)
				{
					_firstPositionRotationPacket = false;
					await bot.Client.SendPositionRotation(_currentPosition, _currentRotation, true);

					StartMoveUp();
				}



				_ = bot.Client.SendTeleportConfirm(x.TeleportId);

			}).DisposeWith(d);



		}

		private async void StartMoveUp()
		{
			try
			{
				
				while (!_cts.IsCancellationRequested)
				{
					await Task.Delay(1000);
					
					await _bot.Client.SendChat("Minecraft Holy Client - High performance stress test tool");
				}
			}
			catch
			{

			}
		}




		public async void Run()
		{
			await _bot.Restart(true);
		}


	}
}
