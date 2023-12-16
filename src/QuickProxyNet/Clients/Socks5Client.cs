using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Security.Authentication;
using System.Text;

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


		public override async Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{

			ValidateArguments(host, port);

			cancellationToken.ThrowIfCancellationRequested();



			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				await socket.ConnectAsync(ProxyHost, ProxyPort, cancellationToken);
			}
			catch
			{
				socket.Dispose();
				throw;
			}
			NetworkStream stream = new NetworkStream(socket, true);
			using (cancellationToken.Register(s => ((Stream)s!).Dispose(), stream))
			{
				try
				{
					await SocksHelper.EstablishSocks5TunnelAsync(stream, host, port, null, true);
				}
				catch
				{
					stream.Dispose();
					throw;
				}
			}
			return stream;

		}
	}
}
