using BenchmarkDotNet.Attributes;
using System.IO;
using System.IO.Pipelines;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser]
	public class ProxyBenchmarks
	{
		private StreamPipeReaderOptions options = new StreamPipeReaderOptions(leaveOpen: true);
		[Benchmark]
		public void CreatePipeReaders()
		{
			MemoryStream ms = new();
			int g = 0;
			for (int i = 0; i < 5000; i++)
			{
				PipeReader pipeReader = PipeReader.Create(ms, options);
				try
				{
					g += i;
				}
				finally
				{					
					pipeReader.Complete();
				}
			}
		}
	}
}
