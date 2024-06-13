using System.Runtime.CompilerServices;
using DotNext.Buffers;

namespace McProtoNet.Abstractions;

public readonly struct OutputPacket : IDisposable
{
    private readonly MemoryOwner<byte> owner;

    public ReadOnlyMemory<byte> Memory => owner.Memory;

    public ReadOnlySpan<byte> Span => owner.Span;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public OutputPacket(MemoryOwner<byte> owner)
    {
        this.owner = owner;
    }

    public void Dispose()
    {
        owner.Dispose();
    }
}