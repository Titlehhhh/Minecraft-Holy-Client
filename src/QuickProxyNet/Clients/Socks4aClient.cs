

using System.Net;
using System.Runtime.CompilerServices;

namespace QuickProxyNet
{
	public class Socks4aClient : ProxyClient
	{
		public Socks4aClient(string host, int port) : base("socks4a", host, port)
		{
		}

		public Socks4aClient(string host, int port, NetworkCredential credentials) : base("socks4a", host, port, credentials)
		{
		}


		public override ProxyType Type => ProxyType.SOCKS4;





		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{
			var result = await ProxyConnector.ConnectToProxyAsync(stream, this.ProxyUri, host, port, this.ProxyCredentials, cancellationToken);
			return result;
		}
	}
}
