using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace McProtoNet.Benchmark;

[MemoryDiagnoser]
public class ReadPacketBenchmarks
{
    [GlobalSetup]
    public async Task Setup()
    {
        Random r = new Random(73);
        using (var fs = File.OpenWrite("data.bin"))
        {
            for (int i = 0; i < 100_000; i++)
            {
                int len = r.Next(5, 100);
                byte[] data = new byte[len];
                r.NextBytes(data);
                await WriteVarIntAsync(fs, len);
                await fs.WriteAsync(data);
            }
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
    }

    [Benchmark]
    public async Task ReadPacketsOld()
    {
        await using var fs = File.OpenRead("data.bin");
        for (int i = 0; i < 100_000; i++)
        {
            int len = await ReadVarIntAsync(fs);
            byte[] buffer = new byte[len];
            await fs.ReadExactlyAsync(buffer);
        }
    }
    [Benchmark]
    public async Task ReadPacketsNew()
    {
        await using var fs = File.OpenRead("data.bin");

        for (int i = 0; i < 100_000; i++)
        {
            int len = await ReadVarIntAsync(fs);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
            try
            {
                await fs.ReadExactlyAsync(buffer, 0, len);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }


    public static async ValueTask<int> ReadVarIntAsync(Stream stream, CancellationToken token = default)
    {
        var buff = ArrayPool<byte>.Shared.Rent(1);
        Memory<byte> memory = buff.AsMemory(0, 1);
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


    public static ValueTask WriteVarIntAsync(Stream stream, int value, CancellationToken token = default)
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
}