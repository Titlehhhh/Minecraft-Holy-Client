using System.Buffers;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using Cysharp.Text;
using DotNext.Buffers;
using McProtoNet.NBT;

namespace McProtoNet.Serialization;

/// <summary>
///     Represents stack-allocated writer for primitive types of Minecraft
/// </summary>
public ref struct MinecraftPrimitiveWriterSlim
{
    private static readonly MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

    private BufferWriterSlim<byte> writerSlim = new(64, s_allocator);

    public MinecraftPrimitiveWriterSlim()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBoolean(bool value)
    {
        writerSlim.Write(value ? 1 : 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedByte(sbyte value)
    {
        writerSlim.Write((byte)value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedByte(byte value)
    {
        writerSlim.Write(value);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedShort(ushort value)
    {
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedShort(short value)
    {
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedInt(int value)
    {
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedInt(uint value)
    {
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedLong(long value)
    {
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedLong(ulong value)
    {
        writerSlim.WriteBigEndian(value);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteFloat(float value)
    {
        var val = BitConverter.SingleToInt32Bits(value);
        writerSlim.WriteBigEndian(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteDouble(double value)
    {
        var val = BitConverter.DoubleToInt64Bits(value);
        writerSlim.WriteBigEndian(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUUID(Guid value)
    {
        var span = writerSlim.GetSpan(16);
        
        if (!value.TryWriteBytes(span)) throw new InvalidOperationException("Guid no write");
        writerSlim.Advance(16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBuffer(ReadOnlySpan<byte> value)
    {
        writerSlim.Write(value);
    }


    public void WriteVarInt(int? value)
    {
        if (value is null)
            throw new ArgumentNullException("value", "value is null");
        WriteVarInt(value.Value);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteVarInt(int value)
    {
        Span<byte> data = stackalloc byte[5];

        var unsigned = (uint)value;

        byte len = 0;
        do
        {
            var temp = (byte)(unsigned & 127);
            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;

            data[len++] = temp;
        } while (unsigned != 0);

        if (len > 5)
            throw new ArithmeticException("Var int is too big");

        writerSlim.Write(data.Slice(0, len));
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteVarLong(long value)
    {
        var unsigned = (ulong)value;

        do
        {
            var temp = (byte)(unsigned & 127);

            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;


            writerSlim.Write(temp);
        } while (unsigned != 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteString(string value)
    {
        var builder = ZString.CreateUtf8StringBuilder();
        try
        {
            builder.Append(value);
            WriteVarInt(builder.Length);
            writerSlim.Write(builder.AsSpan());
        }
        finally
        {
            builder.Dispose();
        }
    }

    public void WriteOptionalNbt(NbtTag? value)
    {
        if (value is null)
        {
            WriteBoolean(false);
        }
        else
        {
            WriteBoolean(true);
            WriteNbt(value);
        }
    }
    public void WriteNbt(NbtTag value)
    {
        MemoryStream ms = new MemoryStream();
        NbtWriter nbtWriter = new NbtWriter(ms, "");

        nbtWriter.WriteTag(value);

        this.WriteBuffer(ms.ToArray());
    }

    public void Clear(bool reuseBuffer = false)
    {
        writerSlim.Clear(reuseBuffer);
    }

    public MemoryOwner<byte> GetWrittenMemory()
    {
        if (!writerSlim.TryDetachBuffer(out var result))
            throw new InvalidOperationException("Don't detach buffer");
        return result;
    }

    
}