namespace McProtoNet.Benchmark
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);


		}
	}
}