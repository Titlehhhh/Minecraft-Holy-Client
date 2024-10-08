using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Abstractions;
using McProtoNet.Net.Zlib;

namespace McProtoNet.Net;

public sealed class MinecraftPacketReader
{
    private static readonly MemoryAllocator<byte> memoryAllocator = ArrayPool<byte>.Shared.ToAllocator();


    private int _compressionThreshold;

    public Stream BaseStream { get; set; }

    public void Dispose()
    {
        //decompressor.Dispose();
    }


    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async ValueTask<InputPacket> ReadNextPacketAsync(CancellationToken token = default)
    {
        var len = await BaseStream.ReadVarIntAsync(token);
        if (_compressionThreshold <= 0)
        {
            var buffer = memoryAllocator.AllocateExactly(len);
            try
            {
                await BaseStream.ReadExactlyAsync(buffer.Memory, token);
                return new InputPacket(buffer);
            }
            catch
            {
                buffer.Dispose();
                throw;
            }
        }
        var sizeUncompressed = await BaseStream.ReadVarIntAsync(token);
        if (sizeUncompressed > 0)
        {
            if (sizeUncompressed < _compressionThreshold)
                throw new Exception(
                    $"Длина sizeUncompressed меньше порога сжатия. sizeUncompressed: {sizeUncompressed} Порог: {_compressionThreshold}");
            len -= sizeUncompressed.GetVarIntLength();
            var buffer_compress = ArrayPool<byte>.Shared.Rent(len);
            try
            {
                await BaseStream.ReadExactlyAsync(buffer_compress, 0, len, token);


                var memoryOwner = new MemoryOwner<byte>(ArrayPool<byte>.Shared, sizeUncompressed);
                try
                {
                    DecompressCore(buffer_compress.AsSpan(0, len), memoryOwner.Span);


                    return new InputPacket(memoryOwner);
                }
                catch
                {
                    memoryOwner.Dispose();
                    throw;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer_compress);
            }
        }

        {
            if (sizeUncompressed != 0)
                throw new Exception("size incorrect");

            var buffer = memoryAllocator.AllocateExactly(len - 1); // -1 is sizeUncompressed length !!!
            try
            {
                await BaseStream.ReadExactlyAsync(buffer.Memory, token);
                return new InputPacket(buffer);
            }
            catch
            {
                buffer.Dispose();
                throw;
            }
        }
    }


    private static void DecompressCore(ReadOnlySpan<byte> bufferCompress, Span<byte> uncompress)
    {
        var decompressor = new ZlibDecompressor();
        //var decompressor = LibDeflateCache.RentDecompressor();
        try
        {
            var status = decompressor.Decompress(
                bufferCompress,
                uncompress, out var written);

            if (status != OperationStatus.Done) throw new Exception("Decompress Error");
        }
        finally
        {
            decompressor.Dispose();
        }
    }


    public void SwitchCompression(int threshold)
    {
        _compressionThreshold = threshold;
    }
}