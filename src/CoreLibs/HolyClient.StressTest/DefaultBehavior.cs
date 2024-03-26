using HolyClient.Abstractions.StressTest;
using McProtoNet;
using Serilog;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HolyClient.StressTest
{
	public class DefaultBehavior : BaseStressTestBehavior
	{
		[System.ComponentModel.DisplayName("Spam text")]
		public string SpamText { get; set; } = "!Spam Spam Spam";

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
			Subject<Unit> spamEvent = new();
			CancellationTokenSource loopCts = new();

			Task.Run(async () =>
			{
				try
				{
					while (!loopCts.IsCancellationRequested)
					{
						await Task.Delay(SpamTimeout, loopCts.Token);
						spamEvent.OnNext(default);
					}
				}
				catch
				{

				}
			});

			loopCts.DisposeWith(disposables);
			spamEvent.DisposeWith(disposables);

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

				bot.Client.OnChatMessage.Subscribe(x =>
				{
					var ch = ChatParser.ParseText(x.Message);
					if (ch.Contains("/register"))
					{
						try
						{
							bot.Client.SendChat("/register 21qwerty123 21qwerty123");


						}
						catch { }
					}
					else if (ch.Contains("nice game"))
					{
						try
						{


							IDisposable d = null;
							d = bot.Client.OnOpenWindow.Subscribe(async x =>
							{
								d?.Dispose();
								logger.Debug("menu: " + x.Id);



								await bot.Client.SendPacket(w =>
								{
									w.WriteUnsignedByte((byte)x.Id);

									w.WriteShort(5);

									w.WriteByte(0);
									w.WriteShort(0);

									w.WriteVarInt(0);
									w.WriteBoolean(false);

								}, McProtoNet.PacketOut.ClickWindow);

								await Task.Delay(1000);

								try
								{
									if (cts is null)
										cts = new();

									SpamMessage(cts, bot, spamEvent);
									
								}
								catch (Exception ex)
								{
									logger.Error(ex, "Start spam error");
								}

							}).DisposeWith(disposables);
							bot.Client.SendChat("/menu");
						}
						catch { }
					} else if(ch.Contains("tempbanned") || ch.Contains("tempmuted"))
					{
						if (ch.Contains(bot.Client.Config.Username))
						{
							bot.Client.Disconnect(new Exception());
						}
					}

				}).DisposeWith(disposables);

				disposables.Add(Disposable.Create(() =>
				{
					bot.Client.OnErrored -= onErr;
				}));




				_ = bot.Restart(true);

			}
			return Task.CompletedTask;
		}
		private void SpamMessage(CancellationTokenSource cts, IStressTestBot bot, IObservable<Unit> observable)
		{
			IDisposable d = observable.Subscribe(async x =>
			{
				var spamText = SpamText + " " + Random.Shared.NextInt64();

				await bot.Client.SendChat(spamText);
			});
			cts.Token.Register(d.Dispose);
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
