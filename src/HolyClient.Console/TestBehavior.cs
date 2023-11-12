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

					Nuker();

				}
				_ = bot.Client.SendTeleportConfirm(x.TeleportId);

			}).DisposeWith(d);

			bot.Client.OnJoinGame.Subscribe(x =>
			{
				StartSpam();
			}).DisposeWith(d);



		}
		private async void Nuker()
		{
			try
			{
				Vector3 radius = new Vector3(3);
				while (!_cts.IsCancellationRequested)
				{

					Vector3 start = Vector3.Min(_currentPosition, _currentPosition - radius);
					Vector3 end = Vector3.Min(_currentPosition, _currentPosition + radius);

					for (double x = start.X; x < end.X; x++)
					{
						for (double y = start.Y; y < end.Y; x++)
						{
							for (double z = start.Z; y < end.Z; x++)
							{
								Vector3 block = new Vector3(x, y, z);

								await _bot.Client.SendAction(0, block, McProtoNet.Core.BlockFace.DOWN);

								await Task.Delay(500, _cts.Token);

								await _bot.Client.SendAction(2, block, McProtoNet.Core.BlockFace.DOWN);
							}

						}
					}
				}
			}
			catch
			{

			}
		}
		private async void StartSpam()
		{
			try
			{
				await Task.Delay(1000);
				await _bot.Client.SendChat("/register 21qwerty 21qwerty");
				await _bot.Client.SendChat("/login 21qwerty");
				while (!_cts.IsCancellationRequested)
				{
					await Task.Delay(5000);

					if (Random.Shared.NextDouble() >= 0.5)
					{
						await _bot.Client.SendChat("!SOSITE https://discord.gg/HVDzx4rCgg " + Random.Shared.Next());
					}
					else
					{
						await _bot.Client.SendChat("!гугли Minecraft Holy Client " + Random.Shared.Next());
					}
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
