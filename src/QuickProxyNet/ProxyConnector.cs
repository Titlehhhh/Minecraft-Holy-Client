using System.Net;

namespace QuickProxyNet;

internal static class ProxyConnector
{
    public static async ValueTask<Stream> ConnectToProxyAsync(Stream stream, Uri proxyUri, string host, int port,
        NetworkCredential? proxyCredentials, CancellationToken cancellationToken)
    {
        await using (cancellationToken.Register(s => ((Stream)s!).Dispose(), stream))
        {
            try
            {
                var credentials = proxyCredentials?.GetCredential(proxyUri, proxyUri.Scheme);

                if (string.Equals(proxyUri.Scheme, "socks5", StringComparison.OrdinalIgnoreCase))
                {
                    await SocksHelper.EstablishSocks5TunnelAsync(stream, host, port, credentials, cancellationToken)
                        .ConfigureAwait(false);
                    return stream;
                }

                if (string.Equals(proxyUri.Scheme, "socks4a", StringComparison.OrdinalIgnoreCase))
                {
                    await SocksHelper
                        .EstablishSocks4TunnelAsync(stream, true, host, port, credentials, cancellationToken)
                        .ConfigureAwait(false);
                    return stream;
                }

                if (string.Equals(proxyUri.Scheme, "socks4", StringComparison.OrdinalIgnoreCase))
                {
                    await SocksHelper
                        .EstablishSocks4TunnelAsync(stream, false, host, port, credentials, cancellationToken)
                        .ConfigureAwait(false);
                    return stream;
                }

                if (string.Equals(proxyUri.Scheme, "http", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(proxyUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
                {
                    var result = await HttpHelper.EstablishHttpTunnelAsync(stream, proxyUri, host, port, credentials,
                        cancellationToken);
                    return result;
                }

                throw new NotSupportedException("Bad protocol");
            }
            catch
            {
                await stream.DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }
    }
}