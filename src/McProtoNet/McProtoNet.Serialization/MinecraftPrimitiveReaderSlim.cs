using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.NBT;

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
        //TODO
        throw new NotImplementedException();
    }

    public Guid ReadUUID()
    {
        //TODO
        throw new NotImplementedException();
    }

    public byte[] ReadRestBuffer()
    {
        //TODO
        throw new NotImplementedException();
    }

    public byte[] ReadBuffer(int length)
    {
        //TODO
        throw new NotImplementedException();
    }

    public NbtTag? ReadOptionalNbt()
    {
        throw new NotImplementedException();
    }

    public NbtTag ReadNbt()
    {
        throw new NotImplementedException();
    }
}