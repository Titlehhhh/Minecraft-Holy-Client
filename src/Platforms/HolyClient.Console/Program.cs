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
	private const int PacketsCount = 100_000;
	private static int CompressionThreshold = 128;
	private static async Task Main(string[] args)
	{
		Console.WriteLine("Start");
		{
			{
				using var mainStream = File.OpenWrite("data.bin");

				var sender = new MinecraftPacketSender();
				sender.SwitchCompression(CompressionThreshold);
				sender.BaseStream = mainStream;
				for (int i = 0; i < PacketsCount; i++)
				{
					var data = new byte[Random.Shared.Next(10, 512)];

					Random.Shared.NextBytes(data);

					MemoryStream ms = new MemoryStream(data);

					ms.Position = 0;


					var packet = new Packet(Random.Shared.Next(0, 60), ms);

					await sender.SendPacketAsync(packet);
				}
				await mainStream.FlushAsync();
			}

			
		}
		GC.Collect();
		Console.WriteLine("Start Reading");

		{
			using var fs = File.OpenRead("data.bin");

			using var reader = new MinecraftPacketReaderNew();
			reader.SwitchCompression(CompressionThreshold);
			reader.BaseStream = fs;
			for (int i = 0; i < PacketsCount; i++)
			{
				try
				{
					using var packet = await reader.ReadNextPacketAsync();
				}
				catch(Exception inner)
				{
					throw new Exception($"Packet: {i} error", inner);
				}
			}

		}
		Console.WriteLine("Start Reading Pipelines");
		{

			using var fs = File.OpenRead("data.bin");

			var reader = PipeReader.Create(fs);


			var p_reader = new PacketPipeReader(reader);
			//p_reader.CompressionThreshold = 128;

			await ProcessPackets(p_reader);

		}


	}
	private static async Task ProcessPackets(PacketPipeReader reader)
	{
		using ZlibDecompressor decompressor = new();

		await foreach (ReadOnlySequence<byte> data in reader.ReadPacketsAsync())
		{

			//SequenceReader<byte> reader1 = new SequenceReader<byte>(data);
			ReadOnlySequence<byte> mainData = default;
			byte[]? rented = null;
			if (CompressionThreshold > 0)
			{
				PacketPipeReader.TryReadVarInt(data, out int sizeUncompressed, out int len);
				ReadOnlySequence<byte> compressed = data.Slice(len);
				if (sizeUncompressed == 0)
				{

					PacketPipeReader.TryReadVarInt(compressed, out int id, out len);
					mainData = compressed.Slice(len);
				}
				else
				{
					byte[] decompressed = ArrayPool<byte>.Shared.Rent(sizeUncompressed);
					rented = decompressed;

					if (compressed.IsSingleSegment)
					{
						var result = decompressor.Decompress(
										compressed.FirstSpan,
										decompressed.AsSpan(0, sizeUncompressed),
										out int written);

						if (result != OperationStatus.Done)
							throw new Exception("Zlib");

						int id = PacketPipeReader.ReadVarInt(decompressed.AsSpan(), out len);



						mainData = new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);
					}
					else
					{
						mainData = DecompressCore(compressed, decompressed, decompressor, sizeUncompressed);

					}




				}

			}
			else
			{
				PacketPipeReader.TryReadVarInt(data, out int id, out int len);

				mainData = data.Slice(len);
			}


			if (rented is not null)
			{
				ArrayPool<byte>.Shared.Return(rented);
			}

		}

	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySequence<byte> DecompressCore(ReadOnlySequence<byte> compressed, byte[] decompressed, ZlibDecompressor decompressor, int sizeUncompressed)
	{
		//byte[] compressedTemp = ArrayPool<byte>.Shared.Rent((int)compressed.Length);

		int compressedLength = (int)compressed.Length;

		using scoped SpanOwner<byte> compressedTemp = compressedLength <= 256 ?
			new SpanOwner<byte>(stackalloc byte[compressedLength]) :
			new SpanOwner<byte>(compressedLength);

		scoped Span<byte> decompressedSpan = decompressed.AsSpan(0, sizeUncompressed);

		scoped Span<byte> compressedTempSpan = compressedTemp.Span;


		compressed.CopyTo(compressedTempSpan);




		var result = decompressor.Decompress(
					compressedTempSpan,
					decompressedSpan,
					out int written);

		if (result != OperationStatus.Done)
			throw new Exception("Zlib");

		int id = PacketPipeReader.ReadVarInt(decompressedSpan, out int len);



		return new ReadOnlySequence<byte>(decompressed, len, sizeUncompressed - len);

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
