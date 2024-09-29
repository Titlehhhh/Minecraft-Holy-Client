using System.Net;
using System.Runtime.CompilerServices;

namespace QuickProxyNet;

public class HttpsProxyClient : ProxyClient
{
    public HttpsProxyClient(string host, int port) : base("https", host, port)
    {
    }

    public HttpsProxyClient(string host, int port, NetworkCredential credentials) : base("https", host, port,
        credentials)
    {
    }

    public override ProxyType Type => ProxyType.HTTPS;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        var result =
            await ProxyConnector.ConnectToProxyAsync(stream, ProxyUri, host, port, ProxyCredentials, cancellationToken);
        return result;
    }
}