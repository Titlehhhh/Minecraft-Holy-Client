using System.Buffers;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace QuickProxyNet
{

	public class HttpsProxyClient : ProxyClient
	{
		public HttpsProxyClient(string host, int port) : base("https", host, port)
		{
		}

		public HttpsProxyClient(string host, int port, NetworkCredential credentials) : base("https", host, port, credentials)
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
