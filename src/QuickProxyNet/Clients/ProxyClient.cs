using System.Net;
using System.Net.Sockets;

namespace QuickProxyNet;

public abstract class ProxyClient : IProxyClient
{
    protected ProxyClient(string protocol, string host, int port)
    {
        ProxyUri = new Uri($"{protocol}://{host}:{port}");

        if (host == null)
            throw new ArgumentNullException(nameof(host));

        if (host.Length == 0 || host.Length > 255)
            throw new ArgumentException("The length of the host name must be between 0 and 256 characters.",
                nameof(host));

        if (port < 0 || port > 65535)
            throw new ArgumentOutOfRangeException(nameof(port));

        ProxyHost = host;
        ProxyPort = port == 0 ? 1080 : port;
    }

    protected ProxyClient(string protocol, string host, int port, NetworkCredential credentials) : this(protocol, host,
        port)
    {
        if (credentials == null)
            throw new ArgumentNullException(nameof(credentials));

        ProxyCredentials = credentials;
    }

    public Uri ProxyUri { get; private set; }
    public abstract ProxyType Type { get; }

    public NetworkCredential? ProxyCredentials { get; }

    public string ProxyHost { get; }

    public int ProxyPort { get; }

    public IPEndPoint LocalEndPoint { get; set; }
    
    public int WriteTimeout { get; set; }
    public int ReadTimeout { get; set; }

    public async ValueTask<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true,
            LingerState = new LingerOption(true, 0),
            SendTimeout = this.WriteTimeout,
            ReceiveTimeout = this.ReadTimeout
        };
        try
        {
            await socket.ConnectAsync(ProxyHost, ProxyPort, cancellationToken);
        }
        catch
        {
            socket.Dispose();
            throw;
        }

        var stream = new NetworkStream(socket, true);
        try
        {
            await using var reg = cancellationToken.Register(s => ((IDisposable)s).Dispose(), stream);

            return await ConnectAsync(stream, host, port, cancellationToken);
        }
        catch
        {
            await stream.DisposeAsync();
            throw;
        }
    }

    public virtual async ValueTask<Stream> ConnectAsync(string host, int port, int timeout,
        CancellationToken cancellationToken = default)
    {
        ValidateArguments(host, port, timeout);

        cancellationToken.ThrowIfCancellationRequested();


        using var ts = new CancellationTokenSource(timeout);
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, ts.Token);
        return await ConnectAsync(host, port, linked.Token);
    }

    public abstract ValueTask<Stream> ConnectAsync(Stream source, string host, int port,
        CancellationToken cancellationToken = default);

    internal static void ValidateArguments(string host, int port)
    {
        if (host == null)
            throw new ArgumentNullException(nameof(host));

        if (host.Length == 0 || host.Length > 255)
            throw new ArgumentException("The length of the host name must be between 0 and 256 characters.",
                nameof(host));

        if (port <= 0 || port > 65535)
            throw new ArgumentOutOfRangeException(nameof(port));
    }

    private static void ValidateArguments(string host, int port, int timeout)
    {
        ValidateArguments(host, port);

        if (timeout < -1)
            throw new ArgumentOutOfRangeException(nameof(timeout));
    }
}