using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using DotNext;
using DotNext.IO;
using DotNext.IO.Pipelines;
using McProtoNet.Abstractions;

namespace McProtoNet.Protocol;

internal sealed class TransportHandler : Disposable
{
    private readonly IDuplexPipe duplexPipe;

    public TransportHandler(IDuplexPipe duplexPipe)
    {
        this.duplexPipe = duplexPipe;
    }

    public Stream BaseStream { get; set; }

    public event Action<InputPacket> GG;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var send = StartSendingAsync(cancellationToken);
        var receive = StartReceiveAsync(cancellationToken);
        return Task.WhenAll(send, receive);
    }

    public void Complete()
    {
        duplexPipe.Output.Complete();
        duplexPipe.Input.Complete();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: System.Threading.Tasks.Task`1[System.Byte[]]; size: 462MB")]
    private async Task StartReceiveAsync(CancellationToken cancellationToken)
    {
        var stream = BaseStream;
        var output = duplexPipe.Output;


        while (!cancellationToken.IsCancellationRequested)
            try
            {
                var buffer = output.GetMemory(4096);

                var bytesRead = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);

                if (bytesRead <= 0)
                    throw new EndOfStreamException();

                output.Advance(bytesRead);

                var result = await output.FlushAsync(cancellationToken).ConfigureAwait(false);
                result.ThrowIfCancellationRequested(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                await output.CompleteAsync();
                break;
            }
            catch (Exception ex)
            {
                await output.CompleteAsync(ex);
                break;
            }
    }

    private async Task StartSendingAsync(CancellationToken cancellationToken)
    {
        var input = duplexPipe.Input;
        var stream = BaseStream;

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await input.ReadAsync(cancellationToken);


            var buffer = result.Buffer;
            try
            {
                await stream.WriteAsync(buffer, cancellationToken);

                if (result.IsCanceled) break;

                if (result.IsCompleted) break;
            }
            finally
            {
                input.AdvanceTo(buffer.End);
            }
        }
    }
}