using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using DotNext.Buffers;

namespace McProtoNet.Benchmark;

[MemoryDiagnoser(true)]
public class ReadBigEndianBenchmarks
{
    public byte[] TestArr;
    private Random r = new();
    [Params(10, 100, 100000)] public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        TestArr = new byte[sizeof(long) * Count];
       
        scoped SpanWriter<byte> writer = new SpanWriter<byte>(TestArr);
        for (int i = 0; i < Count; i++)
        {
            long v = r.NextInt64();
            writer.WriteBigEndian(v);
        }
    }


    [Benchmark]
    public long[] SpanReader()
    {
        scoped SpanReader<byte> reader = new SpanReader<byte>(TestArr);
        long[] source = new long[Count];
        for (int i = 0; i < Count; i++)
        {
            source[i] = reader.ReadBigEndian<long>();
        }

        return source;
    }

    [Benchmark]
    public long[] SimdRead()
    {
        Span<long> numbers = MemoryMarshal.Cast<byte, long>(TestArr);
        long[] source = new long[Count];
        if (BitConverter.IsLittleEndian)
            BinaryPrimitives.ReverseEndianness(numbers, source);
        return source;
    }
}