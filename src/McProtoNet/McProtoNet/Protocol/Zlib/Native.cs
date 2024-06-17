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
    ///<summary>
    /// libdeflate_alloc_compressor() allocates a new compressor that supports
    /// DEFLATE, zlib, and gzip compression.  'compression_level' is the compression
    /// level on a zlib-like scale but with a higher maximum value (1 = fastest, 6 =
    /// medium/default, 9 = slow, 12 = slowest).  Level 0 is also supported and means
    /// "no compression", specifically "create a valid stream, but only emit
    /// uncompressed blocks" (this will expand the data slightly).
    ///
    /// The return value is a pointer to the new compressor, or NULL if out of memory
    /// or if the compression level is invalid (i.e. outside the range [0, 12]).
    ///
    /// Note: for compression, the sliding window size is defined at compilation time
    /// to 32768, the largest size permissible in the DEFLATE format.  It cannot be
    /// changed at runtime.
    ///
    /// A single compressor is not safe to use by multiple threads concurrently.
    /// However, different threads may use different compressors concurrently.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_compressor libdeflate_alloc_compressor(int compression_level);

    /// <summary>
    /// Like <see cref="libdeflate_alloc_compressor"/> but allows specifying advanced options per-compressor.
    /// </summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_compressor libdeflate_alloc_compressor_ex(int compression_level, in libdeflate_options options);

    ///<summary>
    /// libdeflate_deflate_compress() performs raw DEFLATE compression on a buffer of
    /// data.  The function attempts to compress 'in_nbytes' bytes of data located at
    /// 'in' and write the results to 'out', which has space for 'out_nbytes_avail'
    /// bytes.  The return value is the compressed size in bytes, or 0 if the data
    /// could not be compressed to 'out_nbytes_avail' bytes or fewer.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_deflate_compress(libdeflate_compressor compressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail);

    ///<summary>
    /// libdeflate_deflate_compress_bound() returns a worst-case upper bound on the
    /// number of bytes of compressed data that may be produced by compressing any
    /// buffer of length less than or equal to 'in_nbytes' using
    /// libdeflate_deflate_compress() with the specified compressor.  Mathematically,
    /// this bound will necessarily be a number greater than or equal to 'in_nbytes'.
    /// It may be an overestimate of the true upper bound.  The return value is
    /// guaranteed to be the same for all invocations with the same compressor and
    /// same 'in_nbytes'.
    ///
    /// As a special case, 'compressor' may be NULL.  This causes the bound to be
    /// taken across *any* libdeflate_compressor that could ever be allocated with
    /// this build of the library, with any options.
    ///
    /// Note that this function is not necessary in many applications.  With
    /// block-based compression, it is usually preferable to separately store the
    /// uncompressed size of each block and to store any blocks that did not compress
    /// to less than their original size uncompressed.  In that scenario, there is no
    /// need to know the worst-case compressed size, since the maximum number of
    /// bytes of compressed data that may be used would always be one less than the
    /// input length.  You can just pass a buffer of that size to
    /// libdeflate_deflate_compress() and store the data uncompressed if
    /// libdeflate_deflate_compress() returns 0, indicating that the compressed data
    /// did not fit into the provided output buffer.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_deflate_compress_bound(libdeflate_compressor compressor, size_t in_nbytes);

    ///<summary>
    /// Like libdeflate_deflate_compress(), but stores the data in the zlib wrapper
    /// format.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_zlib_compress(libdeflate_compressor compressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail);

    ///<summary>
    /// Like libdeflate_deflate_compress_bound(), but assumes the data will be
    /// compressed with libdeflate_zlib_compress() rather than with
    /// libdeflate_deflate_compress().
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_zlib_compress_bound(libdeflate_compressor compressor, size_t in_nbytes);

    ///<summary>
    /// Like libdeflate_deflate_compress(), but stores the data in the gzip wrapper
    /// format.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_gzip_compress(libdeflate_compressor compressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail);

    ///<summary>
    /// Like libdeflate_deflate_compress_bound(), but assumes the data will be
    /// compressed with libdeflate_gzip_compress() rather than with
    /// libdeflate_deflate_compress().
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern size_t libdeflate_gzip_compress_bound(libdeflate_compressor compressor, size_t in_nbytes);

    ///<summary>
    /// libdeflate_free_compressor() frees a compressor that was allocated with
    /// libdeflate_alloc_compressor().  If a NULL pointer is passed in, no action is
    /// taken.
    ///</summary>
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
        static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
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

    /// <summary>
    ///  This field must be set to the struct size.  This field exists for
    ///  extensibility, so that fields can be appended to this struct in
    ///  future versions of libdeflate while still supporting old binaries.
    /// </summary>
    public readonly size_t sizeof_options;

    /// <summary>
    /// An optional custom memory allocator to use for this (de)compressor.
    /// 'malloc_func' must be a function that behaves like malloc().
    /// </summary>
    /// <remarks>
    /// This is useful in cases where a process might have multiple users of
    /// libdeflate who want to use different memory allocators.  For example,
    /// a library might want to use libdeflate with a custom memory allocator
    /// without interfering with user code that might use libdeflate too.
    ///
    /// This takes priority over the "global" memory allocator (which by
    /// default is malloc() and free(), but can be changed by
    /// libdeflate_set_memory_allocator()).  Moreover, libdeflate will never
    /// call the "global" memory allocator if a per-(de)compressor custom
    /// allocator is always given.
    /// </remarks>
    public readonly CustomMemoryAllocator.malloc_func malloc;

    /// <summary>
    /// An optional custom memory deallocator to use for this (de)compressor.
    /// 'free_func' must be a function that behaves like free().
    /// </summary>
    /// <remarks>
    /// This is useful in cases where a process might have multiple users of
    /// libdeflate who want to use different memory allocators.  For example,
    /// a library might want to use libdeflate with a custom memory allocator
    /// without interfering with user code that might use libdeflate too.
    ///
    /// This takes priority over the "global" memory allocator (which by
    /// default is malloc() and free(), but can be changed by
    /// libdeflate_set_memory_allocator()).  Moreover, libdeflate will never
    /// call the "global" memory allocator if a per-(de)compressor custom
    /// allocator is always given.
    /// </remarks>
    public readonly CustomMemoryAllocator.free_func free;
}

