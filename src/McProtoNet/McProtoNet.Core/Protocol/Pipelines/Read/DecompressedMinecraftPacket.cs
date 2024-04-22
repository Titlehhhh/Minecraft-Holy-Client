using System.Buffers;
using System.Runtime.CompilerServices;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public readonly struct DecompressedMinecraftPacket
	{
		public int Id { get; }
		public ReadOnlySequence<byte> Data { get; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DecompressedMinecraftPacket(int id, ReadOnlySequence<byte> data)
		{
			Id = id;
			Data = data;
		}
	}

}