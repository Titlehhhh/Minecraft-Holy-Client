using System.Globalization;
using System.Net;
using System.Net.Sockets;
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

		internal enum Socks5AddressType : byte
		{
			None = 0x00,
			IPv4 = 0x01,
			Domain = 0x03,
			IPv6 = 0x04
		}

		enum Socks5AuthMethod : byte
		{
			Anonymous = 0x00,
			GSSAPI = 0x01,
			UserPassword = 0x02,
			NotSupported = 0xff
		}

		enum Socks5Command : byte
		{
			Connect = 0x01,
			Bind = 0x02,
			UdpAssociate = 0x03,
		}

		internal enum Socks5Reply : byte
		{
			Success = 0x00,
			GeneralServerFailure = 0x01,
			ConnectionNotAllowed = 0x02,
			NetworkUnreachable = 0x03,
			HostUnreachable = 0x04,
			ConnectionRefused = 0x05,
			TTLExpired = 0x06,
			CommandNotSupported = 0x07,
			AddressTypeNotSupported = 0x08
		}

		internal static string GetFailureReason(byte reply)
		{
			switch ((Socks5Reply)reply)
			{
				case Socks5Reply.GeneralServerFailure: return "General server failure.";
				case Socks5Reply.ConnectionNotAllowed: return "Connection not allowed.";
				case Socks5Reply.NetworkUnreachable: return "Network unreachable.";
				case Socks5Reply.HostUnreachable: return "Host unreachable.";
				case Socks5Reply.ConnectionRefused: return "Connection refused.";
				case Socks5Reply.TTLExpired: return "TTL expired.";
				case Socks5Reply.CommandNotSupported: return "Command not supported.";
				case Socks5Reply.AddressTypeNotSupported: return "Address type not supported.";
				default: return string.Format(CultureInfo.InvariantCulture, "Unknown error ({0}).", (int)reply);
			}
		}

		internal static Socks5AddressType GetAddressType(string host, out IPAddress ip)
		{
			if (!IPAddress.TryParse(host, out ip))
				return Socks5AddressType.Domain;

			switch (ip.AddressFamily)
			{
				case AddressFamily.InterNetworkV6: return Socks5AddressType.IPv6;
				case AddressFamily.InterNetwork: return Socks5AddressType.IPv4;
				default: throw new ArgumentException("The host address must be an IPv4 or IPv6 address.", nameof(host));
			}
		}

		void VerifySocksVersion(byte version)
		{
			if (version != (byte)SocksVersion)
				throw new ProxyProtocolException(string.Format(CultureInfo.InvariantCulture, "Proxy server responded with unknown SOCKS version: {0}", (int)version));
		}

		byte[] GetNegotiateAuthMethodCommand(Socks5AuthMethod[] methods)
		{
			// +-----+----------+----------+
			// | VER | NMETHODS | METHODS  |
			// +-----+----------+----------+
			// |  1  |    1     | 1 to 255 |
			// +-----+----------+----------+
			var buffer = new byte[2 + methods.Length];
			int n = 0;

			buffer[n++] = (byte)SocksVersion;
			buffer[n++] = (byte)methods.Length;
			for (int i = 0; i < methods.Length; i++)
				buffer[n++] = (byte)methods[i];

			return buffer;
		}

		Socks5AuthMethod NegotiateAuthMethod(Socket socket, CancellationToken cancellationToken, params Socks5AuthMethod[] methods)
		{
			var buffer = GetNegotiateAuthMethodCommand(methods);
			socket.Send(buffer);
			//Send(socket, buffer, 0, buffer.Length, cancellationToken);

			// +-----+--------+
			// | VER | METHOD |
			// +-----+--------+
			// |  1  |   1    |
			// +-----+--------+
			int nread, n = 0;
			do
			{
				nread = socket.Receive(buffer, 0 + n, 2 - n, SocketFlags.None);
				if (nread <= 0)
					throw new Exception();
				if (nread > 0)
					n += nread;
			} while (n < 2);

			VerifySocksVersion(buffer[0]);

			return (Socks5AuthMethod)buffer[1];
		}

		async ValueTask<Socks5AuthMethod> NegotiateAuthMethodAsync(NetworkStream stream, CancellationToken cancellationToken, params Socks5AuthMethod[] methods)
		{
			var buffer = GetNegotiateAuthMethodCommand(methods);
			await stream.WriteAsync(buffer.AsMemory(), cancellationToken);


			await stream.ReadToEndAsync(buffer.AsMemory(0, 2), 2, cancellationToken);


			VerifySocksVersion(buffer[0]);

			return (Socks5AuthMethod)buffer[1];
		}

		byte[] GetAuthenticateCommand()
		{
			var user = Encoding.UTF8.GetBytes(ProxyCredentials.UserName);

			if (user.Length > 255)
				throw new AuthenticationException("User name too long.");

			var passwd = Encoding.UTF8.GetBytes(ProxyCredentials.Password);

			if (passwd.Length > 255)
			{
				Array.Clear(passwd, 0, passwd.Length);
				throw new AuthenticationException("Password too long.");
			}

			var buffer = new byte[user.Length + passwd.Length + 3];
			int n = 0;

			buffer[n++] = 1;
			buffer[n++] = (byte)user.Length;
			Buffer.BlockCopy(user, 0, buffer, n, user.Length);
			n += user.Length;
			buffer[n++] = (byte)passwd.Length;
			Buffer.BlockCopy(passwd, 0, buffer, n, passwd.Length);

			Array.Clear(passwd, 0, passwd.Length);

			return buffer;
		}

		void Authenticate(Socket socket, CancellationToken cancellationToken)
		{
			var buffer = GetAuthenticateCommand();

			socket.Send(buffer, SocketFlags.None);

			int nread, n = 0;

			do
			{
				nread = socket.Receive(buffer, 0 + n, 2 - n, SocketFlags.None);
				if (nread <= 0)
					throw new EndOfStreamException();
				n += nread;
			} while (n < 2);

			if (buffer[1] != (byte)Socks5Reply.Success)
				throw new AuthenticationException("Failed to authenticate with SOCKS5 proxy server.");
		}

		async ValueTask AuthenticateAsync(NetworkStream stream, CancellationToken cancellationToken)
		{
			var buffer = GetAuthenticateCommand();

			await stream.WriteAsync(buffer.AsMemory(), cancellationToken);

			int nread, n = 0;

			await stream.ReadToEndAsync(buffer.AsMemory(0, 2), 2, cancellationToken);


			if (buffer[1] != (byte)Socks5Reply.Success)
				throw new AuthenticationException("Failed to authenticate with SOCKS5 proxy server.");
		}

		byte[] GetConnectCommand(Socks5AddressType addrType, byte[] domain, IPAddress ip, int port, out int n)
		{

			var buffer = new byte[4 + 257 + 2];
			byte[] addr;

			n = 0;

			buffer[n++] = (byte)SocksVersion;
			buffer[n++] = (byte)Socks5Command.Connect;
			buffer[n++] = 0x00;
			buffer[n++] = (byte)addrType;
			switch (addrType)
			{
				case Socks5AddressType.Domain:
					buffer[n++] = (byte)domain.Length;
					Buffer.BlockCopy(domain, 0, buffer, n, domain.Length);
					n += domain.Length;
					break;
				case Socks5AddressType.IPv6:
					addr = ip.GetAddressBytes();
					Buffer.BlockCopy(addr, 0, buffer, n, addr.Length);
					n += 16;
					break;
				case Socks5AddressType.IPv4:
					addr = ip.GetAddressBytes();
					Buffer.BlockCopy(addr, 0, buffer, n, addr.Length);
					n += 4;
					break;
			}
			buffer[n++] = (byte)(port >> 8);
			buffer[n++] = (byte)port;

			return buffer;
		}

		int ProcessPartialConnectResponse(string host, int port, byte[] buffer)
		{
			VerifySocksVersion(buffer[0]);

			if (buffer[1] != (byte)Socks5Reply.Success)
				throw new ProxyProtocolException(string.Format(CultureInfo.InvariantCulture, "Failed to connect to {0}:{1}: {2}", host, port, GetFailureReason(buffer[1])));


			var addrType = (Socks5AddressType)buffer[3];

			switch (addrType)
			{
				case Socks5AddressType.Domain: return 4 + buffer[4] + 2;
				case Socks5AddressType.IPv6: return 4 + 16 + 2;
				case Socks5AddressType.IPv4: return 4 + 4 + 2;
				default: throw new ProxyProtocolException("Proxy server returned unknown address type.");
			}
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
			NetworkStream networkStream = new NetworkStream(socket, true);
			try
			{

				var addrType = GetAddressType(host, out var ip);
				byte[] domain = null;

				if (addrType == Socks5AddressType.Domain)
					domain = Encoding.UTF8.GetBytes(host);

				Socks5AuthMethod method;

				if (ProxyCredentials != null)
					method = await NegotiateAuthMethodAsync(networkStream, cancellationToken, Socks5AuthMethod.UserPassword, Socks5AuthMethod.Anonymous);
				else
					method = await NegotiateAuthMethodAsync(networkStream, cancellationToken, Socks5AuthMethod.Anonymous);

				switch (method)
				{
					case Socks5AuthMethod.UserPassword:
						await AuthenticateAsync(networkStream, cancellationToken);
						break;
					case Socks5AuthMethod.Anonymous:
						break;
					default:
						throw new ProxyProtocolException("Failed to negotiate authentication method with the proxy server.");
				}

				var buffer = GetConnectCommand(addrType, domain, ip, port, out int n);

				await networkStream.WriteAsync(buffer.AsMemory(0, n), cancellationToken);

				int need = 5;

				await networkStream.ReadExactlyAsync(buffer.AsMemory(0, need), cancellationToken);


				need = ProcessPartialConnectResponse(host, port, buffer);
				await networkStream.ReadExactlyAsync(buffer.AsMemory(0, need), cancellationToken);
			}
			catch
			{
				await networkStream.DisposeAsync();
				throw;
			}
			return networkStream;

		}
	}
}
