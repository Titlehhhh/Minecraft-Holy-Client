using DotNext.Buffers;
using System.Runtime.CompilerServices;

namespace McProtoNet.Protocol
{

	public readonly struct OutputPacket : IDisposable
	{
		private readonly MemoryOwner<byte> owner;

		public ReadOnlyMemory<byte> Memory => owner.Memory;

		public ReadOnlySpan<byte> Span => owner.Span;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public OutputPacket(MemoryOwner<byte> owner)
		{
			this.owner = owner;
		}

		public void Dispose()
		{
			owner.Dispose();
		}



	}
}
