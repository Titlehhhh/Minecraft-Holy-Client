using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.Protocol.Zlib;
using McProtoNet.Abstractions;

namespace McProtoNet.Protocol;

public sealed class MinecraftPacketSender : IDisposable
{
    private const int ZERO_VARLENGTH = 1;
    private static readonly byte[] ZERO_VARINT = { 0 };


    // private static readonly ZlibCompressor s_compressor = new(6);

    private int _compressionThreshold;
    public Stream BaseStream { get; set; }

    public void Dispose()
    {
        //compressor.Dispose();
    }

    public ValueTask SendPacketAsync(ReadOnlyMemory<byte> data, CancellationToken token = default)
    {
        if (_compressionThreshold > 0)
        {
            var uncompressedSize = data.Length;

            if (uncompressedSize >= _compressionThreshold)
            {
                using var compressor = new ZlibCompressor(4);

                var length = compressor.GetBound(uncompressedSize);
                var compressedBuffer = ArrayPool<byte>.Shared.Rent(length);
                try
                {
                    var bytesCompress = compressor.Compress(data.Span, compressedBuffer.AsSpan(0, length));
                    var compressedLength = bytesCompress;

                    var fullsize = compressedLength + uncompressedSize.GetVarIntLength();


                    return SendCompress(fullsize, uncompressedSize, compressedBuffer, bytesCompress, token);
                }
                catch
                {
                    ArrayPool<byte>.Shared.Return(compressedBuffer);
                    throw;
                }
            }
            else
            {
                uncompressedSize++;
                return SendShort(uncompressedSize, data, token);
            }
        }
        else
        {
            return SendPacketWithoutCompressionAsync(data, token);
        }
    }

    private async ValueTask SendShort(int unSize, ReadOnlyMemory<byte> data, CancellationToken token)
    {
        try
        {
            await BaseStream.WriteVarIntAsync(unSize, token).ConfigureAwait(false);
            await BaseStream.WriteAsync(ZERO_VARINT, token).ConfigureAwait(false);
            await BaseStream.WriteAsync(data, token).ConfigureAwait(false);
        }
        finally
        {
            await BaseStream.FlushAsync(token);
        }
    }

    private async ValueTask SendCompress(int fullsize, int uncompressedSize, byte[] compressedBuffer, int bytesCompress,
        CancellationToken token)
    {
        try
        {
            await BaseStream.WriteVarIntAsync(fullsize, token).ConfigureAwait(false);
            await BaseStream.WriteVarIntAsync(uncompressedSize, token).ConfigureAwait(false);
            //await BaseStream.WriteAsync(compressed.Memory, token);

            await BaseStream.WriteAsync(compressedBuffer.AsMemory(0, bytesCompress), token)
                .ConfigureAwait(false);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(compressedBuffer);
            await BaseStream.FlushAsync(token);
        }
    }

    public void SwitchCompression(int threshold)
    {
        _compressionThreshold = threshold;
    }

    #region Send

    public ValueTask SendPacketAsync(OutputPacket packet, CancellationToken token = default)
    {
        return SendPacketAsync(packet.Memory, token);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private async ValueTask SendPacketWithoutCompressionAsync(ReadOnlyMemory<byte> data, CancellationToken token)
    {
        var len = data.Length;

        await BaseStream.WriteVarIntAsync(len, token).ConfigureAwait(false);

        await BaseStream.WriteAsync(data, token).ConfigureAwait(false);
    }

    #endregion
}