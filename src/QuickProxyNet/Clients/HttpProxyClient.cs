using System.Buffers;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuickProxyNet
{
	public class HttpProxyClient : ProxyClient
	{
		public HttpProxyClient(string host, int port) : base("http", host, port)
		{
		}

		public HttpProxyClient(string host, int port, NetworkCredential credentials) : base("http", host, port, credentials)
		{
		}

		public override ProxyType Type => ProxyType.HTTPS;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{
			var result = await ProxyConnector.ConnectToProxyAsync(stream, this.ProxyUri, host, port, this.ProxyCredentials, cancellationToken);
			return result;

		}
	}
}
