using System.Net;

namespace QuickProxyNet
{
	public class Socks5Client : SocksClient
	{
		public override ProxyType Type => ProxyType.SOCKS5;

		public Socks5Client(string host, int port) : base(5, host, port)
		{
		}

		public Socks5Client(string host, int port, NetworkCredential credentials) : base(5, host, port, credentials)
		{
		}


		public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{

			ValidateArguments(host, port);

			cancellationToken.ThrowIfCancellationRequested();


			await SocksHelper.EstablishSocks5TunnelAsync(stream, host, port, this.ProxyCredentials, true,cancellationToken);

			return stream;

		}
	}
}
