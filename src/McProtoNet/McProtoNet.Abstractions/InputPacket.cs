using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;

namespace McProtoNet.Abstractions;

public readonly struct InputPacket : IDisposable
{
    public readonly int Id;
    public readonly ReadOnlySequence<byte> Data;

    private readonly MemoryOwner<byte>? owner;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public InputPacket(int id, ReadOnlySequence<byte> data)
    {
        Id = id;
        Data = data;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public InputPacket(int id, MemoryOwner<byte> owner, int offset = 0)
    {
        Id = id;
        this.owner = owner;
        Data = new ReadOnlySequence<byte>(owner.Memory.Slice(offset));
    }

    public void Dispose()
    {
        owner?.Dispose();
    }
}