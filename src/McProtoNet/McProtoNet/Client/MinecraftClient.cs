using System.Net.Sockets;
using System.Runtime.CompilerServices;
using DotNext;
using DotNext.Threading;
using Fody;
using McProtoNet.Abstractions;
using McProtoNet.Protocol;
using QuickProxyNet;

namespace McProtoNet.Client;

public struct MinecraftVersion
{
    public readonly static int Latest = 767;

    public int ProtocolVersion { get; }
    public string MajorVersion { get; }
    public string MinorVersion { get; }
}
// Thread safety temporary state for MinecraftClient
internal sealed class TemporaryState
{
    

    private Stream mainStream;
    private TcpClient? _tcpClient;
    private AsyncReaderWriterLock _sendLock;
    private MinecraftPacketSender _packetSender;


    private int _state;

    private const int Reset = 0;
    private const int Disposed = 1;

    public bool IsDisposed => Volatile.Read(_state) == Disposed;

    public Stream MainStream
    {
        get
        {
            CheckDispose();
            var stream = Volatile.Read(ref mainStream);
            if (stream is null)
                throw new NullReferenceException($"{nameof(MainStream)} is null");
            return stream;
        }
        set
        {
            CheckDispose();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange<Stream>(ref mainStream, value, null);
            ThrowStateNotResetIfNotNull(old is not null, nameof(MainStream));
        }
    }

    public MinecraftPacketSender PacketSender
    {
        get
        {
            CheckDispose();
            var old = Volatile.Read(ref _packetSender);
            if (old is null)
                throw new NullReferenceException($"{nameof(PacketSender)} is null");
            return old;
        }
        set
        {
            CheckDispose();
            ArgumentNullException.ThrowIfNull(value, nameof(PacketSender));
            var old = Interlocked.CompareExchange<MinecraftPacketSender>(ref _packetSender, value, null);
            ThrowStateNotResetIfNotNull(old is not null, nameof(PacketSender));
        }
    }


    public TcpClient? TcpClient
    {
        get
        {
            CheckDispose();
            return Volatile.Read(ref _tcpClient);
        }
        set
        {
            CheckDispose();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange(ref _tcpClient, value, null);
            ThrowStateNotResetIfNotNull(old is not null, nameof(TcpClient));
        }
    }

