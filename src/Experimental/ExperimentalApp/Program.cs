using System.IO.Pipelines;

internal class Program
{
    private static async Task Main(string[] args)
    {
        CancellationTokenSource cts = new();
        var pipe = new Pipe();

        var data = new byte[10_000_000];

        Random.Shared.NextBytes(data);

        var ms = new MemoryStream(data);

        ms.Position = 0;


        var fill = FillStream(ms, pipe.Writer, cts.Token);
        var read = ReadStream(pipe.Reader, cts.Token);

        await Task.WhenAll(fill, read);
    }

    private static async Task ReadStream(PipeReader reader, CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                var result = await reader.ReadAsync(token);

                var buffer = result.Buffer;

                buffer = buffer.Slice(100);

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.Buffer.IsEmpty)
                {
                    Console.WriteLine("empty");
                    return;
                }

                if (result.IsCanceled)
                {
                    Console.WriteLine("Canel");
                    return;
                }
            }
        }
        finally
        {
            await reader.CompleteAsync();
        }
    }

    private static async Task FillStream(Stream stream, PipeWriter writer, CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                var buffer = writer.GetMemory(1024);

                var bytes = await stream.ReadAsync(buffer, token);

                if (bytes <= 0)
                {
                }

                writer.Advance(bytes);

                var flushResult = await writer.FlushAsync(token);

                if (flushResult.IsCanceled)
                {
                    Console.WriteLine("cancel");
                    return;
                }

                if (flushResult.IsCompleted)
                {
                    Console.WriteLine("Complete");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ReadEx: " + ex);
        }
        finally
        {
            Console.WriteLine("CompelteAsync");
            await writer.CompleteAsync();
        }
    }
}