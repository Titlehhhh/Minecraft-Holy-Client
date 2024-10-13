using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cysharp.Text;
using DotNext;
using DotNext.Buffers;

namespace QuickProxyNet;

internal static class HttpHelper
{
    private const int BufferSize = 4096;

    private static void Test(string host, int port)
    {
        scoped BufferWriterSlim<char> test1 = new();
        test1.Interpolate($"asd{host}");
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static async ValueTask WriteConnectionCommand(Stream stream, string host, int port,
        NetworkCredential proxyCredentials, CancellationToken cancellationToken)
    {
        var builder = ZString.CreateUtf8StringBuilder();
        try
        {
            builder.AppendFormat("CONNECT {0}:{1} HTTP/1.1\r\n", host, port);
            builder.AppendFormat("Host: {0}:{1}\r\n", host, port);
            if (proxyCredentials != null)
            {
                var token = Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}:{1}",
                    proxyCredentials.UserName, proxyCredentials.Password));
                var base64 = Convert.ToBase64String(token);
                builder.AppendFormat("Proxy-Authorization: Basic {0}\r\n", base64);
            }

            builder.Append("\r\n");


            await stream.WriteAsync(builder.AsMemory(), cancellationToken);
        }
        finally
        {
            builder.Dispose();
        }
    }



    internal static async ValueTask<Stream> EstablishHttpTunnelAsync(Stream stream, Uri proxyUri, string host, int port,
        NetworkCredential? credentials, CancellationToken cancellationToken)
    {
        await WriteConnectionCommand(stream, host, port, credentials, cancellationToken);


        var parser = new HttpResponseParser();
        try
        {
            bool find;
            do
            {
                var memory = parser.GetMemory();
                var nread = await stream.ReadAsync(memory, cancellationToken);
                if (nread <= 0)
                    throw new EndOfStreamException();
                find = parser.Parse(nread);
            } while (find == false);

            bool isValid= parser.Validate();
            //string response = parser.ToString();

            if (!isValid)
            {
                throw new ProxyProtocolException($"Failed to connect {host}:{port}");
            }

            return stream;
        }
        finally
        {
            parser.Dispose();
        }
    }
    
    
}