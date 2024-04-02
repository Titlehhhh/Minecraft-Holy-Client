using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;


internal partial class Program
{

	private static int count = 0;

	private static unsafe void Main(string[] args)
	{
		Console.WriteLine(Sse.IsSupported);
		Console.WriteLine(Sse2.IsSupported);
		Console.WriteLine(Sse3.IsSupported);
		Console.WriteLine(Sse41.IsSupported);
		Console.WriteLine(Sse42.IsSupported);
		Console.WriteLine(Ssse3.IsSupported);

		Console.WriteLine("avx");

		Console.WriteLine(Avx.IsSupported);
		Console.WriteLine(Avx2.IsSupported);
		Console.WriteLine(Avx512BW.IsSupported);
		Console.WriteLine(Avx512CD.IsSupported);
		Console.WriteLine(Avx512DQ.IsSupported);
		Console.WriteLine(Avx512F.IsSupported);
		Console.WriteLine(AvxVnni.IsSupported);
		
		
		

		int a = 1005;

		int* a_ptr = &a;

		

		var vector = Sse3.LoadVector128(a_ptr);

		

		var str = vector.ToString();

		Console.WriteLine(str);

		Console.WriteLine(Vector128.IsHardwareAccelerated);
		//Console.WriteLine(Vector256.IsHardwareAccelerated);
		//Console.WriteLine(Vector512.IsHardwareAccelerated);

	}






}
