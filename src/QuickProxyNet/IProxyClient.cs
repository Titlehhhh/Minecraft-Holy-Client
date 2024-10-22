using System.Net;

namespace QuickProxyNet;

public interface IProxyClient
{
    NetworkCredential? ProxyCredentials { get; }

    string ProxyHost { get; }

    int ProxyPort { get; }

    ProxyType Type { get; }

    IPEndPoint LocalEndPoint { get; set; }

    int WriteTimeout { get; set; }
    int ReadTimeout { get; set; }


    ValueTask<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default);
    ValueTask<Stream> ConnectAsync(Stream source, string host, int port, CancellationToken cancellationToken = default);

    ValueTask<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default);
}