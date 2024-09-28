using System.Buffers;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
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
                throw new ProxyProtocolException($"Failed to connect http://{host}:{port}");
            }

            return stream;
        }
        finally
        {
            parser.Dispose();
        }
    }
}

public struct HttpResponseParser
{
    private static int BufferSize = 1024;
    private MemoryOwner<byte> _memory;
    private static readonly MemoryAllocator<byte> _allocator = ArrayPool<byte>.Shared.ToAllocator();
    private static readonly byte[] http200 = "HTTP/1.1 200".Select(x => (byte)x).ToArray();
    private int _writtenCount = 0;
    private int _indexEnd=-1;
    public ReadOnlySpan<byte> Span => _memory.Span.Slice(0, _writtenCount);

    public HttpResponseParser()
    {
        _memory = _allocator.AllocateExactly(BufferSize);
    }

    public Memory<byte> GetMemory()
    {
        if (_writtenCount < _memory.Length)
        {
            return _memory.Memory.Slice(_writtenCount);
        }

        _memory.Resize(_writtenCount + BufferSize);
        return _memory.Memory.Slice(_writtenCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Parse(int count)
    {
        _writtenCount += count;
        return ParseHttpEnd(count);
    }

    private static readonly byte[] NewLine = "\r\n\r\n".Select(x => (byte)x).ToArray();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ParseHttpEnd(int count)
    {
        int start = _writtenCount - count;
        int length = count;

        int offset = Math.Min(4, start);
        start -= offset;
        length += offset;

        ReadOnlySpan<byte> bytes = _memory.Span.Slice(start, length);

        int index = bytes.IndexOf(NewLine);
        if (index < 0)
        {
            return false;
        }

        _indexEnd = index + start;

        return true;
    }

    public bool Validate()
    {
        if (_indexEnd == -1)
        {
            throw new InvalidOperationException("No find http response");
        }
        
        ReadOnlySpan<byte> span = Span;
        
        
        if (span.Length > (uint)_indexEnd + 4)
        {
            return false;
        }
        if (span.Length >= 15 && span.StartsWith(http200))
        {
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Encoding.UTF8.GetString(_memory.Span.Slice(0, _indexEnd+4));
    }

    public void Dispose()
    {
        _memory.Dispose();
    }
}