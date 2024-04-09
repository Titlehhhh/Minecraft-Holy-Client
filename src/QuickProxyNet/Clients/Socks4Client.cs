using System.Net;
using System.Runtime.CompilerServices;


namespace QuickProxyNet
{


	public class Socks4Client : ProxyClient
	{

		public Socks4Client(string host, int port) : base("socks4", host, port)
		{
		}

		public Socks4Client(string host, int port, NetworkCredential credentials) : base("socks4", host, port, credentials)
		{
		}

		protected bool IsSocks4a
		{
			get; set;
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
