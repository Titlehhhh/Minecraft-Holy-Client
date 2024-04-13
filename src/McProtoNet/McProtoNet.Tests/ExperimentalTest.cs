using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines;
using System.IO;
using System.Threading;
using McProtoNet.Core.Protocol;
using McProtoNet.Experimental;
using McProtoNet.Core;
using LibDeflate;
using System.IO.Compression;
using McProtoNet.Core.Protocol.Pipelines;
using Org.BouncyCastle.Crypto.IO;

namespace McProtoNet.Tests
{
	[TestClass]
	public class ExperimentalTest
	{
		[TestMethod]
		public async Task SenderTest()
		{


			byte[] for_parsing = new byte[300];

			int id = Random.Shared.Next(0, 100);

			Random.Shared.NextBytes(for_parsing);





			var ms = new MemoryStream();
			var originalSender = new MinecraftPacketSender();

			originalSender.SwitchCompression(128);

			originalSender.BaseStream = ms;

			var packetStream = new MemoryStream();

			//await packetStream.WriteVarIntAsync(id);
			await packetStream.WriteAsync(for_parsing);

			var buffer = packetStream.ToArray();
			packetStream.Position = 0;

			//await originalSender.SendPacketAsync(new (0, buffer.Length, buffer, null));
			await originalSender.SendPacketAsync(new Packet(id, packetStream));

			if (ms.Length != 1024)
			{
				//Assert.Fail(ms.Length.ToString());
			}


			var reader = new MinecraftPacketReader();
			reader.SwitchCompression(256);
			reader.BaseStream = ms;
			ms.Position = 0;
			var read = await reader.ReadNextPacketAsync();

			Assert.AreEqual(read.Id, id);

			//Assert.AreEqual(for_parsing.LongLength, read.Data.Length);



			CollectionAssert.AreEqual(for_parsing, read.Data.ToArray(), $"{read.Data.ToArray()}");
		}
		[TestMethod]
		public void CompressionTest()
		{

			byte[] original = new byte[1024];

			Random.Shared.NextBytes(original);

			using var compressor = new ZlibCompressor(6);

			//var compressed = compressor.Compress(original, true);



			using var compressed = compressor.Compress(original, true);


			var compressedArray = compressed.Memory.ToArray();

			using var ms = new MemoryStream();

			using (var zlib = new ZLibStream(ms, CompressionMode.Compress, true))
			{

				zlib.Write(original, 0, original.Length);
			}

			ms.Position = 0;

			var compressedArray2 = ms.ToArray();

			//Assert.Fail($"\n1: [{string.Join(", ", compressedArray)}]\n2: [{string.Join(", ", compressedArray2)}]");

			CollectionAssert.AreEqual(compressedArray, compressedArray2);

		}

		
		[TestMethod]
		public async Task TestPipelinesReader()
		{

			var mainStream = new MemoryStream();

			var sender = new MinecraftPacketSender();
			sender.BaseStream = mainStream;
			byte id = 0;

			List<(int, byte[])> original = new(1000000);

			for (int i = 0; i < 1_000_000; i++)
			{
				var data = new byte[Random.Shared.Next(50,1024)];

				Random.Shared.NextBytes(data);

				MemoryStream ms = new MemoryStream(data.ToArray());

				ms.Position = 0;

				int int_id = id++;

				original.Add((int_id, data));



				var packet = new Packet(int_id, ms);

				await sender.SendPacketAsync(packet);
			}
			mainStream.Position = 0;

			EmptyProcessor processor = new();

			var pipeReader = PipeReader.Create(mainStream);

			using PacketPipeReader packetReader = new PacketPipeReader(pipeReader, processor);



			await packetReader.RunAsync();


			Assert.AreEqual(processor.Count, 1_000_000, 1_000_000 - processor.Count);

			for (int i = 0; i < 1_000_000; i++)
			{
				var actual = processor.Packets[i];
				var expected = original[i];

				Assert.AreEqual<int>(actual.Item1, expected.Item1, message: $"Айди не совпадают у пакета номер {i}");
				CollectionAssert.AreEqual(actual.Item2, expected.Item2, message: $"Данные не совпадают у пакета номер {i}");
			}

		}

		[TestMethod]
		public async Task TestPipelinesReaderWithCompress()
		{

			var mainStream = new MemoryStream();

			var sender = new MinecraftPacketSender();
			sender.SwitchCompression(256);
			sender.BaseStream = mainStream;
			byte id = 0;

			const int countPackets = 1_000_000;

			List<(int, byte[])> original = new(countPackets);

			for (int i = 0; i < countPackets; i++)
			{
				var data = new byte[Random.Shared.Next(50, 1024)];

				Random.Shared.NextBytes(data);

				MemoryStream ms = new MemoryStream(data.ToArray());

				ms.Position = 0;

				int int_id = id++;

				original.Add((int_id, data));



				var packet = new Packet(int_id, ms);

				await sender.SendPacketAsync(packet);
			}
			mainStream.Position = 0;

			EmptyProcessor processor = new();

			var pipeReader = PipeReader.Create(mainStream);

			using PacketPipeReader packetReader = new PacketPipeReader(pipeReader, processor);
			packetReader.CompressionThreshold = 256;


			await packetReader.RunAsync();


			Assert.AreEqual(processor.Count, countPackets, countPackets - processor.Count);

			for (int i = 0; i < countPackets; i++)
			{
				var actual = processor.Packets[i];
				var expected = original[i];

				Assert.AreEqual<int>(actual.Item1, expected.Item1, message: $"Айди не совпадают у пакета номер {i}");
				
				
				CollectionAssert.AreEqual(actual.Item2, expected.Item2, message: $"Данные не совпадают у пакета номер {i} Прочитано: {actual.Item2.Length} Должно быть: {expected.Item2.Length}");
			}

		}


		public sealed class EmptyProcessor : IPacketProcessor
		{
			public int Count { get; private set; }
			public List<(int, byte[])> Packets { get; } = new();
			public void ProcessPacket(int id, ref ReadOnlySpan<byte> data)
			{
				Count++;


				byte[] copy = data.ToArray();

				Packets.Add((id, copy));

				
			}
		}


	}
}
