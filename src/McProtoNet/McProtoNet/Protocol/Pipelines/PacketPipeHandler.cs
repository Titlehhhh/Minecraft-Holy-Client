using System.IO.Pipelines;
using DotNext;
using McProtoNet.Abstractions;

namespace McProtoNet.Protocol;

internal sealed class PacketPipeHandler : Disposable
{
    private readonly IDuplexPipe duplexPipe;
    private readonly MinecraftPacketPipeReader reader;
    private readonly MinecraftPacketPipeWriter writer;
    private int compressionThreshold;
    public PacketHandler PacketReceived;


    public PacketPipeHandler(IDuplexPipe duplexPipe)
    {
        this.duplexPipe = duplexPipe;

        reader = new MinecraftPacketPipeReader(duplexPipe.Input);
        writer = new MinecraftPacketPipeWriter(duplexPipe.Output);
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
            await foreach (var packet in reader.ReadPacketsAsync(cancellationToken))
                PacketReceived?.Invoke(this, packet);
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