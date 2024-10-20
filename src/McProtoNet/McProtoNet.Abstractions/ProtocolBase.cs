﻿using DotNext.Buffers;

namespace McProtoNet.Abstractions;

public abstract class ProtocolBase : IDisposable
{
    public int SupportedVersion { get; protected set; }
    
    protected readonly IPacketBroker _client;
    protected int ProtocolVersion => _client.ProtocolVersion;
    public ProtocolBase(IPacketBroker client)
    {
        _client = client;
        _client.StateChanged += ClientOnStateChanged;
        _client.Disposed += ClientOnDisposed;
        _client.PacketReceived += ClientPacketReceived;
    }

    private void ClientPacketReceived(object sender, InputPacket inputpacket)
    {
        OnPacketReceived(inputpacket);
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

    protected async ValueTask SendPacketCore(MemoryOwner<byte> owner)
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

    protected virtual void OnLoaded()
    {
        
    }

    protected virtual void OnStop()
    {
        
    }

    protected virtual void OnError(Exception exception)
    {
        
    }
    public virtual void Dispose()
    {
        
    }
}