    public AsyncReaderWriterLock SendLock
    {
        get
        {
            CheckDispose();
            var old = Volatile.Read(ref _sendLock);
            if (old is null)
                throw new NullReferenceException($"{nameof(SendLock)} is null");
            return old;
        }
        set
        {
            CheckDispose();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange(ref _sendLock, value, null);
            ThrowStateNotResetIfNotNull(old is not null, nameof(SendLock));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckDispose()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void ThrowStateNotResetIfNotNull(bool notNull, string propName)
    {
        if (notNull)
            throw new InvalidOperationException($"State not reset. Property {propName}");
    }

    public void ResetState()
    {
        int old = Interlocked.CompareExchange(ref _state, Reset, Disposed);
        if (old == Reset)
        {
            throw new InvalidOperationException("The state has already been reset.");
        }
    }

    public void DisposeAll()
    {
        int old = Interlocked.CompareExchange(ref _state, Disposed, Reset);

        ObjectDisposedException.ThrowIf(old == Disposed, this);

        Interlocked.Exchange(ref mainStream, null)?.Dispose();
        Interlocked.Exchange(ref _tcpClient, null)?.Dispose();
        Interlocked.Exchange(ref _sendLock, null)?.Dispose();
    }
}

public sealed class MinecraftClient : Disposable, IPacketBroker
{
    #region Thread-safety fields

    private MinecraftPacketSender _packetSender;
    private Stream mainStream;
    private CancellationTokenSource CTS;
    private TcpClient tcpClient;
    private volatile MinecraftClientState _state;
    private readonly AsyncLock sendLock;

    private bool NeedThrowException => throw new NotImplementedException();

    #endregion

    private readonly MinecraftClientLogin minecraftLogin = new();


    public MinecraftClient()
    {
        minecraftLogin.StateChanged += MinecraftLogin_StateChanged;
        var rw = new AsyncReaderWriterLock();

        sendLock = AsyncLock.WriteLock(rw);
    }

    public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(3);

    public int ReadTimeout { get; set; } = 30_000;
    public int WriteTimeout { get; set; } = 30_000;

    #region Public API

    public async ValueTask SendPacket(ReadOnlyMemory<byte> data)
    {
        CheckSend();
        var tempLock = sendLock;
        var tempSender = _packetSender;
        CancellationTokenSource cts = this.CTS;
        var holder = await tempLock.AcquireAsync(cts.Token);
        try
        {
            await tempSender.SendPacketAsync(data, cts.Token);
        }
        catch (Exception ex)
        {
            OnException(ex);
            throw;
        }
        finally
        {
            holder.Dispose();
        }
    }


    public async Task Start()
    {
        Validate();
        if (!TrySetConnect(out var error))
        {
            throw new InvalidOperationException(error);
        }

        var newCts = new CancellationTokenSource();
        Volatile.Write(ref CTS, newCts);
        try
        {
            using (var timeCts = CancellationTokenSource.CreateLinkedTokenSource(newCts.Token))
            {
                try
                {
                    timeCts.CancelAfter(ConnectTimeout);
                    var newStream = await ConnectAsync(timeCts.Token);
                    Volatile.Write(ref mainStream, newStream);
                }
                catch (OperationCanceledException) when (timeCts.IsCancellationRequested)
                {
                    throw new ConnectTimeoutException(
                        $"Failed to connect to {Host}:{Port} in the specified time: {ConnectTimeout} ms");
                }
            }

            var loginOptions = new LoginOptions(Host, Port, Version, Username);


            var result = await minecraftLogin.Login(mainStream, loginOptions, CTS.Token);

            //StateChange(MinecraftClientState.Play);

            Task mainLoop = MainLoop(result, CTS.Token);
        }
        catch (OperationCanceledException ex) when (newCts.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            OnException(ex);
            throw;
        }
    }

    public void Stop(Exception? customException = null)
    {
        if (customException is not null)
        {
            OnException(customException);
        }
        else
        {
        }
    }

    #endregion

    #region ValidateState

    private bool TrySetConnect(out string errorMessage)
    {
        // TODO
        errorMessage = "client already run";
        return false;
    }

    private void CheckSend()
    {
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
        //StateChange(state);
    }

    private bool TrySetErroredState()
    {
        throw new NotImplementedException();
    }

    private void RaiseStateChanged(MinecraftClientState oldState, MinecraftClientState newState)
    {
        StateChanged?.Invoke(this, new StateEventArgs(oldState, newState));
    }

    private void RaiseStateChanged(Exception error, MinecraftClientState oldState)
    {
        StateChanged?.Invoke(this, new StateEventArgs(error, oldState));
    }


    private void OnException(Exception ex)
    {
        if (TrySetErroredState())
        {
        }


        CleanUp();
    }

    private async Task MainLoop(LoginizationResult loginizationResult, CancellationToken cancellationToken)
    {
        var packetSender = new MinecraftPacketSender();
        var packetReader = new MinecraftPacketReader();


        packetReader.BaseStream = loginizationResult.Stream;
        _packetSender.BaseStream = loginizationResult.Stream;

        _packetSender.SwitchCompression(loginizationResult.CompressionThreshold);

        Volatile.Write(ref _packetSender, packetSender);

        packetReader.SwitchCompression(loginizationResult.CompressionThreshold);

        try
        {
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
            OnException(ex);
            throw;
        }
    }


    private async ValueTask<Stream> ConnectAsync(CancellationToken cancellationToken)
    {
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
                await using (cancellationToken.Register(d => ((IDisposable)d).Dispose(), socket))
                {
                    disposable = socket;
                    await socket.ConnectAsync(Host, Port, cancellationToken);

                    return new NetworkStream(socket);
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
        minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;
        if (CTS is not null)
        {
            CTS.Cancel();
            CTS.Dispose();
            CTS = null;
        }

        if (tcpClient is not null)
        {
            tcpClient.Dispose();
            tcpClient = null;
        }

        if (mainStream is not null)
        {
            mainStream.Dispose();
            mainStream = null;
        }
    }


    protected override void Dispose(bool disposing)
    {
        CleanUp();
        minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;
        Disposed?.Invoke();


        GC.SuppressFinalize(this);
        base.Dispose(disposing);
    }

    #region Events

    public event PacketHandler? PacketReceived;
    public event EventHandler<StateEventArgs> StateChanged;
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

internal struct CompletionState
{
    private int startStopping;

    public void SetStop()
    {
    }

    public MinecraftClientState SetState(MinecraftClientState newState)
    {
        if (IsDisposed)
            ThrowObjectDiposedException();
        int newVal = (int)newState;
        throw new NotImplementedException();
    }

    static void ThrowObjectDiposedException()
    {
        throw new ObjectDisposedException("");
    }

    public void SetException()
    {
    }

    #region States

    private static int STOPPED_STATE = 0;
    private static int CONNECT_STATE = 1;
    private static int HANDSHAKING_STATE = 2;
    private static int LOGIN_STATE = 3;
    private static int PLAY_STATE = 4;
    private static int ERRORED_STATE = 5;
    private static int Disposed;

    #endregion

    private int _disposed;

    public bool IsDisposed => Volatile.Read(ref _disposed) == 1;

    public bool TrySetDisposed()
    {
        return Interlocked.Exchange(ref _disposed, Disposed) == 0;
    }
}