namespace McProtoNet.Abstractions;

public interface IPacketBroker
{
    ValueTask SendPacket(ReadOnlyMemory<byte> data);

    event PacketHandler PacketReceived;

    event EventHandler<StateEventArgs> StateChanged;

    event Action Disposed;

    void StopWithError(Exception ex);
    
    int ProtocolVersion { get; }
}