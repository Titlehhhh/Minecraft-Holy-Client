namespace QuickProxyNet.ProxyChecker;

public static class ProxyChecker
{
    public static IProxyChecker CreateChunked(IEnumerable<ProxyRecord> proxies, ProxyCheckerChunkedOptions options)
    {
        options.Validate();
        return new ProxyCheckerChunk(proxies, options.ChunkSize, options.ConnectTimeout, options.TargetHost,
            options.TargetPort, options.SendAlive);
    }
}