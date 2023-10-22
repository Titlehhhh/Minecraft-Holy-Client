using QuickProxyNet;

namespace McProtoNet.MultiVersion
{
	public struct ClientConfig
	{
		public MinecraftVersion Version { get; set; }
		public string Username { get; set; }
		public string Host { get; set; }
		public ushort Port { get; set; }

		//TODO AuthInfo

		public IProxyClient? Proxy { get; set; }
	}
}