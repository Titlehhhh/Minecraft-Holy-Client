namespace McProtoNet.Client
{
	public readonly struct LoginOptions
	{
		public readonly string Host;
		public readonly ushort Port;
		public readonly int ProtocolVersion;
		public readonly string Username;

		public LoginOptions(string host, ushort port, int protocolVersion, string username)
		{
			Host = host;
			Port = port;
			ProtocolVersion = protocolVersion;
			Username = username;
		}
	}
}
