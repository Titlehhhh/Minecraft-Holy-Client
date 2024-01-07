namespace McProtoNet.Core.Protocol
{
	public struct Packet : IDisposable
	{
		public readonly int Id;
		public MemoryStream Data;
		private IDisposable? memory;

		public Packet(int id, MemoryStream data)
		{
			Id = id;
			Data = data;			
		}
		
		private bool _disposed = false;
		public void Dispose()
		{
			if (_disposed)
				return;
			_disposed = true;
			Data.Dispose();
			
			
			GC.SuppressFinalize(this);
		}
	}


}
