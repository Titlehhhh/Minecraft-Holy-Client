using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using CommandLine;

namespace McProtoNet.Benchmark;

public class Program
{
    public static async Task Main(string[] args)
    {
        var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}