using System.Net;
using System.Net.Sockets;

namespace QuickProxyNet
{
	public static class SocketHelper
	{

		public static Socket CreateSocket()
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				NoDelay = true,
				LingerState = new LingerOption(true, 0),
				SendTimeout = 10000,
				ReceiveTimeout = 10000
			};
			return socket;
		}
	}

	public interface IProxyClient
	{
		NetworkCredential ProxyCredentials { get; }

		string ProxyHost { get; }

		int ProxyPort { get; }

		ProxyType Type { get; }

		IPEndPoint LocalEndPoint { get; set; }

		ValueTask<Stream> EstablishTCPConnectionAsync(CancellationToken token);

		ValueTask<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken));
		ValueTask<Stream> ConnectAsync(Stream source, string host, int port, CancellationToken cancellationToken = default(CancellationToken));

		ValueTask<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default(CancellationToken));
	}
}
