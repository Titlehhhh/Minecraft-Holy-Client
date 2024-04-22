using BenchmarkDotNet.Attributes;
using McProtoNet.Experimental;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace McProtoNet.Benchmark
{
	[MemoryDiagnoser]
	public class CreateClassesBenchmarks
	{

		[Benchmark]
		public void CreateMinecraftClient_10k()
		{
			for (int i = 0; i < 10_000; i++)
			{
				MinecraftClient client = new();
				client.Dispose();
			}
		}
		[Benchmark]
		public void CreateSenderNew_10k()
		{
			for (int i = 0; i < 10_000; i++)
			{
				MinecraftPacketSenderNew sender = new();
				sender.Dispose();
			}
		}

		[Benchmark]
		public void CreatePacket_10k()
		{
			for (int i = 0; i < 10_000; i++)
			{
				TestPacket1 packet = new(1, 2, 3);
			}
		}
		[Benchmark]
		public void CreatePacketInterface_10k()
		{
			for (int i = 0; i < 10_000; i++)
			{
				var packet = new TestPacket(1, 2, 3);
				Send(packet);
			}
		}
	
		private static void Send(IPacket packet)
		{
			packet.Read();
			packet.Write();
		}
	}
	public readonly struct TestPacket : IPacket
	{
		public readonly int A;
		public readonly int B;
		public readonly int C;

		public TestPacket(int a, int b, int c)
		{
			A = a;
			B = b;
			C = c;
		}
		static int h = 0;
		void IPacket.Read()
		{
			
		}

		void IPacket.Write()
		{
			
		}
	}
	public readonly struct TestPacket1
	{
		public readonly int A;
		public readonly int B;
		public readonly int C;

		public TestPacket1(int a, int b, int c)
		{
			A = a;
			B = b;
			C = c;
		}


	}
	public interface IPacket
	{
		internal void Read();
		internal void Write();
	}


}
