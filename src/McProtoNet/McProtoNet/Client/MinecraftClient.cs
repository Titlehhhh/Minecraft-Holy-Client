using DotNext;
using DotNext.Threading;
using LibDeflate;
using McProtoNet.Protocol;
using System.IO.Pipelines;

namespace McProtoNet.Client
{

	internal delegate void MinecraftPacketHandler(InputPacket packet);
	public sealed class ClientContext
	{
		public ZlibCompressor Compressor { get; }
		public ZlibDecompressor Decompressor { get; }

		public IDuplexPipe Pipe { get; }

		public int CompressionThreshold { get; set; }
		public int ProtocolVersion { get; set; }
	}

	public interface IPacketChannel
	{
		IObservable<InputPacket> OnPacket { get; }

		ValueTask<OutputPacket> SendPacketAsync();
	}



	public sealed partial class MinecraftClient : Disposable
	{

		public MinecraftClient()
		{


		}
	}
}
