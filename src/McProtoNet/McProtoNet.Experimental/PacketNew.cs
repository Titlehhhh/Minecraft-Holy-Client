using System.Buffers;

namespace McProtoNet.Experimental
{
	public struct PacketNew : IDisposable
	{
		private readonly byte[] _buffer;
		private readonly ArrayPool<byte>? pool;
		private int _id;
		public PacketNew(int id, byte[] buffer, ArrayPool<byte> pool)
		{
			_id = id;
			this.pool = pool;
			this._buffer = buffer;
		}
		private readonly IMemoryOwner<byte>? owner;
		public PacketNew(IMemoryOwner<byte> owner)
		{
			this.owner = owner;
		}

		public void Dispose()
		{
			if (pool is not null)
				pool.Return(_buffer);
			if(this.owner is not null)
			{
				owner.Dispose();
			}
		}
	}
}
