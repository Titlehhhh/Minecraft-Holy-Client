using System.Net;

namespace QuickProxyNet
{
	public interface IProxyClient
	{
		NetworkCredential ProxyCredentials { get; }

		string ProxyHost { get; }

		int ProxyPort { get; }

		ProxyType Type { get; }

		IPEndPoint LocalEndPoint { get; set; }


		Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken));

		Task<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default(CancellationToken));
	}
}
