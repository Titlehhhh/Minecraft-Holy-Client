using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace McProtoNet.Serialization;

public static class ReadArraysSIMDExtensions
{
    public static int[] ReadArrayInt32BigEndian(this MinecraftPrimitiveSpanReader spanReader, int length) 
    {
        if (spanReader.RemainingCount < length)
        {
            throw new InsufficientMemoryException();
        }

        ReadOnlySpan<byte> bytes = spanReader.Read(sizeof(int) * length);
        ReadOnlySpan<int> ints = MemoryMarshal.Cast<byte, int>(bytes);
        if (BitConverter.IsLittleEndian)
        {
            int[] result = new int[length];
            BinaryPrimitives.ReverseEndianness(ints, result);
            return result;
        }
        return ints.ToArray();
    }
    public static long[] ReadArrayInt64BigEndian(this MinecraftPrimitiveSpanReader spanReader, int length) 
    {
        if (spanReader.RemainingCount < length)
        {
            throw new InsufficientMemoryException();
        }

        ReadOnlySpan<byte> bytes = spanReader.Read(sizeof(long) * length);
        ReadOnlySpan<long> ints = MemoryMarshal.Cast<byte, long>(bytes);
        if (BitConverter.IsLittleEndian)
        {
            long[] result = new long[length];
            BinaryPrimitives.ReverseEndianness(ints, result);
            return result;
        }
        return ints.ToArray();
    }
    public static short[] ReadArrayInt16BigEndian(this MinecraftPrimitiveSpanReader spanReader, int length) 
    {
        if (spanReader.RemainingCount < length)
        {
            throw new InsufficientMemoryException();
        }

        ReadOnlySpan<byte> bytes = spanReader.Read(sizeof(short) * length);
        ReadOnlySpan<short> ints = MemoryMarshal.Cast<byte, short>(bytes);
        if (BitConverter.IsLittleEndian)
        {
            short[] result = new short[length];
            BinaryPrimitives.ReverseEndianness(ints, result);
            return result;
        }
        return ints.ToArray();
    }
    public static ushort[] ReadArrayUnsignedInt16BigEndian(this MinecraftPrimitiveSpanReader spanReader, int length) 
    {
        if (spanReader.RemainingCount < length)
        {
            throw new InsufficientMemoryException();
        }

        ReadOnlySpan<byte> bytes = spanReader.Read(sizeof(ushort) * length);
        ReadOnlySpan<ushort> ints = MemoryMarshal.Cast<byte, ushort>(bytes);
        if (BitConverter.IsLittleEndian)
        {
            ushort[] result = new ushort[length];
            BinaryPrimitives.ReverseEndianness(ints, result);
            return result;
        }
        return ints.ToArray();
    }
    public static uint[] ReadArrayUnsignedInt32BigEndian(this MinecraftPrimitiveSpanReader spanReader, int length) 
    {
        if (spanReader.RemainingCount < length)
        {
            throw new InsufficientMemoryException();
        }

        ReadOnlySpan<byte> bytes = spanReader.Read(sizeof(uint) * length);
        ReadOnlySpan<uint> ints = MemoryMarshal.Cast<byte, uint>(bytes);
        if (BitConverter.IsLittleEndian)
        {
            uint[] result = new uint[length];
            BinaryPrimitives.ReverseEndianness(ints, result);
            return result;
        }
        return ints.ToArray();
    }
    public static ulong[] ReadArrayUnsignedInt64BigEndian(this MinecraftPrimitiveSpanReader spanReader, int length) 
    {
        if (spanReader.RemainingCount < length)
        {
            throw new InsufficientMemoryException();
        }

        ReadOnlySpan<byte> bytes = spanReader.Read(sizeof(ulong) * length);
        ReadOnlySpan<ulong> ints = MemoryMarshal.Cast<byte, ulong>(bytes);
        if (BitConverter.IsLittleEndian)
        {
            ulong[] result = new ulong[length];
            BinaryPrimitives.ReverseEndianness(ints, result);
            return result;
        }
        return ints.ToArray();
    }

}