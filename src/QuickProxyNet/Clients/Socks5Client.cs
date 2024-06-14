using System.Net;

namespace QuickProxyNet;

public class Socks5Client : ProxyClient
{
    public Socks5Client(string host, int port) : base("socks5", host, port)
    {
    }

    public Socks5Client(string host, int port, NetworkCredential credentials) : base("socks5", host, port, credentials)
    {
    }

    public override ProxyType Type => ProxyType.SOCKS5;


    public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port,
        CancellationToken cancellationToken = default)
    {
        var result =
            await ProxyConnector.ConnectToProxyAsync(stream, ProxyUri, host, port, ProxyCredentials, cancellationToken);
        return result;
    }
}