namespace McProtoNet.Abstractions;

public interface IPacketBroker
{
    Task SendPacket(ReadOnlyMemory<byte> data);

    event PacketHandler PacketReceived;

    event EventHandler<StateEventArgs> StateChanged;

    event Action Disposed;
}