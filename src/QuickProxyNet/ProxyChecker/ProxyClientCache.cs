using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace QuickProxyNet.ProxyChecker;

internal class ProxyClientCache : IProxyClient
{
    private IProxyClient _client;
    private Stream _stream;
    private long timeAdded;
    private readonly Task<Stream> cacheResult;
    private readonly string host;
    private readonly int port;
    private Action onDisposed;
    private bool disposed;

    public NetworkCredential? ProxyCredentials => _client.ProxyCredentials;

    public string ProxyHost => _client.ProxyHost;

    public int ProxyPort => _client.ProxyPort;

    public ProxyType Type => _client.Type;

    public IPEndPoint LocalEndPoint
    {
        get => _client.LocalEndPoint;
        set => _client.LocalEndPoint = value;
    }

    public int WriteTimeout
    {
        get => _client.WriteTimeout;
        set => _client.WriteTimeout = value;
    }

    public int ReadTimeout
    {
        get => _client.ReadTimeout;
        set => _client.ReadTimeout = value;
    }

    public ProxyClientCache(IProxyClient client, Stream stream, Action onDisposed)
    {
        _client = client;
        _stream = stream;
        this.onDisposed = onDisposed;
        onDisposed += OnDisposed;
        cacheResult = Task.FromResult(_stream);
        timeAdded = Stopwatch.GetTimestamp();
    }

    private void OnDisposed()
    {
        if (onDisposed is not null)
        {
            onDisposed -= OnDisposed;
        }

        _stream.Dispose();
        _stream = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckTime()
    {
        TimeSpan time = Stopwatch.GetElapsedTime(timeAdded);
        return time.TotalSeconds <= 10;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CheckHostPort(string host, int port)
    {
        return this.host == host && this.port == port;
    }

    public Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        if (CheckHostPort(host, port) && CheckTime())
        {
            return cacheResult;
        }
        else
        {
            _stream.Dispose();
            return _client.ConnectAsync(host, port, cancellationToken);
        }
    }

    public ValueTask<Stream> ConnectAsync(Stream source, string host, int port,
        CancellationToken cancellationToken = default)
    {
        if (CheckHostPort(host, port) && CheckTime())
        {
            return ValueTask.FromResult(_stream);
        }
        else
        {
            _stream.Dispose();
            return _client.ConnectAsync(source, host, port, cancellationToken);
        }
    }

    public Task<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default)
    {
        if (CheckHostPort(host, port) && CheckTime())
        {
            return Task.FromResult(_stream);
        }
        else
        {
            _stream.Dispose();
            return _client.ConnectAsync(host, port, timeout, cancellationToken);
        }
    }
}