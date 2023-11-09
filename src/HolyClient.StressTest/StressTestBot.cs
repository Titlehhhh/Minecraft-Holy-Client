using Fody;
using HolyClient.Abstractions.StressTest;
using McProtoNet;
using QuickProxyNet;
using Serilog;
using Serilog.Core;
using System.Reactive.Subjects;

namespace HolyClient.StressTest
{
	[ConfigureAwait(false)]
	public sealed class StressTestBot : IStressTestBot
	{
		public MinecraftClient Client { get; }

		private INickProvider nickProvider;
		private IProxyProvider? proxyProvider;
		private ILogger logger;
		private int number;
		private CancellationToken cancellationToken;
		public StressTestBot(MinecraftClient client, INickProvider nickProvider, IProxyProvider? proxyProvider, ILogger logger, int number, CancellationToken cancellationToken)
		{
			Client = client;
			this.nickProvider = nickProvider;
			this.proxyProvider = proxyProvider;
			this.logger = logger;
			this.number = number;
			this.cancellationToken = cancellationToken;
		}

		private Subject<Exception> _onError = new();

		public IObservable<Exception> OnError => _onError;

		public async Task Restart(bool changeNickAndProxy)
		{
			if (cancellationToken.IsCancellationRequested)
				return;
			
			try
			{
				Client.Disconnect();
				if (changeNickAndProxy)
				{
					IProxyClient? proxy = null;

					if (proxyProvider is not null)
						proxy = await this.proxyProvider.GetNextProxy();


					this.Client.Config = new ClientConfig
					{
						Host = Client.Config.Host,
						Port = Client.Config.Port,
						Username = this.nickProvider.GetNextNick(),
						Version = Client.Config.Version,
						Proxy = proxy
					};
				}

				await Client.Login(Logger.None);

			}
			catch (Exception ex)
			{
				_onError.OnNext(ex);
				
			}
		}
	}

}
