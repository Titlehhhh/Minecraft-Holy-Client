using System.Runtime.InteropServices;
using size_t = nuint;
using libdeflate_decompressor = System.IntPtr;


namespace McProtoNet.Protocol.Zlib.Native;

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