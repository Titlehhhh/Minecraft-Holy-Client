using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using DotNext.Buffers;

namespace QuickProxyNet;

internal struct HttpResponseParser
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