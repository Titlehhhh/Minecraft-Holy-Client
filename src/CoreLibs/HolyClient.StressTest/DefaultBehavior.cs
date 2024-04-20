using HolyClient.Abstractions.StressTest;
using McProtoNet;
using ReactiveUI;
using Serilog;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

		private IObservable<long> StaticSpam;

		public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger, CancellationToken cancellationToken)
		{


			StaticSpam = Observable.Interval(TimeSpan.FromMilliseconds(SpamTimeout), RxApp.TaskpoolScheduler);

			int gg = 0;


			foreach (var bot in bots)
			{
				CompositeDisposable disp = null;


				Action<Exception> onErr = async (exc) =>
				{
					
					

					try
					{
						if (disp is null)
						{
							disp = new();
						}
						else
						{
							disp.Dispose();
							disp = null;
						}
					}
					catch
					{

					}
					finally
					{

					}
					if (Reconnects <= 1)
					{
						try
						{
							await bot.Restart(true);
						}
						catch (Exception ex)
						{
							logger.Information($"Ошибка переподключения: {ex.Message}");
						}
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

				bot.Client.OnJoinGame.Subscribe(async x =>
				{
					return;
					try
					{
						if (disp is null)
							disp = new();
						else if (disp.IsDisposed)
							disp = new();

						await Task.Delay(1000);
						await bot.Client.SendChat("/register 21qwerty123 21qwerty123");

						SpamMessage(disp, bot);
					}
					catch { }
				}).DisposeWith(disposables);

				bot.Client.OnChatMessage.Subscribe(async x =>
				{
					return;
					var ch = ChatParser.ParseText(x.Message);
					if (ch.Contains("/register") || ch.Contains("/reg"))
					{
						try
						{
							await bot.Client.SendChat("/register 21qwerty123 21qwerty123");
							return;
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
									if (disp is null)
										disp = new();

									SpamMessage(disp, bot);

								}
								catch (Exception ex)
								{
									logger.Error(ex, "Start spam error");
								}

							}).DisposeWith(disposables);
							bot.Client.SendChat("/menu");

						}
						catch { }
					}


				}).DisposeWith(disposables);

				bot.Client.OnPlayerPositionRotation.Subscribe(x =>
				{

				}).DisposeWith(disposables);

				bot.Client.OnMapData.Subscribe(x =>
				{
					//logger.Information("map");
				}).DisposeWith(disposables);

				disposables.Add(Disposable.Create(() =>
				{
					bot.Client.OnErrored -= onErr;
				}));




				_ = bot.Restart(true);

			}
			return Task.CompletedTask;
		}
		private void SpamMessage(CompositeDisposable d, IStressTestBot bot)
		{
			StaticSpam.Subscribe(async x =>
			{
				try
				{
					var spamText = SpamText + " " + Random.Shared.NextInt64();

					await bot.Client.SendChat(spamText);
				}
				catch
				{

				}

			}).DisposeWith(d);

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
