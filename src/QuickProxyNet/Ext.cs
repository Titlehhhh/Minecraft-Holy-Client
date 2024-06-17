using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace QuickProxyNet;

public static class Ext
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static async ValueTask<int> ReadToEndAsync(this Stream stream, Memory<byte> buffer, int length,
        CancellationToken token)
    {
        var totalRead = 0;
        while (totalRead < length)
        {
            var read = await stream.ReadAsync(buffer.Slice(totalRead), token);
            if (read <= 0)
                throw new EndOfStreamException();

            totalRead += read;
        }

        return totalRead;
    }
}