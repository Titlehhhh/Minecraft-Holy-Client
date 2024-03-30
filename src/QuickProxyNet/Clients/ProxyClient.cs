using System.Net;
using System.Net.Sockets;

namespace QuickProxyNet
{


	public abstract class ProxyClient : IProxyClient
	{
		public abstract ProxyType Type { get; }

		protected ProxyClient(string host, int port)
		{
			if (host == null)
				throw new ArgumentNullException(nameof(host));

			if (host.Length == 0 || host.Length > 255)
				throw new ArgumentException("The length of the host name must be between 0 and 256 characters.", nameof(host));

			if (port < 0 || port > 65535)
				throw new ArgumentOutOfRangeException(nameof(port));

			ProxyHost = host;
			ProxyPort = port == 0 ? 1080 : port;

			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				NoDelay = true,
				LingerState = new LingerOption(true, 0),
				SendTimeout = 10000,
				ReceiveTimeout = 10000
			};
		}
		private Socket socket;
		protected ProxyClient(string host, int port, NetworkCredential credentials) : this(host, port)
		{
			if (credentials == null)
				throw new ArgumentNullException(nameof(credentials));

			ProxyCredentials = credentials;
		}

		public NetworkCredential? ProxyCredentials
		{
			get; private set;
		}

		public string ProxyHost
		{
			get; private set;
		}

		public int ProxyPort
		{
			get; private set;
		}

		public IPEndPoint LocalEndPoint
		{
			get; set;
		}

		internal static void ValidateArguments(string host, int port)
		{
			if (host == null)
				throw new ArgumentNullException(nameof(host));

			if (host.Length == 0 || host.Length > 255)
				throw new ArgumentException("The length of the host name must be between 0 and 256 characters.", nameof(host));

			if (port <= 0 || port > 65535)
				throw new ArgumentOutOfRangeException(nameof(port));
		}

		static void ValidateArguments(string host, int port, int timeout)
		{
			ValidateArguments(host, port);

			if (timeout < -1)
				throw new ArgumentOutOfRangeException(nameof(timeout));
		}

		public async Task<Stream> ConnectAsync(string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				NoDelay = true,
				LingerState = new LingerOption(true, 0),
				SendTimeout = 10000,
				ReceiveTimeout = 10000
			};

			try
			{
				await socket.ConnectAsync(ProxyHost, ProxyPort, cancellationToken);


				var stream = new NetworkStream(socket, true);
				try
				{


					using var reg = cancellationToken.Register(() => stream.Dispose());

					return await ConnectAsync(stream, host, port, cancellationToken);
				}
				catch
				{
					stream.Dispose();
					throw;
				}

			}
			catch
			{


				throw;
			}

		}

		public async virtual Task<Stream> ConnectAsync(string host, int port, int timeout, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidateArguments(host, port, timeout);

			cancellationToken.ThrowIfCancellationRequested();



			using (var ts = new CancellationTokenSource(timeout))
			{
				//ts.CancelAfter(TimeSpan.FromSeconds(10));
				using (var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, ts.Token))
				{
					return await ConnectAsync(host, port, linked.Token);
				}
			}


		}

		public abstract ValueTask<Stream> ConnectAsync(Stream source, string host, int port, CancellationToken cancellationToken = default);

		public async Task<Stream> EstablishTCPConnectionAsync(CancellationToken token)
		{

			throw new NotImplementedException();
			//TcpClient tcpClient = new();

			try
			{
				await socket.ConnectAsync(ProxyHost, ProxyPort, token);

			}
			catch
			{
				socket.Dispose();
				throw;
			}


			return new NetworkStream(socket, true);
		}
		private bool disposed = false;
		public void Dispose()
		{
			if (disposed)
				return;

			disposed = true;
			Interlocked.Exchange(ref socket, null)?.Dispose();


			GC.SuppressFinalize(this);

		}
	}
}
