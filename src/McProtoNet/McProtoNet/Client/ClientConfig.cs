using QuickProxyNet;

namespace McProtoNet
{
	public struct ClientConfig
	{
		public MinecraftVersion Version { get; set; }
		public string Username { get; set; }
		public string Host { get; set; }
		public ushort Port { get; set; }

		public string? HandshakeHost { get; set; }
		public ushort? HandshakePort { get; set; }

		//TODO AuthInfo

		public IProxyClient? Proxy { get; set; }
	}
}