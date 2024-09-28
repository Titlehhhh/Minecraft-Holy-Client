using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using DotNext.Buffers;

namespace McProtoNet.Benchmark;

[MemoryDiagnoser(true)]
public class ReadBigEndianBenchmarks
{
    public byte[] TestArr;
    private Random r = new();
    [Params(10, 100, 100000)] public int Count { get; set; }
   //public int Count = 10;
    [GlobalSetup]
    public void Setup()
    {
       
        Random r = new(71);
        r.NextBytes(MemoryMarshal.AsBytes(new Span<Vector128<byte>>(ref A128)));
        r.NextBytes(MemoryMarshal.AsBytes(new Span<Vector128<byte>>(ref B128)));
        r.NextBytes(MemoryMarshal.AsBytes(new Span<Vector256<byte>>(ref A256)));
        r.NextBytes(MemoryMarshal.AsBytes(new Span<Vector256<byte>>(ref B256)));
      // int Count = 10;
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
    
    [Benchmark]
    public long[] SimdReadUnsafe()
    {
        Span<long> numbers = MemoryMarshal.Cast<byte, long>(TestArr);
        long[] source = new long[Count];
        if (BitConverter.IsLittleEndian)
            BinaryPrimitivesTest.ReverseEndianness(numbers, source);
        return source;
    }
    public Vector128<byte> A128;
    public Vector128<byte> B128;

    public Vector256<byte> A256;
    public Vector256<byte> B256;
    //[Benchmark]
    public Vector128<byte> Shuffle128()
    {
        return Vector128.Shuffle(A128, B128);
    }
    //[Benchmark]
    public Vector128<byte> ShuffleUnsafe128()
    {
        return Impl.ShuffleUnsafe(A128, B128);
    }
    
    
}