using System;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using McProtoNet.Core.Protocol;
using QuickProxyNet;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser(true)]
	public class Benchmarks
	{




		private MemoryStream ms = new(10000);

		private byte[] small = new byte[64];
		private byte[] big = new byte[512];

		private MinecraftPacketSender sender;
		private MinecraftPacketSender senderWithCompress;


		private MinecraftPacketReader reader;
		

		private Packet smallPacket;
		private Packet bigPacket;

		private MemoryStream forReadSmall;
		private MemoryStream forReadBig;

		private MemoryStream forReadSmall_Compress;
		private MemoryStream forReadBig_Compress;

		[GlobalSetup]
		public async Task Setup()
		{

			Random.Shared.NextBytes(small);
			Random.Shared.NextBytes(big);

			reader = new();

			

			sender = new MinecraftPacketSender(ms);
			senderWithCompress = new MinecraftPacketSender(ms);

			senderWithCompress.SwitchCompression(256);



			smallPacket = new Packet(150, new MemoryStream(small)
			{
				Position = 0
			});
			bigPacket = new Packet(165, new MemoryStream(big)
			{
				Position = 0
			});

			var testSender = new MinecraftPacketSender();

			forReadSmall = new(1000);
			forReadBig_Compress = new(1000);
			forReadBig = new(1000);
			forReadSmall_Compress = new(1000);

			//for (int i = 0; i < 1_000_000; i++)
			{

				testSender.BaseStream = forReadBig;
				
				await testSender.SendPacketAsync(bigPacket);



				testSender.BaseStream = forReadSmall;
				
				await testSender.SendPacketAsync(smallPacket);





				testSender.BaseStream = forReadSmall_Compress;
				
				testSender.SwitchCompression(256);
				
				await testSender.SendPacketAsync(smallPacket);

				

				testSender.BaseStream = forReadBig_Compress;
				testSender.SwitchCompression(256);

				await testSender.SendPacketAsync(bigPacket);

				testSender.SwitchCompression(0);
			}

			forReadSmall.Position = 0;
			forReadBig.Position = 0;
			
			forReadSmall_Compress.Position = 0;
			forReadBig_Compress.Position = 0;

			testSender.BaseStream = null;

			

		}

		[GlobalCleanup]
		public void Clean()
		{
			try
			{
				ms.Dispose();
			}
			catch { }

			ms = null; 


			small = Array.Empty<byte>();
			big = Array.Empty<byte>();
			try
			{
				forReadBig.Dispose();
				forReadBig_Compress.Dispose();
				forReadSmall.Dispose();
				forReadSmall_Compress.Dispose();



				
			}
			catch
			{

			}

			forReadBig = null;
			forReadBig_Compress = null;
			forReadSmall = null;
			forReadSmall_Compress = null;

			try
			{
				smallPacket.Dispose();
			}
			catch
			{

			}

			try
			{
				bigPacket.Dispose();
			}
			catch
			{

			}
			

			
		}

		//[Benchmark]
		public async ValueTask Send_Small()
		{
			ms.Position = 0;
			await sender.SendPacketAsync(smallPacket);
		}

		//[Benchmark]
		public async ValueTask Send1M_Small()
		{
			for (int i = 0; i < 1000000; i++)
			{
				ms.Position = 0;
				await sender.SendPacketAsync(smallPacket);				
			}

		}

		//[Benchmark]
		public async ValueTask Send_Big()
		{
			ms.Position = 0;
			await sender.SendPacketAsync(bigPacket);
		}

		//[Benchmark]
		public async ValueTask Send1M_Big()
		{
			for (int i = 0; i < 1000000; i++)
			{
				ms.Position = 0;
				await sender.SendPacketAsync(bigPacket);				
			}

		}

		//[Benchmark]
		public ValueTask Send_Big_Compress()
		{
			return senderWithCompress.SendPacketAsync(bigPacket);
		}

		//[Benchmark]
		public async ValueTask Send1K_Big_Compress()
		{
			for (int i = 0; i < 1000; i++)
			{
				ms.Position = 0;
				await senderWithCompress.SendPacketAsync(bigPacket);				
			}

		}




		[Benchmark]
		public async ValueTask Read_Small()
		{
			forReadSmall.Position = 0;
			reader.BaseStream = forReadSmall;

			reader.SwitchCompression(0);
			var packet = await reader.ReadNextPacketAsync(default);

			packet.Dispose();

		}

		

		[Benchmark]
		public async ValueTask Read_Small_1M()
		{
			
			reader.BaseStream = forReadSmall;

			reader.SwitchCompression(0);
			for (int i = 0; i < 1000000; i++)
			{
				forReadSmall.Position = 0;

				var packet = await reader.ReadNextPacketAsync(default);

				packet.Dispose();
			}

		}

		[Benchmark]
		public async ValueTask Read_Big()
		{
			forReadBig.Position = 0;
			reader.BaseStream = forReadBig;
			reader.SwitchCompression(0);

			var packet = await reader.ReadNextPacketAsync(default);

			packet.Dispose();

		}

		[Benchmark]
		public async ValueTask Read_Big_1M()
		{
			
			reader.BaseStream = forReadBig;
			reader.SwitchCompression(0);
			for (int i = 0; i < 1000000; i++)
			{
				forReadBig.Position = 0;

				var packet = await reader.ReadNextPacketAsync(default);

				packet.Dispose();
			}
		}


		[Benchmark]
		public async ValueTask Read_Small_Compress()
		{
			forReadSmall_Compress.Position = 0;
			reader.BaseStream = forReadSmall_Compress;
			reader.SwitchCompression(256);

			var packet = await reader.ReadNextPacketAsync(default);
			
			packet.Dispose();
		}


		[Benchmark]
		public async ValueTask Read_Small_Compress_1M()
		{
			
			reader.BaseStream = forReadSmall_Compress;
			reader.SwitchCompression(256);
			for (int i = 0; i < 1000000; i++)
			{
				forReadSmall_Compress.Position = 0;

				var packet = await reader.ReadNextPacketAsync(default);

				packet.Dispose();
			}
		}


		[Benchmark]
		public async ValueTask Read_Big_Compress()
		{
			forReadBig_Compress.Position = 0;
			reader.BaseStream = forReadBig_Compress;
			reader.SwitchCompression(256);
			
			var packet = await reader.ReadNextPacketAsync(default);

			packet.Dispose();
		}

		[Benchmark]
		public async ValueTask Read_Big_Compress_1K()
		{
			
			reader.BaseStream = forReadBig_Compress;
			reader.SwitchCompression(256);
			for (int i = 0; i < 1000; i++)
			{
				forReadBig_Compress.Position = 0;

				var packet = await reader.ReadNextPacketAsync(default);

				packet.Dispose();
			}
		}


	}
}
