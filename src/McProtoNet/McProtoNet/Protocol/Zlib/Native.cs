using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using libdeflate_compressor = nint;
using size_t = nuint;
using libdeflate_decompressor = System.IntPtr;


namespace McProtoNet.Protocol.Zlib;

internal static class Constants
{
    public const string DllName = "libdeflate";

    public const CallingConvention CallConv = CallingConvention.Cdecl;
}

internal static class Compression
{
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_compressor libdeflate_alloc_compressor(int compression_level);

    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_compressor libdeflate_alloc_compressor_ex(int compression_level,
        in libdeflate_options options);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_deflate_compress(libdeflate_compressor compressor, in byte @in,
        size_t in_nbytes, ref byte @out, size_t out_nbytes_avail);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_deflate_compress_bound(libdeflate_compressor compressor, size_t in_nbytes);

    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_zlib_compress(libdeflate_compressor compressor, in byte @in,
        size_t in_nbytes, ref byte @out, size_t out_nbytes_avail);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_zlib_compress_bound(libdeflate_compressor compressor, size_t in_nbytes);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_gzip_compress(libdeflate_compressor compressor, in byte @in,
        size_t in_nbytes, ref byte @out, size_t out_nbytes_avail);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_gzip_compress_bound(libdeflate_compressor compressor, size_t in_nbytes);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_free_compressor(libdeflate_compressor compressor);
}

internal readonly struct libdeflate_options
{
    private static readonly size_t Size = (nuint)(nint)Unsafe.SizeOf<libdeflate_options>();

    public libdeflate_options(CustomMemoryAllocator.malloc_func malloc, CustomMemoryAllocator.free_func free)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(malloc);
        ArgumentNullException.ThrowIfNull(free);
#else
        ThrowIfNull(malloc);
        ThrowIfNull(free);
#endif

        this.sizeof_options = Size;
        this.malloc = malloc;
        this.free = free;

#if !NET6_0_OR_GREATER
        static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName
 = null)
        {
            if(argument is null)
            {
                ThrowHelperArgumentNull(paramName!);
            }

            [DoesNotReturn]
            static void ThrowHelperArgumentNull(string paramName) => throw new ArgumentNullException(paramName);
        }
#endif
    }


    public readonly size_t sizeof_options;


    public readonly CustomMemoryAllocator.malloc_func malloc;


    public readonly CustomMemoryAllocator.free_func free;
}

internal static class CustomMemoryAllocator
{
    public delegate IntPtr malloc_func(size_t size);


    public delegate void free_func(IntPtr alloc);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_set_memory_allocator(malloc_func malloc, free_func free);
}

internal static class Decompression
{
    public enum libdeflate_result
    {
        LIBDEFLATE_SUCCESS = 0,


        LIBDEFLATE_BAD_DATA = 1,


        LIBDEFLATE_SHORT_OUTPUT = 2,


        LIBDEFLATE_INSUFFICIENT_SPACE = 3,
    }


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_decompressor libdeflate_alloc_decompressor();


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_decompressor libdeflate_alloc_decompressor_ex(in libdeflate_options options);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_deflate_decompress(libdeflate_decompressor decompressor,
        in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_out_nbytes_ret);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_deflate_decompress_ex(libdeflate_decompressor decompressor,
        in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_in_nbytes_ret,
        out size_t actual_out_nbytes_ret);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_zlib_decompress(libdeflate_decompressor decompressor, in byte @in,
        size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_out_nbytes_ret);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_zlib_decompress_ex(libdeflate_decompressor decompressor,
        in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_in_nbytes_ret,
        out size_t actual_out_nbytes_ret);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_gzip_decompress(libdeflate_decompressor decompressor, in byte @in,
        size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_out_nbytes_ret);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_gzip_decompress_ex(libdeflate_decompressor decompressor,
        in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_in_nbytes_ret,
        out size_t actual_out_nbytes_ret);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_free_decompressor(libdeflate_decompressor decompressor);
}