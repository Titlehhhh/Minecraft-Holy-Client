using DotNext;
using LibDeflate;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
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
		Console.WriteLine("Start");
		{
			MemoryStream ms = new MemoryStream();
			var sender = new MinecraftPacketSender(ms);
			sender.SwitchCompression(128);
			for (int i = 0; i < 100_000; i++)
			{
				byte[] data = new byte[512];

				//Random.Shared.NextBytes(data);
				var ms2 = new MemoryStream(data);
				ms2.Position = 0;
				await sender.SendPacketAsync(new Packet(i, ms2));
			}


			



			ms.Position = 0;

			await File.WriteAllBytesAsync("data.bin", ms.ToArray());
		}
		GC.Collect();
		using var fs = File.OpenRead("data.bin");

		var reader = PipeReader.Create(fs);

		Console.WriteLine("Start Reading");
		var p_reader = new PacketPipeReader(reader);
		p_reader.CompressionThreshold = 128;
		int g = 0;
		await foreach (var item in p_reader.RunAsync())
		{
			g++;
			//if (g % 100 == 0)
				//Console.WriteLine(g);

			item.Dispose();
			//item.Dispose();
		}


	}

	private static void TestAes()
	{
		byte[] test = new byte[256];



		using Aes aes = Aes.Create();

		//var encryptor = aes.CreateEncryptor();

		//encryptor.TransformBlock()
	}

	private static void TestCompressDecompress()
	{
		MemoryStream ms = new();
		byte[] original = new byte[1000];
		byte b = 0;
		for (int i = 0; i < original.Length; i++)
		{
			original[i] = b++;
		}
		using (ZLibStream libStream = new ZLibStream(ms, CompressionMode.Compress, true))
		{
			libStream.Write(original);
		}
		ms.Position = 0;

		byte[] buffer = ms.ToArray();

		using DeflateDecompressor decompressor = new();

		Span<byte> span1 = stackalloc byte[100];
		buffer.AsSpan(0, 100).CopyTo(span1);

		Span<byte> span2 = stackalloc byte[buffer.Length - 100];

		buffer.AsSpan(100).CopyTo(span2);

		Span<byte> testBuffer = stackalloc byte[1000];

		var result = decompressor.Decompress(buffer, 1000, out IMemoryOwner<byte>? output);


	}


	public static async IAsyncEnumerable<int> Test()
	{
		Console.WriteLine("Start");
		for (int i = 0; i < 10; i++)
		{
			Console.WriteLine($"BeforeAwait: {i}");
			await Task.Delay(1000);
			Console.WriteLine($"AfterAwait: {i}");
			yield return i;
			Console.WriteLine($"Before Yield: {i}");
		}
	}






}
