using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;

namespace McProtoNet.Abstractions;

public readonly struct InputPacket : IDisposable
{
    public readonly int Id;
    public readonly Memory<byte> Data;

    private readonly MemoryOwner<byte> owner;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public InputPacket(MemoryOwner<byte> owner)
    {
        this.owner = owner;
        Id = ReadVarInt(owner.Span, out int offset);
        Data = this.owner.Memory.Slice(offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int ReadVarInt(Span<byte> data, out int len)
    {
        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            read = data[numRead];

            var value = read & 0b01111111;
            result |= value << (7 * numRead);

            numRead++;
            if (numRead > 5) throw new ArithmeticException("VarInt too long");
        } while ((read & 0b10000000) != 0);

        //data = data.Slice(numRead);


        len = numRead;
        return result;
    }

    public void Dispose()
    {
        owner.Dispose();
    }
}