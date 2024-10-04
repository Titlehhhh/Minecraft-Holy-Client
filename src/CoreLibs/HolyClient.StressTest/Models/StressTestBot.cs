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
    private CancellationToken cancellationToken;
    private ILogger logger;

    private  INickProvider nickProvider;
    private int number;
    private  IProxyProvider? proxyProvider;
    private  MinecraftClient _client;
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
        _client.Disconnected += ClientOnDisconnected;
    }

    private void ClientOnDisconnected(object? sender, DisconnectedEventArgs e)
    {
        if (e.Exception is not null)
        {
            autoRestart?.Invoke(this);
        }
    }

    private void ClientOnStateChanged(object? sender, StateEventArgs e)
    {
    }

    public MinecraftClient Client
    {
        get
        {
            ThrowIfDisposed();
            return _client;
        }
    }

    public ProtocolBase Protocol
    {
        get
        {
            ThrowIfDisposed();
            return _protocol;
        }
    }

    public void ConfigureAutoRestart(Action<IStressTestBot> action)
    {
        autoRestart = action;
    }

    public async Task Restart(bool changeNickAndProxy)
    {
        ThrowIfDisposed();
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
        ThrowIfDisposed();
        Client.Stop();
    }


    private void ThrowIfDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(StressTestBot));
    }

    private bool disposed = false;
    private ProtocolBase _protocol;

    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;
        _client.StateChanged -= ClientOnStateChanged;
        _client.Disconnected -= ClientOnDisconnected;
        proxyProvider?.Dispose();
        _client.Dispose();
        _protocol.Dispose();
        _client = null;
        _protocol = null;
        this.nickProvider = null;
        this.logger = null;
        autoRestart = null;
    }
}