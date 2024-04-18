using Cysharp.Text;
using System;
using System.Buffers;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using DotNext;
using System.Buffers;

namespace QuickProxyNet
{

	internal static class HttpHelper
	{
		const int BufferSize = 4096;


		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static async ValueTask WriteConnectionCommand(Stream stream, string host, int port, NetworkCredential proxyCredentials, CancellationToken cancellationToken)
		{
			using var builder = ZString.CreateUtf8StringBuilder();

			builder.AppendFormat("CONNECT {0}:{1} HTTP/1.1\r\n", host, port);
			builder.AppendFormat("Host: {0}:{1}\r\n", host, port);
			if (proxyCredentials != null)
			{
				var token = Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", proxyCredentials.UserName, proxyCredentials.Password));
				var base64 = Convert.ToBase64String(token);
				builder.AppendFormat("Proxy-Authorization: Basic {0}\r\n", base64);
			}
			builder.Append("\r\n");


			await stream.WriteAsync(builder.AsMemory(), cancellationToken);

		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static bool TryConsumeHeaders(ref Utf16ValueStringBuilder builder)
		{



			return false;
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

		internal static async ValueTask<Stream> EstablishHttpTunnelAsync(Stream stream, Uri proxyUri, string host, int port, NetworkCredential? credentials, CancellationToken cancellationToken)
		{
			await WriteConnectionCommand(stream, host, port, credentials, cancellationToken);

			using var builder = ZString.CreateUtf8StringBuilder();

			do
			{
				Memory<byte> memory = builder.GetMemory(BufferSize);
				int nread = await stream.ReadAsync(memory, cancellationToken);

				if (nread <= 0)
					break;

				builder.Advance(nread);


				string test = builder.ToString();

				
				//if (TryConsumeHeaders(builder, buffer, ref index, nread, ref newline))
				//	break;



			} while (true);

			return null;
		}

	}
}
