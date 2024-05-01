namespace McProtoNet.Client
{
	public readonly struct LoginOptions
	{
		public readonly string Host;
		public readonly ushort Port;
		public readonly int ProtocolVersion;
		public readonly string Username;
	}
}
