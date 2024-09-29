namespace QuickProxyNet.ProxyChecker;

public struct ProxyCheckerChunkedOptions
{
    public int ChunkSize { get; set; }
    public int ConnectTimeout { get; set; }
    public bool IsSingleConsumer { get; set; }
    public string TargetHost { get; set; }
    public int TargetPort { get; set; }
    public bool SendAlive { get; set; }

    internal void Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ChunkSize, nameof(ChunkSize));
        ArgumentOutOfRangeException.ThrowIfNegative(ConnectTimeout,nameof(ConnectTimeout));
        ArgumentOutOfRangeException.ThrowIfNegative(TargetPort, nameof(TargetPort));
        ArgumentException.ThrowIfNullOrEmpty(TargetHost, nameof(TargetHost));
    }

    public override string ToString()
    {
        return
            $"{nameof(ChunkSize)}: {ChunkSize}, {nameof(ConnectTimeout)}: {ConnectTimeout}, {nameof(IsSingleConsumer)}: {IsSingleConsumer}, {nameof(TargetHost)}: {TargetHost}, {nameof(TargetPort)}: {TargetPort}, {nameof(SendAlive)}: {SendAlive}";
    }
}