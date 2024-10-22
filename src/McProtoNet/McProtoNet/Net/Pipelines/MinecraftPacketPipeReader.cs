using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Abstractions;
using McProtoNet.Net.Zlib;

namespace McProtoNet.Net;

internal sealed class MinecraftPacketPipeReader
{
    private readonly PipeReader pipeReader;

    public MinecraftPacketPipeReader(PipeReader pipeReader)
    {
        this.pipeReader = pipeReader;
        //this.decompressor = decompressor;
    }

    public int CompressionThreshold { get; set; }

    public async IAsyncEnumerable<InputPacket> ReadPacketsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chunkcount = 0;
        cancellationToken.ThrowIfCancellationRequested();
        while (!cancellationToken.IsCancellationRequested)
        {
            ReadResult result = default;
            try
            {
                result = await pipeReader.ReadAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                await pipeReader.CompleteAsync();
                break;
            }

            var buffer = result.Buffer;
            var consumed = buffer.Start;
            var examined = buffer.End;

            if (result.IsCompleted) break;

            if (result.IsCanceled) break;


            try
            {
                while (TryReadPacket(ref buffer, out var packet))
                {
                    //consumed = buffer.Start;
                    //examined = consumed;
                    yield return Decompress(packet);
                }
            }
            finally
            {
                pipeReader.AdvanceTo(buffer.Start, buffer.End);
            }
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> packet)
    {
        scoped SequenceReader<byte> reader = new(buffer);


        packet = ReadOnlySequence<byte>.Empty;

        if (buffer.Length < 1) return false; // Недостаточно данных для чтения заголовка пакета

        int length;
        int bytesRead;
        if (!reader.TryReadVarInt(out length, out bytesRead)) return false; // Невозможно прочитать длину заголовка


        if (length > reader.Remaining) return false; // Недостаточно данных для чтения полного пакета


        packet = reader.UnreadSequence.Slice(0, length);

        reader.Advance(length);


        buffer = buffer.Slice(reader.Position);

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private InputPacket Decompress(in ReadOnlySequence<byte> data)
    {
        byte[]? rented = null;
        try
        {
            var id = -1;
            ReadOnlySequence<byte> mainData = default;

            if (CompressionThreshold > 0)
            {
                data.TryReadVarInt(out var sizeUncompressed, out var len);


                var compressed = data.Slice(len);
                if (sizeUncompressed > 0)
                {
                    if (sizeUncompressed < CompressionThreshold)
                        throw new Exception("Размер несжатого пакета меньше порога сжатия.");

                    var decompressed = ArrayPool<byte>.Shared.Rent(sizeUncompressed);
                    rented = decompressed;


                    using scoped var decompressor = new ZlibDecompressor();
                    if (compressed.IsSingleSegment)
                    {
                        var result = decompressor.Decompress(
                            compressed.FirstSpan,
                            decompressed.AsSpan(0, sizeUncompressed),
                            out var written);

                        if (result != OperationStatus.Done)
                            throw new Exception("Zlib: " + result);


                        id = decompressed.AsSpan().ReadVarInt(out len);


                        mainData = new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);
                    }
                    else
                    {
                        mainData = DecompressMultiSegment(compressed, decompressed, decompressor, sizeUncompressed,
                            out id);
                    }
                }
                else if (sizeUncompressed == 0)
                {
                    compressed.TryReadVarInt(out id, out len);
                    mainData = compressed.Slice(len);
                }
                else
                {
                    throw new InvalidOperationException($"sizeUncompressed negative: {sizeUncompressed}");
                }
            }
            else
            {
                data.TryReadVarInt(out id, out var len);

                mainData = data.Slice(len);
            }

            MemoryOwner<byte> gg = ArrayPool<byte>.Shared.ToAllocator()
                .AllocateExactly((int)mainData.Length);
            mainData.ToArray().AsSpan().CopyTo(gg.Span);

            return new InputPacket(id, gg);
        }
        finally
        {
            if (rented is not null) ArrayPool<byte>.Shared.Return(rented);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlySequence<byte> DecompressMultiSegment(ReadOnlySequence<byte> compressed, byte[] decompressed,
        scoped ZlibDecompressor decompressor, int sizeUncompressed, out int id)
    {
        var compressedLength = (int)compressed.Length;

        using scoped var compressedTemp = compressedLength <= 256
            ? new SpanOwner<byte>(stackalloc byte[compressedLength])
            : new SpanOwner<byte>(compressedLength);

        scoped var decompressedSpan = decompressed.AsSpan(0, sizeUncompressed);

        scoped var compressedTempSpan = compressedTemp.Span;


        compressed.CopyTo(compressedTempSpan);


        var result = decompressor.Decompress(
            compressedTempSpan,
            decompressedSpan,
            out var written);

        if (result != OperationStatus.Done)
            throw new Exception("Zlib: " + sizeUncompressed);

        id = decompressedSpan.ReadVarInt(out var len);

        return new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);
    }
}