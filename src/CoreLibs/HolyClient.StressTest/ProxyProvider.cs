using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Fody;
using QuickProxyNet;

namespace HolyClient.StressTest;

[ConfigureAwait(false)]
internal class ProxyProvider : IProxyProvider
{
    private bool _disposed;
    private CancellationTokenSource cts = new();
    private ChannelReader<IProxyClient> reader;

    public ProxyProvider(ChannelReader<IProxyClient> reader)
    {
        this.reader = reader;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async ValueTask<IProxyClient> GetNextProxy()
    {
        return await reader.ReadAsync(cts.Token);
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        cts.Cancel();
        cts.Dispose();
        cts = null;
        reader = null;

        GC.SuppressFinalize(this);
    }
}