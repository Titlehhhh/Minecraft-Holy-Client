using System.Buffers;
using System.Runtime.CompilerServices;
using McProtoNet.Abstractions;
using McProtoNet.Net;

namespace McProtoNet;

public static class Extensions
{
    private static int SEGMENT_BITS = 0x7F;
    private static int CONTINUE_BIT = 0x80;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteVarInt(this IBufferWriter<byte> writer, int value)
    {
        if (value == 0)
        {
            writer.GetSpan(1)[0] = 0;
            writer.Advance(1);
            return;
        }


        Span<byte> data = stackalloc byte[5];

        var len = value.GetVarIntLength(data);

        writer.Write(data.Slice(0, len));
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadVarInt(this Span<byte> data, out int len)
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


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte GetVarIntLength(this int val)
    {
        byte amount = 0;
        do
        {
            val >>= 7;
            amount++;
        } while (val != 0);

        return amount;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte GetVarIntLength(this int value, byte[] data)
    {
        return GetVarIntLength(value, data, 0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte GetVarIntLength(this int value, byte[] data, int offset)
    {
        var unsigned = (uint)value;

        byte len = 0;
        do
        {
            var temp = (byte)(unsigned & 127);
            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;

            data[offset + len++] = temp;
        } while (unsigned != 0);

        if (len > 5)
            throw new ArithmeticException("Var int is too big");
        return len;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte GetVarIntLength(this int value, Span<byte> data, int offset)
    {
        var unsigned = (uint)value;

        byte len = 0;
        do
        {
            var temp = (byte)(unsigned & 127);
            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;

            data[offset + len++] = temp;
        } while (unsigned != 0);

        if (len > 5)
            throw new ArithmeticException("Var int is too big");
        return len;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte GetVarIntLength(this int value, Span<byte> data)
    {
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

        return len;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte GetVarIntLength(this int value, Memory<byte> data)
    {
        return GetVarIntLength(value, data.Span);
    }

    public static int ReadVarInt(this Stream stream)
    {
        Span<byte> buff = stackalloc byte[1];

        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            if (stream.Read(buff) <= 0) throw new EndOfStreamException();

            read = buff[0];


            var value = read & 0b01111111;
            result |= value << (7 * numRead);

            numRead++;
            if (numRead > 5) throw new InvalidOperationException("VarInt is too big");
        } while ((read & 0b10000000) != 0);

        return result;
    }

    public static async ValueTask<int> ReadVarIntAsync(this Stream stream, CancellationToken token = default)
    {
        var buff = ArrayPool<byte>.Shared.Rent(1);
        Memory<byte> memory = buff.AsMemory(0,1);
        try
        {
            var numRead = 0;
            var result = 0;
            byte read;
            do
            {
                await stream.ReadExactlyAsync(memory, token);

                read = buff[0];


                var value = read & 0b01111111;
                result |= value << (7 * numRead);

                numRead++;
                if (numRead > 5) throw new InvalidOperationException("VarInt is too big");
            } while ((read & 0b10000000) != 0);

            return result;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buff);
        }
    }


    public static int ReadVarInt(this Stream stream, out int len)
    {
        var buff = new byte[1];

        var numRead = 0;
        var result = 0;
        byte read;
        do
        {
            if (stream.Read(buff, 0, 1) <= 0)
                throw new EndOfStreamException();
            read = buff[0];


            var value = read & 0b01111111;
            result |= value << (7 * numRead);

            numRead++;
            if (numRead > 5) throw new InvalidOperationException("VarInt is too big");
        } while ((read & 0b10000000) != 0);

        len = (byte)numRead;
        return result;
    }

    public static void WriteVarInt(this Stream stream, int value)
    {
        var unsigned = (uint)value;

        do
        {
            var temp = (byte)(unsigned & 127);
            unsigned >>= 7;

            if (unsigned != 0)
                temp |= 128;

            stream.WriteByte(temp);
        } while (unsigned != 0);
    }

    public static ValueTask WriteVarIntAsync(this Stream stream, int value, CancellationToken token = default)
    {
        var unsigned = (uint)value;


        var data = ArrayPool<byte>.Shared.Rent(5);
        try
        {
            var len = 0;
            do
            {
                token.ThrowIfCancellationRequested();
                var temp = (byte)(unsigned & 127);
                unsigned >>= 7;

                if (unsigned != 0)
                    temp |= 128;
                data[len++] = temp;
            } while (unsigned != 0);

            return stream.WriteAsync(data.AsMemory(0, len), token);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(data);
        }
    }

    public static async ValueTask SendAndDisposeAsync(this MinecraftPacketSender sender, OutputPacket packet,
        CancellationToken token)
    {
        try
        {
            await sender.SendPacketAsync(packet, token);
        }
        finally
        {
            packet.Dispose();
        }
    }

}