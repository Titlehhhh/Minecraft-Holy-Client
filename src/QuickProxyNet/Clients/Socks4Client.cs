using System.Net;
using System.Runtime.CompilerServices;


namespace QuickProxyNet
{


	public class Socks4Client : SocksClient
	{

		public Socks4Client(string host, int port) : base(4, host, port)
		{
		}

		public Socks4Client(string host, int port, NetworkCredential credentials) : base(4, host, port, credentials)
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


			ValidateArguments(host, port);




			await SocksHelper.EstablishSocks4TunnelAsync(stream, IsSocks4a, host, port, this.ProxyCredentials, true, cancellationToken);



			return stream;
		}
	}
}
