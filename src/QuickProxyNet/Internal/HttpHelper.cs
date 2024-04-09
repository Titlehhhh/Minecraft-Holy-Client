using System.Net;

namespace QuickProxyNet
{
	internal static class HttpHelper
	{
		private readonly static HttpClient s_httpClient;

		static HttpHelper()
		{
			var socketsHandler = new SocketsHttpHandler
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(2)
			};
			s_httpClient = new HttpClient(socketsHandler);
		}

		internal static async ValueTask<Stream> EstablishHttpTunnelAsync(Stream stream, Uri proxyUri, string host, int port, NetworkCredential? credentials, CancellationToken cancellationToken)
		{
			HttpRequestMessage tunnelRequest = new HttpRequestMessage(HttpMethod.Connect, proxyUri);
			tunnelRequest.Headers.Host = $"{host}:{port}";    // This specifies destination host/port to connect to

			
			HttpResponseMessage tunnelResponse = await s_httpClient.SendAsync(tunnelRequest, cancellationToken);
			
			if (tunnelResponse.StatusCode != HttpStatusCode.OK)
			{
				tunnelResponse.Dispose();
				throw new ProxyProtocolException("Bad Http status");
			}

			try
			{
				return tunnelResponse.Content.ReadAsStream(cancellationToken);
			}
			catch
			{
				tunnelResponse.Dispose();
				throw;
			}
		}

	}
}
