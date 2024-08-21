using System.Net;
using System.Runtime.CompilerServices;

namespace QuickProxyNet;

public class HttpProxyClient : ProxyClient
{
    public HttpProxyClient(string host, int port) : base("http", host, port)
    {
    }

    public HttpProxyClient(string host, int port, NetworkCredential credentials) : base("http", host, port, credentials)
    {
    }

    public override ProxyType Type => ProxyType.HTTPS;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port,
        CancellationToken cancellationToken = default)
    {
        var result =
            await ProxyConnector.ConnectToProxyAsync(stream, ProxyUri, host, port, ProxyCredentials, cancellationToken);
        return result;
    }
}