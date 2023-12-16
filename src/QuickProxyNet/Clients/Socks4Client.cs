using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;


namespace QuickProxyNet
{


	public class Socks4Client : SocksClient
	{
		static readonly byte[] InvalidIPAddress = { 0, 0, 0, 1 };

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

		enum Socks4Command : byte
		{
			Connect = 0x01,
			Bind = 0x02,
		}

		enum Socks4Reply : byte
		{
			RequestGranted = 0x5a,
			RequestRejected = 0x5b,
			RequestFailedNoIdentd = 0x5c,
			RequestFailedWrongId = 0x5d
		}

		static string GetFailureReason(byte reply)
		{
			switch ((Socks4Reply)reply)
			{
				case Socks4Reply.RequestRejected: return "Request rejected or failed.";
				case Socks4Reply.RequestFailedNoIdentd: return "Request failed; unable to contact client machine's identd service.";
				case Socks4Reply.RequestFailedWrongId: return "Request failed; client ID does not match specified username.";
				default: return "Unknown error.";
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static IPAddress Resolve(string host, IPAddress[] ipAddresses)
		{


			throw new ArgumentException($"Could not resolve a suitable IPv4 address for '{host}'.", nameof(host));
		}


		static ConcurrentDictionary<string, byte[]> _cache = new();

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		static async ValueTask<byte[]> ResolveAsync(string host, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (_cache.TryGetValue(host, out var bytes))
				return bytes;

			var ipAddresses = await Dns.GetHostAddressesAsync(host);

			for (int i = 0; i < ipAddresses.Length; i++)
			{
				if (ipAddresses[i].AddressFamily == AddressFamily.InterNetwork)
				{
					var ip = ipAddresses[i].GetAddressBytes();

					_cache[host] = ip;
					return ip;
				}
			}

			throw new Exception();

		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		byte[] GetConnectCommand(byte[] domain, byte[] addr, int port)
		{
			// +----+-----+----------+----------+----------+-------+--------------+-------+
			// |VER | CMD | DST.PORT | DST.ADDR |  USERID  | NULL  |  DST.DOMAIN  | NULL  |
			// +----+-----+----------+----------+----------+-------+--------------+-------+
			// | 1  |  1  |    2     |    4     | VARIABLE | X'00' |   VARIABLE   | X'00' |
			// +----+-----+----------+----------+----------+-------+--------------+-------+
			var user = ProxyCredentials != null ? Encoding.UTF8.GetBytes(ProxyCredentials.UserName) : new byte[0];
			int bufferSize = 9 + user.Length + (domain != null ? domain.Length + 1 : 0);
			var buffer = new byte[bufferSize];
			int n = 0;

			buffer[n++] = (byte)SocksVersion;
			buffer[n++] = (byte)Socks4Command.Connect;
			buffer[n++] = (byte)(port >> 8);
			buffer[n++] = (byte)port;
			Buffer.BlockCopy(addr, 0, buffer, n, 4);
			n += 4;
			Buffer.BlockCopy(user, 0, buffer, n, user.Length);
			n += user.Length;
			buffer[n++] = 0x00;

			if (domain != null)
			{
				Buffer.BlockCopy(domain, 0, buffer, n, domain.Length);
				n += domain.Length;
				buffer[n++] = 0x00;
			}

			return buffer;
		}




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



			NetworkStream result = new NetworkStream(socket, true);
			try
			{
				await SocksHelper.EstablishSocks4TunnelAsync(result, false, host, port, null, true, cancellationToken);
			}
			catch
			{
				await result.DisposeAsync();
				throw;
			}
			return result;
		}
	}
}
