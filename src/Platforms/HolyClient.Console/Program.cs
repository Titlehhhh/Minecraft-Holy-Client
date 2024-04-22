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
using System.Text;


internal partial class Program
{

	public static async Task Main(string[] args)
	{

		string url = "https://github.com/TheSpeedX/PROXY-List/raw/master/http.txt";

		HttpClient httpClient = new();

		string result = await httpClient.GetStringAsync(url);

		if (!Directory.Exists("proxies"))
			Directory.CreateDirectory("proxies");
		List<Task> tasks = new List<Task>();

		foreach (var item in result.Split("\n"))
		{
			try
			{
				string[] line = item.Trim().Split(":");
				string host = line[0];
				int port = int.Parse(line[1]);


				HttpProxyClient httpProxy = new(host, port);

				Task t = httpProxy.ConnectAsync("185.215.4.16", 5000, 443);

				tasks.Add(t);
			}
			catch { }
		}

		await Task.WhenAll(tasks);

	}







}
