using System.Buffers;
using System.Runtime.CompilerServices;


namespace McProtoNet.Core.Protocol.Pipelines
{
	public readonly struct InputPacket
	{
		public readonly int Id;
		public readonly ReadOnlySequence<byte> Data;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public InputPacket(int id, ReadOnlySequence<byte> data)
		{
			Id = id;
			Data = data;
		}
	}

}