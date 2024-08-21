using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static McProtoNet.Protocol.Zlib.Compression;

namespace McProtoNet.Protocol.Zlib;

public  ref struct ZlibCompressor
{
    private readonly IntPtr compressor;

    private bool disposedValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ZlibCompressor(int compressionLevel)
    {
        if (compressionLevel < 0 || compressionLevel > 12)
        {
            ThrowHelperBadCompressionLevel();
        }

        var compressor = libdeflate_alloc_compressor(compressionLevel);
        if (compressor == IntPtr.Zero)
        {
            ThrowHelperFailedAllocCompressor();
        }

        this.compressor = compressor;

        static void ThrowHelperBadCompressionLevel() => throw new ArgumentOutOfRangeException(nameof(compressionLevel));

        static void ThrowHelperFailedAllocCompressor() =>
            throw new InvalidOperationException("Failed to allocate compressor");
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private nuint CompressCore(ReadOnlySpan<byte> input, Span<byte> output)
    {
        return libdeflate_zlib_compress(compressor, MemoryMarshal.GetReference(input), (nuint)input.Length,
            ref MemoryMarshal.GetReference(output), (nuint)output.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private nuint GetBoundCore(nuint inputLength)
    {
        return libdeflate_zlib_compress_bound(compressor, inputLength);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compress(ReadOnlySpan<byte> input, Span<byte> output)
    {
        DisposedGuard();
        return (int)CompressCore(input, output);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetBound(int inputLength)
    {
        DisposedGuard();
        return (int)GetBoundCore((nuint)inputLength);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DisposedGuard()
    {
        if (disposedValue)
        {
            ThrowHelperObjectDisposed();
        }

        static void ThrowHelperObjectDisposed() => throw new ObjectDisposedException(nameof(ZlibCompressor));
    }


    public void Dispose()
    {
        libdeflate_free_compressor(compressor);
    }
}