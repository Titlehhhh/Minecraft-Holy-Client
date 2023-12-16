using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;


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
		public override async Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{


			ValidateArguments(host, port);
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

			cancellationToken.ThrowIfCancellationRequested();



			NetworkStream stream = new NetworkStream(socket, true);
			using (cancellationToken.Register(s => ((Stream)s!).Dispose(), stream))
			{
				try
				{


					await SocksHelper.EstablishSocks4TunnelAsync(stream, IsSocks4a, host, port, null, true, cancellationToken);
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