internal static class CustomMemoryAllocator
{
    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr malloc_func(size_t size);
    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void free_func(IntPtr alloc);

    ///<summary>
    /// Install a custom memory allocator which libdeflate will use for all memory
    /// allocations.  'malloc_func' is a function that must behave like malloc(), and
    /// 'free_func' is a function that must behave like free().
    ///
    /// There must not be any libdeflate_compressor or libdeflate_decompressor
    /// structures in existence when calling this function.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_set_memory_allocator(malloc_func malloc, free_func free);
}

internal static class Decompression
{
    ///<summary>
    /// Result of a call to libdeflate_deflate_decompress(),
    /// libdeflate_zlib_decompress(), or libdeflate_gzip_decompress().
    ///</summary>
    public enum libdeflate_result
    {
        ///<summary>
        /// Decompression was successful.
        ///</summary>
        LIBDEFLATE_SUCCESS = 0,

        ///<summary>
        /// Decompressed failed because the compressed data was invalid, corrupt,
        /// or otherwise unsupported.
        ///</summary>
        LIBDEFLATE_BAD_DATA = 1,

        ///<summary>
        /// A NULL 'actual_out_nbytes_ret' was provided, but the data would have
        /// decompressed to fewer than 'out_nbytes_avail' bytes.
        ///</summary>
        LIBDEFLATE_SHORT_OUTPUT = 2,

        ///<summary>
        /// The data would have decompressed to more than 'out_nbytes_avail' bytes.
        ///</summary>
        LIBDEFLATE_INSUFFICIENT_SPACE = 3,
    }

    ///<summary>
    /// libdeflate_alloc_decompressor() allocates a new decompressor that can be used
    /// for DEFLATE, zlib, and gzip decompression.  The return value is a pointer to
    /// the new decompressor, or NULL if out of memory.
    ///
    /// This function takes no parameters, and the returned decompressor is valid for
    /// decompressing data that was compressed at any compression level and with any
    /// sliding window size.
    ///
    /// A single decompressor is not safe to use by multiple threads concurrently.
    /// However, different threads may use different decompressors concurrently.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_decompressor libdeflate_alloc_decompressor();

    /// <summary>
    /// Like <see cref="libdeflate_alloc_decompressor"/> but allows specifying advanced options per-decompressor.
    /// </summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_decompressor libdeflate_alloc_decompressor_ex(in libdeflate_options options);

