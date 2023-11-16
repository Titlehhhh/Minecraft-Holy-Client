namespace McProtoNet.Utils
{
	public struct SrvRecord
	{
		public string Host { get; private set; }
		public ushort Port { get; private set; }

		public SrvRecord(string host, ushort port)
		{
			Host = host;
			Port = port;
		}
	}

}