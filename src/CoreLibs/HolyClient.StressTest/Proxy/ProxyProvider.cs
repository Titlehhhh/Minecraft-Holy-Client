using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Fody;
using QuickProxyNet;
using QuickProxyNet.ProxyChecker;

namespace HolyClient.Proxy;

[ConfigureAwait(false)]
internal class ProxyProvider : IProxyProvider
{
    private bool _disposed;
    private CancellationTokenSource cts = new();
    private IProxyChecker reader;

    public ProxyProvider(IProxyChecker reader)
    {
        this.reader = reader;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public  ValueTask<IProxyClient> GetNextProxy()
    {
        return reader.GetNextProxy(cts.Token);
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        cts.Cancel();
        cts.Dispose();
        cts = null;

        GC.SuppressFinalize(this);
    }
}