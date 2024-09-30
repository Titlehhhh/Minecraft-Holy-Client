using System.Runtime.InteropServices;

namespace McProtoNet.Protocol.Zlib.Native;

internal static class Compression
{
    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern IntPtr libdeflate_alloc_compressor(int compression_level);

    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern IntPtr libdeflate_alloc_compressor_ex(int compression_level,
        in libdeflate_options options);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern UIntPtr libdeflate_deflate_compress(IntPtr compressor, in byte @in,
        UIntPtr in_nbytes, ref byte @out, UIntPtr out_nbytes_avail);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern UIntPtr libdeflate_deflate_compress_bound(IntPtr compressor, UIntPtr in_nbytes);

    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern UIntPtr libdeflate_zlib_compress(IntPtr compressor, in byte @in,
        UIntPtr in_nbytes, ref byte @out, UIntPtr out_nbytes_avail);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern UIntPtr libdeflate_zlib_compress_bound(IntPtr compressor, UIntPtr in_nbytes);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern UIntPtr libdeflate_gzip_compress(IntPtr compressor, in byte @in,
        UIntPtr in_nbytes, ref byte @out, UIntPtr out_nbytes_avail);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern UIntPtr libdeflate_gzip_compress_bound(IntPtr compressor, UIntPtr in_nbytes);


    [DllImport(Constants.DllName, CallingConvention = Constants.CallConv, ExactSpelling = true)]
    public static extern void libdeflate_free_compressor(IntPtr compressor);
}