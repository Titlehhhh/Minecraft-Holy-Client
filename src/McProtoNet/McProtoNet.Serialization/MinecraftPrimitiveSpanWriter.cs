using System.Buffers;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using Cysharp.Text;
using DotNext.Buffers;
using McProtoNet.NBT;

namespace McProtoNet.Serialization;

/// <summary>
///     Represents stack-allocated writer for primitive types of Minecraft
/// </summary>
[StructLayout(LayoutKind.Auto)]
public ref struct MinecraftPrimitiveSpanWriter
{
    private static readonly MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

    private BufferWriterSlim<byte> writerSlim = new(64, s_allocator);

    public MinecraftPrimitiveSpanWriter()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBoolean(bool value)
    {
        CheckDisposed();
        writerSlim.Write(value ? 1 : 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedByte(sbyte value)
    {
        CheckDisposed();
        writerSlim.Write((byte)value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedByte(byte value)
    {
        CheckDisposed();
        writerSlim.Write(value);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedShort(ushort value)
    {
        CheckDisposed();
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedShort(short value)
    {
        CheckDisposed();
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedInt(int value)
    {
        CheckDisposed();
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedInt(uint value)
    {
        CheckDisposed();
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteSignedLong(long value)
    {
        CheckDisposed();
        writerSlim.WriteBigEndian(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUnsignedLong(ulong value)
    {
        CheckDisposed();
        writerSlim.WriteBigEndian(value);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteFloat(float value)
    {
        CheckDisposed();
        var val = BitConverter.SingleToInt32Bits(value);
        writerSlim.WriteBigEndian(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteDouble(double value)
    {
        CheckDisposed();
        var val = BitConverter.DoubleToInt64Bits(value);
        writerSlim.WriteBigEndian(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteUUID(Guid value)
    {
        CheckDisposed();
        var span = writerSlim.GetSpan(16);

        if (!value.TryWriteBytes(span)) throw new InvalidOperationException("Guid no write");
        writerSlim.Advance(16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteBuffer(ReadOnlySpan<byte> value)
    {
        CheckDisposed();
        writerSlim.Write(value);
    }


    public void WriteVarInt(int? value)
    {
        CheckDisposed();
        if (value is null)
            throw new ArgumentNullException("value", "value is null");
        WriteVarInt(value.Value);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteVarInt(int value)
    {
        CheckDisposed();
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
        CheckDisposed();
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
        CheckDisposed();
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
        CheckDisposed();
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
        CheckDisposed();
        MemoryStream ms = new MemoryStream();
        NbtWriter nbtWriter = new NbtWriter(ms, "");

        nbtWriter.WriteTag(value);

        this.WriteBuffer(ms.ToArray());
    }

    public void Clear(bool reuseBuffer = false)
    {
        writerSlim.Clear(reuseBuffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(MinecraftPrimitiveSpanWriter));
    }

    private bool disposed;

    public MemoryOwner<byte> GetWrittenMemory()
    {
        if (!writerSlim.TryDetachBuffer(out var result))
            throw new InvalidOperationException("Don't detach buffer");
        disposed = true;
        return result;
    }

    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;

        writerSlim.Dispose();
        ;
    }
}