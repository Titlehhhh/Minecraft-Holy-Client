using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using McProtoNet.NBT;
using DotNext.IO;

namespace McProtoNet.Serialization;

/// <summary>
///     Represents stack-allocated reader for primitive types of Minecraft
/// </summary>
public ref struct MinecraftPrimitiveReaderSlim
{
    private SequenceReader<byte> reader;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MinecraftPrimitiveReaderSlim(SequenceReader<byte> reader)
    {
        this.reader = reader;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MinecraftPrimitiveReaderSlim(ReadOnlySequence<byte> data)
    {
        reader = new SequenceReader<byte>(data);
    }

    private void ThrowEndOfData()
    {
        throw new InvalidOperationException("End of data");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadVarInt()
    {
        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            if (!reader.TryRead(out read))
                ThrowEndOfData();


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
            if (!reader.TryRead(out read))
                ThrowEndOfData();


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
        if (!reader.TryRead(out var result))
            ThrowEndOfData();

        return result == 1;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadUnsignedByte()
    {
        if (!reader.TryRead(out var result))
            ThrowEndOfData();

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ReadSignedByte()
    {
        return (sbyte)ReadUnsignedByte();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUnsignedShort()
    {
        if (!reader.TryReadBigEndian(out short value))
            ThrowEndOfData();


        return (ushort)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadSignedShort()
    {
        return (short)ReadUnsignedShort();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadSignedInt()
    {
        if (!reader.TryReadBigEndian(out int value))
            ThrowEndOfData();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUnsignedInt()
    {
        return ReadUnsignedInt();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadSignedLong()
    {
        if (!reader.TryReadBigEndian(out long value))
            ThrowEndOfData();
        return value;
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
        byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
        try
        {
            return Encoding.UTF8.GetString(buffer);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
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
        return this.reader.UnreadSequence.ToArray();
    }

    public byte[] ReadBuffer(int length)
    {
        if (length > reader.Remaining)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "the buffer is less than the requested length");
        }

        this.reader.TryReadExact(length, out var result);

        return result.ToArray();
    }

    public NbtTag? ReadOptionalNbt()
    {
        if (ReadBoolean())
        {
            return ReadNbt();
        }

        return null;

    }

    public NbtTag ReadNbt()
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(reader.UnreadSequence.ToArray());
            ms.Position = 0;

            NbtReader nbtReader = new NbtReader(ms);

            NbtTag result = nbtReader.ReadAsTag();

            reader.Advance(ms.Position);
            return result;
        }
    }
}