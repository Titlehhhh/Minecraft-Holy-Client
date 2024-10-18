using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Unicode;
using DotNext;
using DotNext.Buffers;
using McProtoNet.NBT;
using DotNext.IO;

namespace McProtoNet.Serialization;

/// <summary>
///     Represents stack-allocated reader for primitive types of Minecraft
/// </summary>
[StructLayout(LayoutKind.Auto)]
public ref partial struct MinecraftPrimitiveSpanReader
{
    private SpanReader<byte> _reader;
    private bool disposed;
    public ReadOnlySpan<byte> Span => _reader.Span;

    public ref byte Current => ref Unsafe.AsRef(in _reader.Current);


    public int ConsumedCount => _reader.ConsumedCount;

    public readonly int RemainingCount => _reader.RemainingCount;
    public readonly ReadOnlySpan<byte> ConsumedSpan => _reader.ConsumedSpan;
    public readonly ReadOnlySpan<byte> RemainingSpan => _reader.RemainingSpan;

    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MinecraftPrimitiveSpanReader(ReadOnlySpan<byte> data)
    {
        _reader = new SpanReader<byte>(data);
    }

    public MinecraftPrimitiveSpanReader(ReadOnlyMemory<byte> data) : this(data.Span)
    {
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Advance(int count)
    {
        _reader.Advance(count);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> Read(int count) => _reader.Read(count);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Read(Span<byte> output) => _reader.Read(output);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(MinecraftPrimitiveSpanWriter));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadVarInt()
    {
        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            read = _reader.Read();
            var value = read & 127;
            result |= value << (7 * numRead);

            numRead++;
            if (numRead > 5) throw new ArithmeticException("VarInt too long");
        } while ((read & 0b10000000) != 0);

        return result;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadVarLong()
    {
        var numRead = 0;
        long result = 0;
        byte read;
        do
        {
            read = _reader.Read();


            var value = read & 127;
            result |= (long)value << (7 * numRead);

            numRead++;
            if (numRead > 10) throw new ArithmeticException("VarLong too long");
        } while ((read & 0b10000000) != 0);

        return result;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ReadBoolean()
    {
        return _reader.Read() == 1;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadUnsignedByte()
    {
        return _reader.Read();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSignedByte()
    {
        return (sbyte)ReadUnsignedByte();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUnsignedShort()
    {
        return _reader.ReadBigEndian<ushort>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadSignedShort()
    {
        return (short)ReadUnsignedShort();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadSignedInt()
    {
        return _reader.ReadBigEndian<int>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUnsignedInt()
    {
        return ReadUnsignedInt();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadSignedLong()
    {
        return _reader.ReadBigEndian<long>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUnsignedLong()
    {
        return (ulong)ReadSignedLong();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadFloat()
    {
        var value = ReadSignedInt();

        return Unsafe.BitCast<int, float>(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble()
    {
        var value = ReadSignedLong();

        return Unsafe.BitCast<long, double>(value);
    }

    public string ReadString()
    {
        int len = ReadVarInt();

        if (len > 0)
        {
            return Encoding.UTF8.GetString(_reader.Read(len));
        }

        return "";
    }


    public unsafe Guid ReadUUID()
    {
        long x = ReadSignedLong();
        long y = ReadSignedLong();

        long* ptr = stackalloc long[2];
        ptr[0] = x;
        ptr[1] = y;
        return *(Guid*)ptr;
    }

    public byte[] ReadRestBuffer()
    {
        return _reader.ReadToEnd().ToArray();
    }

    public byte[] ReadBuffer(int length)
    {
        return _reader.Read(length).ToArray();
    }

    public NbtTag? ReadOptionalNbt(bool readRootTag)
    {
        if (ReadBoolean())
        {
            return ReadNbt(readRootTag);
        }

        return null;
    }


    public NbtTag ReadNbt(bool readRootTag)
    {
        NbtSpanReader nbtSpanReader = new NbtSpanReader(_reader.RemainingSpan);
        NbtTag result = nbtSpanReader.ReadAsTag<NbtTag>(readRootTag);

        _reader.Advance(nbtSpanReader.ConsumedCount);
        return result;
    }

    
}