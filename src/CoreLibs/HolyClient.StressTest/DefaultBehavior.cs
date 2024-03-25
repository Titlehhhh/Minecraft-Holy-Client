using HolyClient.Abstractions.StressTest;
using Serilog;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HolyClient.StressTest
{
	public class DefaultBehavior : BaseStressTestBehavior
	{
		[System.ComponentModel.DisplayName("Spam text")]
		public string SpamText { get; set; } = "!github com Titlehhhh Minecraft-Holy-Client";

		[System.ComponentModel.DisplayName("Spam timeout")]
		public int SpamTimeout { get; set; } = 2500;
		[System.ComponentModel.DisplayName("Reconnect timeout")]
		public int ReconnectTimeout { get; set; } = 1000;

		[System.ComponentModel.DisplayName("Reconnect count")]
		public int Reconnects { get; set; } = 1;

		[System.ComponentModel.DisplayName("Spam Nocom")]
		public bool SpamNocom { get; set; } = false;

		private static Regex SayVerifyRegex = new(@"\.say \/verify (\d+)");

		public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger, CancellationToken cancellationToken)
		{
			foreach (var bot in bots)
			{
				CancellationTokenSource cts = null;


				Action<Exception> onErr = async (exc) =>
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
					if (Reconnects <= 1)
					{
						await bot.Restart(true);
					}
					else
					{

						for (int i = 0; i < Reconnects - 1; i++)
						{
							if (ReconnectTimeout <= 0)
								await Task.Delay(1000);
							else
								await Task.Delay(ReconnectTimeout);



							await bot.Restart(false);
						}
						await bot.Restart(true);
					}
				};

				bot.Client.OnErrored += onErr;


				disposables.Add(Disposable.Create(() =>
				{
					bot.Client.OnErrored -= onErr;
				}));

				var d2 = bot.Client.OnJoinGame.Subscribe(async x =>
				{
					cts = new();
					try
					{
						//await bot.Client.SendSettings("ru", 8, 1, true, 255, 1);

						await Task.Delay(1000);

						await bot.Client.SendChat("/register 21qwerty");
						await bot.Client.SendChat("/register 21qwerty 21qwerty");
						await bot.Client.SendChat("/reg 21qwerty 21qwerty");
						await bot.Client.SendChat("/login 21qwerty");
						if (false)
						{
							try
							{
								using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));


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
						}

						await Task.Delay(1000);

						await bot.Client.SendChat("/menu");


						IDisposable d = null;
						d = bot.Client.OnOpenWindow.Subscribe(async x =>
						{
							d?.Dispose();
							logger.Debug("menu: " + x.Id);



							await bot.Client.SendPacket(w =>
							{
								w.WriteUnsignedByte((byte)x.Id);

								w.WriteShort(3);

								w.WriteByte(0);
								w.WriteShort(0);

								w.WriteVarInt(0);
								w.WriteBoolean(false);

							}, McProtoNet.PacketOut.ClickWindow);

							await Task.Delay(1000);

							try
							{


								var spamming = SpamMessage(cts, bot);
								var nuker = SpamNocomAsync(cts, bot);

								await Task.WhenAll(spamming, nuker);
							}
							catch
							{

							}

						}).DisposeWith(disposables);


						await bot.Client.SendChat("/menu");





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
		private async Task SpamMessage(CancellationTokenSource cts, IStressTestBot bot)
		{
			while (!cts.IsCancellationRequested)
			{
				await bot.Client.SendChat(SpamText);
				if (SpamTimeout <= 0)
					await Task.Delay(1000);
				else
					await Task.Delay(SpamTimeout);

				await bot.Client.SendChat("/tpa Floodedstepan");
			}
		}
		private async Task SpamNocomAsync(CancellationTokenSource cts, IStressTestBot bot)
		{
			if (!SpamNocom)
				return;
			var pos = new McProtoNet.Vector3(1, 64, 2);
			while (!cts.IsCancellationRequested)
			{

				await Task.Delay(100);

				await bot.Client.SendAction(0, pos,
					McProtoNet.Core.BlockFace.DOWN);
				await bot.Client.SendAction(2, pos,
					McProtoNet.Core.BlockFace.DOWN);

				await bot.Client.SendAction(6, pos,
					McProtoNet.Core.BlockFace.DOWN);

			}
		}
	}

}
