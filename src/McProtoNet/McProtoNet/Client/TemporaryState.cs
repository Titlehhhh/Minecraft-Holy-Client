using System.Net.Sockets;
using System.Runtime.CompilerServices;
using DotNext.Threading;
using McProtoNet.Protocol;

namespace McProtoNet.Client;

internal sealed class TemporaryState : IDisposable
{
    private Stream mainStream;
    private TcpClient? _tcpClient;
    private AsyncReaderWriterLock _sendLock;
    private MinecraftPacketSender _packetSender;
    private CancellationTokenSource _cts;


    private int _state;

    private const int Actived = 0;
    private const int Deactived = 1;
    private const int Disposed = 2;

    public bool IsDisposed => Volatile.Read(_state) == Disposed;

    #region Properties

    public Stream MainStream
    {
        get
        {
            CheckState();
            var stream = Volatile.Read(ref mainStream);
            if (stream is null)
                throw new NullReferenceException($"{nameof(MainStream)} is null");
            return stream;
        }
        set
        {
            CheckState();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange<Stream>(ref mainStream, value, null);
            ThrowIfNotNull(old is not null, nameof(MainStream));
        }
    }

    public MinecraftPacketSender PacketSender
    {
        get
        {
            CheckState();
            var old = Volatile.Read(ref _packetSender);
            ThrowNullReference(old, nameof(PacketSender));
            return old;
        }
        set
        {
            CheckState();
            ArgumentNullException.ThrowIfNull(value, nameof(PacketSender));
            var old = Interlocked.CompareExchange<MinecraftPacketSender>(ref _packetSender, value, null);
            ThrowIfNotNull(old is not null, nameof(PacketSender));
        }
    }


    public TcpClient? TcpClient
    {
        get
        {
            CheckState();
            return Volatile.Read(ref _tcpClient);
        }
        set
        {
            CheckState();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange(ref _tcpClient, value, null);
            ThrowIfNotNull(old, nameof(TcpClient));
        }
    }

    public AsyncReaderWriterLock SendLock
    {
        get
        {
            CheckState();
            var old = Volatile.Read(ref _sendLock);
            ThrowNullReference(old, nameof(SendLock));
            return old;
        }
        set
        {
            CheckState();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange(ref _sendLock, value, null);
            ThrowIfNotNull(old, nameof(SendLock));
        }
    }

    public CancellationTokenSource CancellationTokenSource
    {
        get
        {
            CheckState();
            var old = Volatile.Read(ref _cts);
            ThrowNullReference(old, nameof(CancellationTokenSource));
            return old;
        }
        set
        {
            CheckState();
            ArgumentNullException.ThrowIfNull(value);
            var old = Interlocked.CompareExchange(ref _cts, value, null);
            ThrowIfNotNull(old, nameof(CancellationTokenSource));
        }
    }

    #endregion


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckState()
    {
        var state = Volatile.Read(ref _state);
        ObjectDisposedException.ThrowIf(state == Disposed, this);
        if (state == Deactived)
            throw new InvalidOperationException("State deactivated");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void ThrowIfNotNull(object value, string propName)
    {
        if (value is not null)
            throw new InvalidOperationException($"The value of property {propName} has not been deleted");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void ThrowNullReference(object value, string propName)
    {
        if (value is null)
            throw new NullReferenceException($"{propName} is null");
    }

    public void Active()
    {
        int old = Interlocked.CompareExchange(ref _state, Actived, Deactived);
        ObjectDisposedException.ThrowIf(old == Disposed, this);
        if (old == Actived)
        {
            throw new InvalidOperationException("The state has already been activated.");
        }
    }

    public void Deactive()
    {
        int old = Interlocked.CompareExchange(ref _state, Deactived, Actived);
        ObjectDisposedException.ThrowIf(old == Disposed, this);
        if (old == Deactived)
        {
            throw new InvalidOperationException("The state has already been deactivated.");
        }

        Clear();
    }

    private void Clear()
    {
        Interlocked.Exchange(ref mainStream, null)?.Dispose();
        Interlocked.Exchange(ref _tcpClient, null)?.Dispose();
        Interlocked.Exchange(ref _sendLock, null)?.Dispose();
        CancellationTokenSource oldCts = Interlocked.Exchange(ref _cts, null);
        if (oldCts is not null)
        {
            oldCts.Cancel();
            oldCts.Dispose();
        }
    }

    private void CheckDisposed()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _state, Disposed) != Disposed)
        {
            Clear();
        }
    }
}