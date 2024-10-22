namespace QuickProxyNet.ProxyChecker;

public static class ProxyChecker
{
    public static IProxyChecker CreateChunked(IEnumerable<ProxyRecord> proxies, ProxyCheckerChunkedOptions options)
    {
        options.Validate();
        return new ProxyCheckerChunk(proxies, options);
    }
}