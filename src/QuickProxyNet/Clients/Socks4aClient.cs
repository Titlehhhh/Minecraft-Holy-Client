

using System.Net;

namespace QuickProxyNet
{
	public class Socks4aClient : Socks4Client
	{
		public new ProxyType Type => ProxyType.SOCKS4;
		public Socks4aClient(string host, int port) : base(host, port)
		{
			IsSocks4a = true;
		}

		public Socks4aClient(string host, int port, NetworkCredential credentials) : base(host, port, credentials)
		{
			IsSocks4a = true;
		}
	}
}
