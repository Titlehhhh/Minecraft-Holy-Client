using LibDeflate;
using McProtoNet.Core.Protocol;
using McProtoNet.Core.Protocol.Pipelines;
using Org.BouncyCastle.Utilities;
using System.Buffers;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Security.Cryptography;


internal partial class Program
{



	private static async Task Main(string[] args)
	{



		TestAes();

		TestCompressDecompress();



		MemoryStream ms = new MemoryStream();
		var sender = new MinecraftPacketSender(ms);
		sender.SwitchCompression(256);
		for (int i = 0; i < 10_000; i++)
		{
			byte[] data = new byte[Random.Shared.Next(10, 300)];

			Random.Shared.NextBytes(data);
			var ms2 = new MemoryStream(data);
			ms2.Position = 0;
			await sender.SendPacketAsync(new Packet(i, ms2));
		}






		ms.Position = 0;



		var reader = PipeReader.Create(ms);

		var processor = new TestProcessor();

		var p_reader = new PacketPipeReader(reader, processor);
		p_reader.CompressionThreshold = 256;
		var t = p_reader.RunAsync();

		await Task.Delay(-1);
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

	class TestProcessor : IPacketProcessor
	{
		public void ProcessPacket(int id, ref ReadOnlySpan<byte> data)
		{

			//throw new NotImplementedException();
		}
	}






}
