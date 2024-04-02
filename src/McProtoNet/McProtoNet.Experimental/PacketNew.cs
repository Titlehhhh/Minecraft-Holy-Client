using System.Buffers;

namespace McProtoNet.Experimental
{
	public struct PacketNew : IDisposable
	{
		private readonly byte[] _buffer;
		private readonly ArrayPool<byte> pool;
		private int _id;
		public PacketNew(int id, byte[] buffer, ArrayPool<byte> pool)
		{
			_id = id;
			this.pool = pool;
			this._buffer = buffer;
		}

		public void Dispose()
		{
			pool.Return(_buffer);
		}
	}
}
