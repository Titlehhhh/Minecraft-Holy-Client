using System.Diagnostics;
using System.IO.Pipelines;
using DotNext;
using LibDeflate;
using McProtoNet.Abstractions;

namespace McProtoNet.Protocol;

internal sealed class PacketPipeHandler : Disposable
{
    private readonly IDuplexPipe duplexPipe;
    private readonly MinecraftPacketPipeReader reader;
    private readonly MinecraftPacketPipeWriter writer;
    private int compressionThreshold;
    public PacketHandler PacketReceived;


    public PacketPipeHandler(IDuplexPipe duplexPipe, ZlibCompressor compressor, ZlibDecompressor decompressor)
    {
        this.duplexPipe = duplexPipe;

        reader = new MinecraftPacketPipeReader(duplexPipe.Input, decompressor);
        writer = new MinecraftPacketPipeWriter(duplexPipe.Output, compressor);
    }


    public int CompressionThreshold
    {
        get => compressionThreshold;
        set
        {
            reader.CompressionThreshold = value;
            writer.CompressionThreshold = value;
            compressionThreshold = value;
        }
    }

    public async ValueTask SendPacketAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
    {
        try
        {
            await writer.SendPacketAsync(data, cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            await duplexPipe.Input.CompleteAsync(ex);
        }
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            MinecraftPacketReader packetReader = new MinecraftPacketReader();
            packetReader.BaseStream = duplexPipe.Input.AsStream();
            packetReader.SwitchCompression(this.compressionThreshold);

            while (true)
            {
                var p = await packetReader.ReadNextPacketAsync(cancellationToken);
                PacketReceived?.Invoke(this,p);
            }
            // await foreach (var packet in reader.ReadPacketsAsync(cancellationToken))
            // {
            //     PacketReceived?.Invoke(this, packet);
            // }
        }
        catch (Exception ex)
        {
            duplexPipe.Output.CancelPendingFlush();
        }
        finally
        {
            await duplexPipe.Output.CompleteAsync();
        }
    }


    public void Complete()
    {
        duplexPipe.Output.Complete();
        duplexPipe.Input.Complete();
    }
}