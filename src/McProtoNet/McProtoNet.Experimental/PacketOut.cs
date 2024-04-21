using System.Buffers;

namespace McProtoNet.Experimental
{

	public readonly struct PacketOut : IDisposable
	{
		private readonly int offset;
		private readonly int length;
		private readonly byte[] buffer;
		private readonly ArrayPool<byte>? pool;

		public byte[] Buffer => buffer;
		public int Offset => offset;
		public int Length => length;

		public PacketOut(int offset, int length, byte[] buffer, ArrayPool<byte>? pool)
		{
			this.offset = offset;
			this.length = length;
			this.buffer = buffer;
			this.pool = pool;
		}

		public void Dispose()
		{
			if (pool is not null)
				pool.Return(buffer);
		}

		public ReadOnlyMemory<byte> GetMemory()
		{
			return new ReadOnlyMemory<byte>(buffer, offset, length);
		}



	}
}
