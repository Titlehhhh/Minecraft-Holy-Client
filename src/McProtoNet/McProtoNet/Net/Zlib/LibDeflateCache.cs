namespace McProtoNet.Net.Zlib;

internal static class LibDeflateCache
{
    [ThreadStatic] private static ZlibCompressorHeapAlloc? t_compressor;

    [ThreadStatic] private static ZlibDecompressorHeapAlloc? t_decompressor;

    public static ZlibCompressorHeapAlloc RentCompressor()
    {
        ZlibCompressorHeapAlloc compressor = t_compressor ??= new(4);
        return compressor;
    }

    public static ZlibDecompressorHeapAlloc RentDecompressor()
    {
        ZlibDecompressorHeapAlloc decompressor = t_decompressor ??= new();
        return decompressor;
    }
}