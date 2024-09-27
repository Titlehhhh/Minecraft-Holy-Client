using System.Buffers;
using System.IO.Pipelines;
using McProtoNet.Protocol.Zlib;

namespace McProtoNet.Protocol;

internal sealed class MinecraftPacketPipeWriter
{
    private static readonly byte[] ZeroVarInt = { 0 };
  
    private readonly PipeWriter pipeWriter;

    public MinecraftPacketPipeWriter(PipeWriter pipeWriter)
    {
        this.pipeWriter = pipeWriter;
      
    }

    public int CompressionThreshold { get; set; }

    public ValueTask<FlushResult> SendPacketAsync(ReadOnlyMemory<byte> data,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (CompressionThreshold < 0)
        {
            pipeWriter.WriteVarInt(data.Length);
            pipeWriter.Write(data.Span);
            return pipeWriter.FlushAsync(cancellationToken);
        }

        if (data.Length < CompressionThreshold)
        {
            pipeWriter.WriteVarInt(data.Length + 1);
            pipeWriter.WriteVarInt(0);

            return pipeWriter.WriteAsync(data, cancellationToken);
        }

        var uncompressedSize = data.Length;
        using scoped var compressor = new ZlibCompressor(); 
        var length = compressor.GetBound(uncompressedSize);

        var compressedBuffer = ArrayPool<byte>.Shared.Rent(length);

        try
        {
            var bytesCompress = compressor.Compress(data.Span, compressedBuffer.AsSpan(0, length));

            var compressedLength = bytesCompress;

            var fullsize = compressedLength + uncompressedSize.GetVarIntLength();

            pipeWriter.WriteVarInt(fullsize);
            pipeWriter.WriteVarInt(uncompressedSize);
            pipeWriter.Write(compressedBuffer.AsSpan(0, bytesCompress));
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(compressedBuffer);
        }

        return pipeWriter.FlushAsync(cancellationToken);
    }
}