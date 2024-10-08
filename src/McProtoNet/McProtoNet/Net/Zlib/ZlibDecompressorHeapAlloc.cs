using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using McProtoNet.Net.Zlib.Native;

namespace McProtoNet.Net.Zlib;

public class ZlibDecompressorHeapAlloc
{
    private readonly IntPtr decompressor;

    private bool disposedValue;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ZlibDecompressorHeapAlloc()
    {
        var decompressor = Decompression.libdeflate_alloc_decompressor();
        if (decompressor == IntPtr.Zero)
        {
            ThrowHelper_FailedAllocDecompressor();
        }

        this.decompressor = decompressor;

        [DoesNotReturn]
        static void ThrowHelper_FailedAllocDecompressor() =>
            throw new InvalidOperationException("Failed to allocate decompressor");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static OperationStatus StatusFromResult(Decompression.libdeflate_result result)
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
        => result switch
        {
            Decompression.libdeflate_result.LIBDEFLATE_SUCCESS => OperationStatus.Done,
            Decompression.libdeflate_result.LIBDEFLATE_BAD_DATA => OperationStatus.InvalidData,
            Decompression.libdeflate_result.LIBDEFLATE_SHORT_OUTPUT => OperationStatus.NeedMoreData,
            Decompression.libdeflate_result.LIBDEFLATE_INSUFFICIENT_SPACE => OperationStatus.DestinationTooSmall,
        };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private OperationStatus DecompressCore(ReadOnlySpan<byte> input, Span<byte> output, out nuint bytesWritten)
    {
        return StatusFromResult(Decompression.libdeflate_zlib_decompress(decompressor, MemoryMarshal.GetReference(input),
            (nuint)input.Length, ref MemoryMarshal.GetReference(output), (nuint)output.Length, out bytesWritten));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OperationStatus Decompress(ReadOnlySpan<byte> input, Span<byte> output, out int bytesWritten)
    {
        DisposedGuard();
        var status = DecompressCore(input, output, out nuint out_nbytes);
        switch (status)
        {
            case OperationStatus.Done:
                bytesWritten = (int)out_nbytes;
                return status;
            case OperationStatus.NeedMoreData:
            case OperationStatus.DestinationTooSmall:
            case OperationStatus.InvalidData:
            default:
                bytesWritten = default;
                return status;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    private void DisposedGuard()
    {
        if (disposedValue)
        {
            ThrowHelperObjectDisposed();
        }

        [DoesNotReturn]
        static void ThrowHelperObjectDisposed() => throw new ObjectDisposedException(nameof(ZlibDecompressor));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        Decompression.libdeflate_free_decompressor(decompressor);
    }
}