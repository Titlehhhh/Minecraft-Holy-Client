namespace QuickProxyNet.ProxyChecker;

public struct ProxyCheckerChunkedOptions
{
    public required int ChunkSize { get; set; }
    public required int QueueSize { get; set; }
    public required TimeSpan ConnectTimeout { get; set; }
    public required bool IsSingleConsumer { get; set; }
    public required string TargetHost { get; set; }
    public required int TargetPort { get; set; }
    public required bool SendAlive { get; set; }

    public required bool InfinityLoop { get; set; } 

    internal void Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegative(QueueSize, nameof(QueueSize));
        ArgumentOutOfRangeException.ThrowIfNegative(ChunkSize, nameof(ChunkSize));
        ArgumentOutOfRangeException.ThrowIfNegative(TargetPort, nameof(TargetPort));
        ArgumentException.ThrowIfNullOrEmpty(TargetHost, nameof(TargetHost));
    }

    public override string ToString()
    {
        return
            $"{nameof(ChunkSize)}: {ChunkSize}, {nameof(QueueSize)}: {QueueSize}, {nameof(ConnectTimeout)}: {ConnectTimeout}, {nameof(IsSingleConsumer)}: {IsSingleConsumer}, {nameof(TargetHost)}: {TargetHost}, {nameof(TargetPort)}: {TargetPort}, {nameof(SendAlive)}: {SendAlive}, {nameof(InfinityLoop)}: {InfinityLoop}";
    }
}