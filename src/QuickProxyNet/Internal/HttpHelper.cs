using Cysharp.Text;
using System;
using System.Buffers;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using DotNext;
using System.Buffers;
using System.Text.Unicode;
using DotNext.Buffers;
using System.Numerics;

namespace QuickProxyNet
{

	internal static class HttpHelper
	{
		const int BufferSize = 4096;
		private static void Test(string host, int port)
		{

			scoped BufferWriterSlim<char> test1 = new();
			test1.Interpolate($"asd{host}");




		}


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
			//builder.ToString
			do
			{
				Memory<byte> memory = builder.GetMemory(BufferSize);
				int nread = await stream.ReadAsync(memory, cancellationToken);

				if (nread <= 0)
					break;

				builder.Advance(nread);




				return null;



			} while (true);

			return null;
		}

	}

	public static class StringEscape
	{
		static char[] toEscape = "\0\x1\x2\x3\x4\x5\x6\a\b\t\n\v\f\r\xe\xf\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f\"\\".ToCharArray();
		static string[] literals = @"\0,\x0001,\x0002,\x0003,\x0004,\x0005,\x0006,\a,\b,\t,\n,\v,\f,\r,\x000e,\x000f,\x0010,\x0011,\x0012,\x0013,\x0014,\x0015,\x0016,\x0017,\x0018,\x0019,\x001a,\x001b,\x001c,\x001d,\x001e,\x001f".Split(new char[] { ',' });

		public static string Escape(this string input)
		{
			int i = input.IndexOfAny(toEscape);
			if (i < 0) return input;

			var sb = new System.Text.StringBuilder(input.Length + 5);
			int j = 0;
			do
			{
				sb.Append(input, j, i - j);
				var c = input[i];
				if (c < 0x20) sb.Append(literals[c]); else sb.Append(@"\").Append(c);
			} while ((i = input.IndexOfAny(toEscape, j = ++i)) > 0);

			return sb.Append(input, j, input.Length - j).ToString();
		}
	}


}
