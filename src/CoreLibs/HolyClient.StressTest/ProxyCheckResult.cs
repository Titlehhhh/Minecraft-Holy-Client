using QuickProxyNet;

namespace HolyClient.StressTest;

public struct ProxyCheckResult
{
    public bool Success { get; }
    public IProxyClient ProxyClient { get; }


    public ProxyCheckResult(bool success, IProxyClient proxyClient)
    {
        Success = success;
        ProxyClient = proxyClient;
    }
}