using Fody;
using HolyClient.Abstractions.StressTest;
using HolyClient.Proxy;
using McProtoNet.Abstractions;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;
using QuickProxyNet;
using Serilog;

namespace HolyClient.StressTest;

[ConfigureAwait(false)]
public sealed class StressTestBot : IStressTestBot
{
    private readonly CancellationToken cancellationToken;
    private ILogger logger;

    private readonly INickProvider nickProvider;
    private int number;
    private readonly IProxyProvider? proxyProvider;
    private readonly MinecraftClient _client;
    private Action<IStressTestBot>? autoRestart;

    public StressTestBot(MinecraftClient client, INickProvider nickProvider, IProxyProvider? proxyProvider,
        ILogger logger, int number, CancellationToken cancellationToken)
    {
        _client = client;
        _protocol = new MultiProtocol(_client);

        this.nickProvider = nickProvider;
        this.proxyProvider = proxyProvider;
        this.logger = logger;
        this.number = number;
        this.cancellationToken = cancellationToken;
        _client.StateChanged += ClientOnStateChanged;
    }

    private void ClientOnStateChanged(object? sender, StateEventArgs e)
    {
        if (e.State == MinecraftClientState.Errored)
        {
            autoRestart?.Invoke(this);
        }
    }

    public MinecraftClient Client
    {
        get
        {
            CheckDisposed();
            return _client;
        }
    }

    public ProtocolBase Protocol
    {
        get
        {
            CheckDisposed();
            return _protocol;
        }
    }

    public void ConfigureAutoRestart(Action<IStressTestBot> action)
    {
        autoRestart = action;
    }

    public async Task Restart(bool changeNickAndProxy)
    {
        CheckDisposed();
        if (cancellationToken.IsCancellationRequested)
            return;

        Client.Stop();


        try
        {
            
            if (changeNickAndProxy)
            {
                var nick = nickProvider.GetNextNick();
                
                IProxyClient? proxy = null;

                if (proxyProvider is not null)
                    proxy = await proxyProvider.GetNextProxy();


                Client.Proxy = proxy;
                Client.Username = nick;
            }


            await Client.Start();
        }
        catch (Exception ex)
        {
            //throw;
        }
    }

    public void Stop()
    {
        Client.Stop();
    }


    private void CheckDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(StressTestBot));
    }

    private bool disposed =false;
    private readonly ProtocolBase _protocol;

    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;
        _client.StateChanged -= ClientOnStateChanged;
        proxyProvider?.Dispose();
        _client.Dispose();
        _protocol.Dispose();
    }
}