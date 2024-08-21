using System.IO;
using System.IO.Pipelines;
using BenchmarkDotNet.Attributes;

namespace McProtoNet.Benchmark;

[MemoryDiagnoser]
public class ProxyBenchmarks
{
    private readonly StreamPipeReaderOptions options = new(leaveOpen: true);

    [Benchmark]
    public void CreatePipeReaders()
    {
        MemoryStream ms = new();
        var g = 0;
        for (var i = 0; i < 5000; i++)
        {
            var pipeReader = PipeReader.Create(ms, options);
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