using System.Diagnostics;
using System.IO;
using System.Net;

namespace QuickProxyNet
{
	internal static class ProxyConnector
	{
		public static async ValueTask<Stream> ConnectToProxyAsync(Stream stream, Uri proxyUri, string host, int port, NetworkCredential? proxyCredentials, CancellationToken cancellationToken)
		{
			using (cancellationToken.Register(s => ((Stream)s!).Dispose(), stream))
			{
				try
				{
					NetworkCredential? credentials = proxyCredentials?.GetCredential(proxyUri, proxyUri.Scheme);

					if (string.Equals(proxyUri.Scheme, "socks5", StringComparison.OrdinalIgnoreCase))
					{
						await SocksHelper.EstablishSocks5TunnelAsync(stream, host, port, credentials, true).ConfigureAwait(false);
						return stream;
					}
					else if (string.Equals(proxyUri.Scheme, "socks4a", StringComparison.OrdinalIgnoreCase))
					{
						await SocksHelper.EstablishSocks4TunnelAsync(stream, isVersion4a: true, host, port, credentials, true).ConfigureAwait(false);
						return stream;
					}
					else if (string.Equals(proxyUri.Scheme, "socks4", StringComparison.OrdinalIgnoreCase))
					{
						await SocksHelper.EstablishSocks4TunnelAsync(stream, isVersion4a: false, host, port, credentials, true).ConfigureAwait(false);
						return stream;
					}
					else if (string.Equals(proxyUri.Scheme, "http", StringComparison.OrdinalIgnoreCase) ||
						string.Equals(proxyUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
					{
						var result = await HttpHelper.EstablishHttpTunnelAsync(stream, proxyUri, host, port, credentials, cancellationToken);
						return result;
					}
					else
					{
						throw new NotSupportedException("Bad protocol");
					}
				}
				catch
				{
					stream.Dispose();
					throw;
				}
			}
		}
	}
	public interface IProxyClient
	{
		NetworkCredential? ProxyCredentials { get; }

		string ProxyHost { get; }

		int ProxyPort { get; }

		ProxyType Type { get; }

		IPEndPoint LocalEndPoint { get; set; }



		Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken));
		ValueTask<Stream> ConnectAsync(Stream source, string host, int port, CancellationToken cancellationToken = default(CancellationToken));

		Task<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default(CancellationToken));
	}
}
