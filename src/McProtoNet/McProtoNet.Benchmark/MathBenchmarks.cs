using BenchmarkDotNet.Attributes;
using System;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;

namespace McProtoNet.Benchmark
{

	public class MathBenchmarks
	{
		private int[] data = new int[2048];
		[GlobalSetup]
		public void Setup()
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = 10;
				
			}
		}

		public int Sum(ReadOnlySpan<int> source)
		{
			int result = 0;

			for (int i = 0; i < source.Length; i++)
			{
				result += source[i];
			}

			return result;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int SumVectorized(ReadOnlySpan<int> source)
		{			
			if (Sse2.IsSupported)
			{
				return SumVectorizedSse2(source);
			}
			else
			{
				return SumVectorT(source);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int SumVectorT(ReadOnlySpan<int> source)
		{
			int result = 0;

			Vector<int> vresult = Vector<int>.Zero;

			int i = 0;
			int lastBlockIndex = source.Length - (source.Length % Vector<int>.Count);

			while (i < lastBlockIndex)
			{
				vresult += new Vector<int>(source.Slice(i));
				i += Vector<int>.Count;
			}

			for (int n = 0; n < Vector<int>.Count; n++)
			{
				result += vresult[n];
			}

			while (i < source.Length)
			{
				result += source[i];
				i += 1;
			}

			return result;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe int SumVectorizedSse2(ReadOnlySpan<int> source)
		{
			int result;

			fixed (int* pSource = source)
			{
				Vector128<int> vresult = Vector128<int>.Zero;

				int i = 0;
				int lastBlockIndex = source.Length - (source.Length % 4);

				while (i < lastBlockIndex)
				{
					vresult = Sse2.Add(vresult, Sse2.LoadVector128(pSource + i));
					i += 4;
				}
				if (Ssse3.IsSupported)
				{
					vresult = Ssse3.HorizontalAdd(vresult, vresult);
					vresult = Ssse3.HorizontalAdd(vresult, vresult);
				}
				else
				{
					vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0x4E));
					vresult = Sse2.Add(vresult, Sse2.Shuffle(vresult, 0xB1));
				}
				result = vresult.ToScalar();

				while (i < source.Length)
				{
					result += pSource[i];
					i += 1;
				}
			}

			return result;
		}


		[Benchmark]
		public void SumNative()
		{
			Sum(data);
		}
		[Benchmark]
		public void SumPerformance()
		{
			SumVectorized(data);
		}


		[Benchmark]
		public void Shift()
		{
			
			//Vector128<byte> vector = Vector128.Create<>
		}

	}

}
