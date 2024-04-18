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
using System.Diagnostics;

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


			var reader = new MinecraftPacketReaderNew();
			reader.SwitchCompression(256);
			reader.BaseStream = ms;
			ms.Position = 0;
			var read = await reader.ReadNextPacketAsync();

			Assert.AreEqual(read.Id, id);

			//Assert.AreEqual(for_parsing.LongLength, read.Data.Length);



			CollectionAssert.AreEqual(for_parsing, read.Span.ToArray(), $"{read.Span.ToArray()}");
		}
		
		
		


	}
}
