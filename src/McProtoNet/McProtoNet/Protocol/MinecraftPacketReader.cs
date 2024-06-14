using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using LibDeflate;
using McProtoNet.Abstractions;

namespace McProtoNet.Protocol;

public sealed class MinecraftPacketReader : IDisposable
{
    private static readonly MemoryAllocator<byte> memoryAllocator = ArrayPool<byte>.Shared.ToAllocator();

    //private readonly Inflater Inflater = new Inflater();

    private readonly ZlibDecompressor decompressor = new();

    private int _compressionThreshold;

    public Stream BaseStream { get; set; }

    public void Dispose()
    {
        decompressor.Dispose();
    }


    //[MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public async ValueTask<InputPacket> ReadNextPacketAsync(CancellationToken token = default)
    {
        var len = await BaseStream.ReadVarIntAsync(token);
        if (_compressionThreshold <= 0)
        {
            var id = await BaseStream.ReadVarIntAsync(token);
            len -= id.GetVarIntLength();

            //var buffer = ArrayPool<byte>.Shared.Rent(len);

            var buffer = memoryAllocator.AllocateExactly(len);

            try
            {
                await BaseStream.ReadExactlyAsync(buffer.Memory, token);

                return new InputPacket(id, buffer);
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

                var uncompressed = ArrayPool<byte>.Shared.Rent(sizeUncompressed);

                var status = decompressor.Decompress(
                    buffer_compress.AsSpan(0, len),
                    uncompressed.AsSpan(0, sizeUncompressed), out var written);
                if (status != OperationStatus.Done) throw new Exception("Decompress Error");

                var id = ReadVarInt(uncompressed, out var offset);


                var memoryOwner = new MemoryOwner<byte>(uncompressed, sizeUncompressed);

                try
                {
                    return new InputPacket(id, memoryOwner, offset);
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

            var id = await BaseStream.ReadVarIntAsync(token);
            len -= id.GetVarIntLength() + 1;


            var buffer = memoryAllocator.AllocateExactly(len);

            try
            {
                await BaseStream.ReadExactlyAsync(buffer.Memory, token);

                return new InputPacket(id, buffer);
            }
            catch
            {
                buffer.Dispose();
                throw;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ReadVarInt(Span<byte> data, out int len)
    {
        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            read = data[numRead];

            var value = read & 0b01111111;
            result |= value << (7 * numRead);

            numRead++;
            if (numRead > 5) throw new ArithmeticException("VarInt too long");
        } while ((read & 0b10000000) != 0);

        //data = data.Slice(numRead);


        len = numRead;
        return result;
    }

    public void SwitchCompression(int threshold)
    {
        _compressionThreshold = threshold;
    }
}