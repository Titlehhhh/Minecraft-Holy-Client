using System.Buffers;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuickProxyNet
{
	public class HttpProxyClient : ProxyClient
	{


		public override ProxyType Type => ProxyType.HTTP;

		const int BufferSize = 4096;

		public HttpProxyClient(string host, int port) : base(host, port)
		{
		}

		public HttpProxyClient(string host, int port, NetworkCredential credentials) : base(host, port, credentials)
		{
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		internal static byte[] GetConnectCommand(string host, int port, NetworkCredential proxyCredentials)
		{
			var builder = new StringBuilder();

			builder.AppendFormat(CultureInfo.InvariantCulture, "CONNECT {0}:{1} HTTP/1.1\r\n", host, port);
			builder.AppendFormat(CultureInfo.InvariantCulture, "Host: {0}:{1}\r\n", host, port);
			if (proxyCredentials != null)
			{
				var token = Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", proxyCredentials.UserName, proxyCredentials.Password));
				var base64 = Convert.ToBase64String(token);
				builder.AppendFormat(CultureInfo.InvariantCulture, "Proxy-Authorization: Basic {0}\r\n", base64);
			}
			builder.Append("\r\n");

			return Encoding.UTF8.GetBytes(builder.ToString());
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		internal static bool TryConsumeHeaders(StringBuilder builder, byte[] buffer, ref int index, int count, ref bool newLine)
		{
			int endIndex = index + count;
			int startIndex = index;
			var endOfHeaders = false;

			while (index < endIndex && !endOfHeaders)
			{
				switch ((char)buffer[index])
				{
					case '\r':
						break;
					case '\n':
						endOfHeaders = newLine;
						newLine = true;
						break;
					default:
						newLine = false;
						break;
				}

				index++;
			}

			var block = Encoding.UTF8.GetString(buffer, startIndex, index - startIndex);
			builder.Append(block);

			return endOfHeaders;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		internal static void ValidateHttpResponse(string response, string host, int port)
		{
			if (response.Length >= 15 && response.StartsWith("HTTP/1.", StringComparison.OrdinalIgnoreCase) &&
				(response[7] == '1' || response[7] == '0') && response[8] == ' ' &&
				response[9] == '2' && response[10] == '0' && response[11] == '0' &&
				response[12] == ' ')
			{
				return;
			}

			throw new ProxyProtocolException(string.Format(CultureInfo.InvariantCulture, "Failed to connect to {0}:{1}: {2}", host, port, response));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public override async ValueTask<Stream> ConnectAsync(Stream stream,string host, int port, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidateArguments(host, port);

			cancellationToken.ThrowIfCancellationRequested();

			
			var command = GetConnectCommand(host, port, ProxyCredentials);

			using (cancellationToken.Register(s => ((Stream)s!).Dispose(), stream))
			{


				try
				{
					await stream.WriteAsync(command.AsMemory(), cancellationToken);

					var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
					var builder = new StringBuilder();

					try
					{
						var newline = false;


						do
						{
							int nread = await stream.ReadAsync(buffer.AsMemory(0, BufferSize), cancellationToken);
							if (nread <= 0)
								throw new EndOfStreamException();
							int index = 0;

							if (TryConsumeHeaders(builder, buffer, ref index, nread, ref newline))
								break;
						} while (true);
					}
					finally
					{
						ArrayPool<byte>.Shared.Return(buffer);
					}


					int index1 = 0;

					while (builder[index1] != '\n')
						index1++;

					if (index1 > 0 && builder[index1 - 1] == '\r')
						index1--;

					// trim everything beyond the "HTTP/1.1 200 ..." part of the response
					builder.Length = index1;

					ValidateHttpResponse(builder.ToString(), host, port);
					return stream;
				}
				catch
				{
					stream.Dispose();
					throw;
				}
			}
		}
	}
}
