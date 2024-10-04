using System.Diagnostics;
using System.Net.Sockets;
using DotNext;
using DotNext.Threading;
using McProtoNet.Abstractions;
using McProtoNet.Protocol;
using QuickProxyNet;

namespace McProtoNet.Client;

public sealed class MinecraftClient : Disposable, IPacketBroker
{
#if NET9_0_OR_GREATER
    private readonly System.Threading.Lock _gate = new();
#else
    private readonly object _gate = new();
#endif
    private volatile int _state;

    private volatile int _isActive;

    private bool IsActive => _isActive == 1;

    private volatile CancellationTokenSource _aliveClient;

    private volatile AsyncReaderWriterLock _sendLock;
    private volatile MinecraftPacketSender _packetSender;
    private volatile Stream mainStream;

    private readonly MinecraftClientLogin minecraftLogin = new();


    public MinecraftClient()
    {
        minecraftLogin.StateChanged += this.MinecraftLogin_StateChanged;
    }

    public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(3);

    public int ReadTimeout { get; set; } = 30_000;
    public int WriteTimeout { get; set; } = 30_000;

    #region Public API

    public async ValueTask SendPacket(ReadOnlyMemory<byte> data)
    {
        ThrowIfDisposed();
        ThrowIfNotPlay();
        var holder = await _sendLock.AcquireWriteLockAsync(_aliveClient.Token);
        try
        {
            await _packetSender.SendPacketAsync(data, _aliveClient.Token);
        }
        finally
        {
            holder.Dispose();
        }
    }

    public async Task Start(bool throwErrorConnect = false)
    {
        ThrowIfDisposed();
        ThrowIfConnected();


        Validate();

        if (CompareExchangeState(MinecraftClientState.Connect, MinecraftClientState.Disconnected) !=
            MinecraftClientState.Disconnected)
        {
            throw new InvalidOperationException("Not allowed to connect while connect/disconnect is pending.");
        }

        RaiseStateChanged(MinecraftClientState.Disconnected, MinecraftClientState.Connect);

        try
        {
            CleanUp();
            var newCts = new CancellationTokenSource();
            _aliveClient = newCts;

            Stream tcpStream = null;
            using (var timeCts = CancellationTokenSource.CreateLinkedTokenSource(newCts.Token))
            {
                try
                {
                    timeCts.CancelAfter(ConnectTimeout);
                    tcpStream = await ConnectAsync(timeCts.Token);
                }
                catch (OperationCanceledException)
                {
                    throw new ConnectTimeoutException(
                        $"Failed to connect to {Host}:{Port} in the specified time: {ConnectTimeout} ms");
                }
            }

            mainStream = tcpStream;
            Debug.Assert(mainStream is not null, $"mainStream is not null?");
            var loginOptions = new LoginOptions(Host, Port, Version, Username);


            var result = await minecraftLogin.Login(tcpStream, loginOptions, newCts.Token);
            _sendLock = new AsyncReaderWriterLock();
            mainStream = result.Stream;

            Task mainLoop = MainLoop(result, newCts.Token);
        }
        catch (Exception ex)
        {
            if (DisconnectIsPendingOrFinished(out var previus))
            {
                return;
            }

            TryInitiateDisconnect();
            CleanUp();
            CompareExchangeState(MinecraftClientState.Disconnected, MinecraftClientState.Disconnecting);

            RaiseDisconnected(ex, previus);
        }
    }

    private bool DisconnectIsPendingOrFinished(out MinecraftClientState old)
    {
        var status = (MinecraftClientState)_state;
        do
        {
            switch (status)
            {
                case MinecraftClientState.Disconnecting:
                case MinecraftClientState.Disconnected:
                    old = status;
                    return true;
                case MinecraftClientState.Connect:
                case MinecraftClientState.Login:
                case MinecraftClientState.Handshaking:
                case MinecraftClientState.Play:
                    var curStatus = CompareExchangeState(MinecraftClientState.Disconnecting, status);
                    if (curStatus == status)
                    {
                        old = curStatus;
                        return false;
                    }

                    status = curStatus;
                    break;
            }
        } while (true);
    }

    public void Stop(Exception? customException = null)
    {
        ThrowIfDisposed();

        if (DisconnectIsPendingOrFinished(out var previus))
        {
            return;
        }

        TryInitiateDisconnect();
        CleanUp();
        CompareExchangeState(MinecraftClientState.Disconnected, MinecraftClientState.Disconnecting);
        RaiseDisconnected(customException, previus);
    }

    #endregion

    #region ValidateState

    private void ThrowIfConnected()
    {
        var state = (MinecraftClientState)_state;
        switch (state)
        {
            case MinecraftClientState.Login:
            case MinecraftClientState.Handshaking:
            case MinecraftClientState.Play:
                throw new InvalidOperationException(
                    "It is not allowed to connect with a server after the connection is established.");
        }
    }

    private void ThrowIfNotPlay()
    {
        if (_state != (int)MinecraftClientState.Play)
        {
            throw new InvalidOperationException("Cannot send packages outside of Play mode");
        }
    }

    #endregion


