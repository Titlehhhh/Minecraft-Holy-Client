using DotNext;
using DotNext.Buffers;
using DotNext.IO;
using LibDeflate;
using McProtoNet.Core;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
using McProtoNet.Experimental;
using Org.BouncyCastle.Utilities;
using QuickProxyNet;
using System.Buffers;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;


internal partial class Program
{

	private static async Task Main(string[] args)
	{
		//try
		//{
		//	PipelinesBenchmarks benchmarks = new();
		//	Console.WriteLine("setup");
		//	await benchmarks.Setup();
		//	Console.WriteLine("Start read");
		//	Console.WriteLine("stream1\n");
		//	await benchmarks.ReadStream1();
		//	Console.WriteLine("stream2\n");
		//	await benchmarks.ReadStream2();
		//	Console.WriteLine("Pipelines\n");
		//	await benchmarks.ReadWithPipelines();
			
		//	benchmarks.Clean();
		//}
		//catch (Exception ex)
		//{
		//	Console.WriteLine(ex);
		//}


	}







}
