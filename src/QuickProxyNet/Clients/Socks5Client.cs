using System.Net;

namespace QuickProxyNet
{
	public class Socks5Client : ProxyClient
	{
		public override ProxyType Type => ProxyType.SOCKS5;

		public Socks5Client(string host, int port) : base("socks5", host, port)
		{
		}

		public Socks5Client(string host, int port, NetworkCredential credentials) : base("socks5", host, port, credentials)
		{
		}


		public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{
			var result = await ProxyConnector.ConnectToProxyAsync(stream, this.ProxyUri, host, port, this.ProxyCredentials, cancellationToken);
			return result;
		}
	}
}
