using Fody;
using HolyClient.Abstractions.StressTest;
using QuickProxyNet;
using Serilog;
using Serilog.Core;

namespace HolyClient.StressTest;

[ConfigureAwait(false)]
public sealed class StressTestBot : IStressTestBot
{
    private readonly CancellationToken cancellationToken;
    private ILogger logger;

    private readonly INickProvider nickProvider;
    private int number;
    private readonly IProxyProvider? proxyProvider;

    public StressTestBot(MinecraftClient client, INickProvider nickProvider, IProxyProvider? proxyProvider,
        ILogger logger, int number, CancellationToken cancellationToken)
    {
        Client = client;
        this.nickProvider = nickProvider;
        this.proxyProvider = proxyProvider;
        this.logger = logger;
        this.number = number;
        this.cancellationToken = cancellationToken;
    }

    public MinecraftClient Client { get; }


    public async Task Restart(bool changeNickAndProxy)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        Client.Disconnect();


        try
        {
            if (changeNickAndProxy)
            {
                IProxyClient? proxy = null;

                if (proxyProvider is not null)
                    proxy = await proxyProvider.GetNextProxy();


                Client.Config = new ClientConfig
                {
                    Host = Client.Config.Host,
                    Port = Client.Config.Port,
                    Username = nickProvider.GetNextNick(),
                    Version = Client.Config.Version,
                    Proxy = proxy,
                    HandshakeHost = Client.Config.HandshakeHost,
                    HandshakePort = Client.Config.HandshakePort
                };
            }

            Client.Start(Logger.None);
        }
        catch
        {
            //throw;
        }
    }
}