    private void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Host, nameof(Host));
        ArgumentException.ThrowIfNullOrWhiteSpace(Username, nameof(Username));

        if (!(Version >= MinVersionSupport && Version <= MaxVersionSupport))
            throw new ArgumentException($"Version {Version} not supported");
    }

    private void MinecraftLogin_StateChanged(MinecraftClientState state)
    {
        ThrowIfDisposed();
        var curState = (MinecraftClientState)_state;
        switch (curState)
        {
            case MinecraftClientState.Disconnected:
            case MinecraftClientState.Disconnecting:
                return;
        }

        if (curState == MinecraftClientState.Play)
        {
            throw new InvalidOperationException("The login status changes in Play mode");
        }

        var prev = state switch
        {
            MinecraftClientState.Handshaking => MinecraftClientState.Connect,
            MinecraftClientState.Login => MinecraftClientState.Handshaking,
        };
        var old = CompareExchangeState(state, prev);
        if (old == prev)
        {
            RaiseStateChanged(prev, state);
        }
        else
        {
            Debug.Assert(false, $"Current: {state} Prev: {prev}");
        }
    }


    void TryInitiateDisconnect()
    {
        lock (_gate)
        {
            try
            {
                _aliveClient?.Cancel(false);
            }
            catch
            {
                // ignored
            }
        }
    }

    private MinecraftClientState ExchangeState(MinecraftClientState newVal)
    {
        return (MinecraftClientState)Interlocked.Exchange(ref _state, (int)newVal);
    }

    private MinecraftClientState CompareExchangeState(MinecraftClientState val, MinecraftClientState comparand)
    {
        return (MinecraftClientState)Interlocked.CompareExchange(ref _state, (int)val, (int)comparand);
    }

    private void RaiseStateChanged(MinecraftClientState oldState, MinecraftClientState newState)
    {
        StateChanged?.Invoke(this, new StateEventArgs(oldState, newState));
    }

    private void RaiseDisconnected(Exception? ex, MinecraftClientState previousState)
    {
        Disconnected?.Invoke(this, new DisconnectedEventArgs(ex, previousState));
    }


    private async Task MainLoop(LoginizationResult loginizationResult, CancellationToken cancellationToken)
    {
        var packetSender = new MinecraftPacketSender();
        var packetReader = new MinecraftPacketReader();


        packetReader.BaseStream = loginizationResult.Stream;
        packetSender.BaseStream = loginizationResult.Stream;

        packetSender.SwitchCompression(loginizationResult.CompressionThreshold);

        _packetSender = packetSender;

        packetReader.SwitchCompression(loginizationResult.CompressionThreshold);
        try
        {
            MinecraftClientState old = // Enable SendPacket
                ExchangeState(MinecraftClientState.Play);

            RaiseStateChanged(old, MinecraftClientState.Play);


            while (!cancellationToken.IsCancellationRequested)
            {
                var packet = await packetReader
                    .ReadNextPacketAsync(cancellationToken);
                try
                {
                    PacketReceived?.Invoke(this, packet);
                }
                finally
                {
                    packet.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            if (DisconnectIsPendingOrFinished(out _))
            {
                return;
            }

            TryInitiateDisconnect();
            CleanUp();
            CompareExchangeState(MinecraftClientState.Disconnected, MinecraftClientState.Disconnecting);
            RaiseDisconnected(ex, MinecraftClientState.Play);
        }
        finally
        {
            packetReader.BaseStream = null;
        }
    }


    private async ValueTask<Stream> ConnectAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IDisposable? disposable = null;
        try
        {
            if (Proxy is not null)
            {
                Proxy.ReadTimeout = this.ReadTimeout;
                Proxy.WriteTimeout = this.WriteTimeout;
                return await Proxy.ConnectAsync(Host, Port, cancellationToken);
            }
            else
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    LingerState = new LingerOption(true, 0),
                    SendTimeout = this.WriteTimeout,
                    ReceiveTimeout = this.ReadTimeout
                };
                using (cancellationToken.Register(d => ((IDisposable)d).Dispose(), socket))
                {
                    disposable = socket;
                    await socket.ConnectAsync(Host, Port, cancellationToken).ConfigureAwait(false);

                    return new NetworkStream(socket, true);
                }
            }
        }
        catch
        {
            disposable?.Dispose();
            throw;
        }
    }


    private void CleanUp()
    {
        try
        {
            _aliveClient?.Cancel(false);
        }
        finally
        {
            _aliveClient?.Dispose();
            _aliveClient = null;

            mainStream?.Dispose();
            mainStream = null;


            if (_packetSender is not null)
            {
                _packetSender.BaseStream = null;
                _packetSender = null;
            }

            _sendLock?.Dispose();
            _sendLock = null;
        }
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed || IsDisposing, this);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CleanUp();
            minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;
            Disposed?.Invoke();
        }

        base.Dispose(disposing);
    }

    #region Events

    public event PacketHandler? PacketReceived;
    public event EventHandler<StateEventArgs> StateChanged;
    public event EventHandler<DisconnectedEventArgs> Disconnected;

    public event Action? Disposed;

    #endregion

    #region Properties

    int IPacketBroker.ProtocolVersion => this.Version;
    public string Host { get; set; }
    public ushort Port { get; set; } = 25565;

    public string Username { get; set; }
    public int Version { get; set; }
    public IProxyClient? Proxy { get; set; }

    #endregion


    #region Constans

    public const int MinVersionSupport = 340;
    public static readonly int MaxVersionSupport = MinecraftVersion.Latest;

    #endregion
}