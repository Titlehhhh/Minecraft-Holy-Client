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

				//Console.WriteLine(x.Message);

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

					//Nuker();

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

					
					Vector3 block = new Vector3(Random.Shared.Next(-20000,2000), Random.Shared.Next(0,255), Random.Shared.Next(0,20000));

					await _bot.Client.SendAction(0, block, McProtoNet.Core.BlockFace.DOWN);

					await Task.Delay(500, _cts.Token);

					await _bot.Client.SendAction(2, block, McProtoNet.Core.BlockFace.DOWN);
							
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
				await _bot.Client.SendChat("/reg 21qwerty 21qwerty");
				await _bot.Client.SendChat("/login 21qwerty");
				
				while (!_cts.IsCancellationRequested)
				{
					await Task.Delay(3000);

					await _bot.Client.SendChat($"!{Random.Shared.Next()} Minecraft Holy Client Best stress tes tool {Random.Shared.Next()}");
					
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
