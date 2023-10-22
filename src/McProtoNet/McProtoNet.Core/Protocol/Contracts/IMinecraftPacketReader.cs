namespace McProtoNet.Core.Protocol
{
    public interface IMinecraftPacketReader : ISwitchCompression,IDisposable,IAsyncDisposable
    {
        Packet ReadNextPacket();
        ValueTask<Packet> ReadNextPacketAsync(CancellationToken cancellationToken = default);
    }
}

