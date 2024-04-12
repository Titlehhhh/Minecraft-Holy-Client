using System.Net;

namespace QuickProxyNet
{

	internal static class HttpHelper
	{
		private readonly static HttpClient s_httpClient;

		static HttpHelper()
		{

			s_httpClient = new HttpClient();
		}

		internal static async ValueTask<Stream> EstablishHttpTunnelAsync(Stream stream, Uri proxyUri, string host, int port, NetworkCredential? credentials, CancellationToken cancellationToken)
		{

			HttpRequestMessage tunnelRequest = new HttpRequestMessage(HttpMethod.Connect, proxyUri);
			tunnelRequest.Headers.Host = $"{host}:{port}";    // This specifies destination host/port to connect to


			tunnelRequest.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows; Windows NT 10.5; Win64; x64; en-US) Gecko/20100101 Firefox/73.8");


			HttpResponseMessage tunnelResponse = await s_httpClient.SendAsync(tunnelRequest, cancellationToken).ConfigureAwait(false);

			if (tunnelResponse.StatusCode != HttpStatusCode.OK)
			{
				tunnelResponse.Dispose();
				throw new ProxyProtocolException("Bad Http status");
			}

			try
			{
				return await tunnelResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
			}
			catch
			{
				tunnelResponse.Dispose();
				throw;
			}

		}

	}
}
