using System.Net;

namespace QuickProxyNet
{

	public interface IProxyClient : IDisposable
	{
		NetworkCredential ProxyCredentials { get; }

		string ProxyHost { get; }

		int ProxyPort { get; }

		ProxyType Type { get; }

		IPEndPoint LocalEndPoint { get; set; }

		Task<Stream> EstablishTCPConnectionAsync(CancellationToken token);

		Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken));
		ValueTask<Stream> ConnectAsync(Stream source, string host, int port, CancellationToken cancellationToken = default(CancellationToken));

		Task<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default(CancellationToken));
	}
}
