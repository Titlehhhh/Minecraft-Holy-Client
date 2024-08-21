using System.Buffers;
using System.Runtime.CompilerServices;

namespace McProtoNet.Experimental
{
	
	public readonly struct PacketNew 
	{
		private readonly byte[] _buffer;
		private readonly ArrayPool<byte>? pool;
		private readonly int _id;
		private readonly int offset;
		private readonly int length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal PacketNew(int id, byte[] buffer, ArrayPool<byte> pool)
		{
			_id = id;
			this.pool = pool;
			this._buffer = buffer;
			offset = 0;
			length = buffer.Length;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal PacketNew(int id, byte[] buffer, ArrayPool<byte> pool, int offset, int length)
		{
			_id = id;
			this.pool = pool;
			this._buffer = buffer;
			this.offset = offset;
			this.length = length;
		}
		public int Id => _id;


		public ReadOnlySpan<byte> Span
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _buffer.AsSpan(offset, length);
		}

		public ReadOnlyMemory<byte> Memory
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _buffer.AsMemory(offset, length);
		}



		public void Dispose()
		{
			if (pool is not null)
				pool.Return(_buffer);
		}
	}
}
