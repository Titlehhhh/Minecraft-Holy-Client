namespace McProtoNet.Abstractions;

public interface IPacketBroker
{
    ValueTask SendPacket(ReadOnlyMemory<byte> data);

    event PacketHandler PacketReceived;

    event EventHandler<StateEventArgs> StateChanged;

    event Action Disposed;

    void Stop(Exception? customException = null);
    
    int ProtocolVersion { get; }
}