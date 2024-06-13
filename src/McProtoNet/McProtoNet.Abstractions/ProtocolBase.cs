using DotNext.Buffers;

namespace McProtoNet.Abstractions;

public abstract class ProtocolBase
{
    private readonly IPacketBroker _client;

    public ProtocolBase(IPacketBroker client)
    {
        _client = client;
        _client.StateChanged += ClientOnStateChanged;
        _client.Disposed += ClientOnDisposed;
        _client.PacketReceived += ClientPacketReceived;
    }

    private void ClientPacketReceived(object sender, InputPacket inputpacket)
    {
        this.OnPacketReceived(inputpacket);
    }

    private void ClientOnDisposed()
    {
        _client.StateChanged -= ClientOnStateChanged;
        _client.Disposed -= ClientOnDisposed;
        _client.PacketReceived -= ClientPacketReceived;
    }

    private void ClientOnStateChanged(object? sender, StateEventArgs e)
    {
    }

    protected async Task SendPacketCore(MemoryOwner<byte> owner)
    {
        try
        {
            await _client.SendPacket(owner.Memory);
        }
        finally
        {
            owner.Dispose();
        }
    }

    protected virtual void OnPacketReceived(InputPacket packet)
    {
    }
}