    ///<summary>
    /// libdeflate_deflate_decompress() decompresses the DEFLATE-compressed stream
    /// from the buffer 'in' with compressed size up to 'in_nbytes' bytes.  The
    /// uncompressed data is written to 'out', a buffer with size 'out_nbytes_avail'
    /// bytes.  If decompression succeeds, then 0 (LIBDEFLATE_SUCCESS) is returned.
    /// Otherwise, a nonzero result code such as LIBDEFLATE_BAD_DATA is returned.  If
    /// a nonzero result code is returned, then the contents of the output buffer are
    /// undefined.
    ///
    /// Decompression stops at the end of the DEFLATE stream (as indicated by the
    /// BFINAL flag), even if it is actually shorter than 'in_nbytes' bytes.
    ///
    /// libdeflate_deflate_decompress() can be used in cases where the actual
    /// uncompressed size is known (recommended) or unknown (not recommended):
    ///
    ///   - If the actual uncompressed size is known, then pass the actual
    ///     uncompressed size as 'out_nbytes_avail' and pass NULL for
    ///     'actual_out_nbytes_ret'.  This makes libdeflate_deflate_decompress() fail
    ///     with LIBDEFLATE_SHORT_OUTPUT if the data decompressed to fewer than the
    ///     specified number of bytes.
    ///
    ///   - If the actual uncompressed size is unknown, then provide a non-NULL
    ///     'actual_out_nbytes_ret' and provide a buffer with some size
    ///     'out_nbytes_avail' that you think is large enough to hold all the
    ///     uncompressed data.  In this case, if the data decompresses to less than
    ///     or equal to 'out_nbytes_avail' bytes, then
    ///     libdeflate_deflate_decompress() will write the actual uncompressed size
    ///     to *actual_out_nbytes_ret and return 0 (LIBDEFLATE_SUCCESS).  Otherwise,
    ///     it will return LIBDEFLATE_INSUFFICIENT_SPACE if the provided buffer was
    ///     not large enough but no other problems were encountered, or another
    ///     nonzero result code if decompression failed for another reason.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_deflate_decompress(libdeflate_decompressor decompressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_out_nbytes_ret);

    ///<summary>
    /// Like libdeflate_deflate_decompress(), but adds the 'actual_in_nbytes_ret'
    /// argument.  If decompression succeeds and 'actual_in_nbytes_ret' is not NULL,
    /// then the actual compressed size of the DEFLATE stream (aligned to the next
    /// byte boundary) is written to *actual_in_nbytes_ret.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_deflate_decompress_ex(libdeflate_decompressor decompressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_in_nbytes_ret, out size_t actual_out_nbytes_ret);

    ///<summary>
    /// Like libdeflate_deflate_decompress(), but assumes the zlib wrapper format
    /// instead of raw DEFLATE.
    ///
    /// Decompression will stop at the end of the zlib stream, even if it is shorter
    /// than 'in_nbytes'.  If you need to know exactly where the zlib stream ended,
    /// use libdeflate_zlib_decompress_ex().
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_zlib_decompress(libdeflate_decompressor decompressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_out_nbytes_ret);

    ///<summary>
    /// Like libdeflate_deflate_decompress(), but assumes the zlib wrapper format
    /// instead of raw DEFLATE.
    ///
    /// Decompression will stop at the end of the zlib stream, even if it is shorter
    /// than 'in_nbytes'.  If you need to know exactly where the zlib stream ended,
    /// use libdeflate_zlib_decompress_ex().
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_zlib_decompress_ex(libdeflate_decompressor decompressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_in_nbytes_ret, out size_t actual_out_nbytes_ret);

    ///<summary>
    /// Like libdeflate_deflate_decompress(), but assumes the gzip wrapper format
    /// instead of raw DEFLATE.
    ///
    /// If multiple gzip-compressed members are concatenated, then only the first
    /// will be decompressed.  Use libdeflate_gzip_decompress_ex() if you need
    /// multi-member support.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_gzip_decompress(libdeflate_decompressor decompressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_out_nbytes_ret);

    ///<summary>
    /// Like libdeflate_gzip_decompress(), but adds the 'actual_in_nbytes_ret'
    /// argument.  If 'actual_in_nbytes_ret' is not NULL and the decompression
    /// succeeds (indicating that the first gzip-compressed member in the input
    /// buffer was decompressed), then the actual number of input bytes consumed is
    /// written to *actual_in_nbytes_ret.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern libdeflate_result libdeflate_gzip_decompress_ex(libdeflate_decompressor decompressor, in byte @in, size_t in_nbytes, ref byte @out, size_t out_nbytes_avail, out size_t actual_in_nbytes_ret, out size_t actual_out_nbytes_ret);

    ///<summary>
    /// libdeflate_free_decompressor() frees a decompressor that was allocated with
    /// libdeflate_alloc_decompressor().  If a NULL pointer is passed in, no action
    /// is taken.
    ///</summary>
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_free_decompressor(libdeflate_decompressor decompressor);
}

