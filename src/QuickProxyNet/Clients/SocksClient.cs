using System.Net;

namespace QuickProxyNet
{
	public abstract class SocksClient : ProxyClient
	{

		protected SocksClient(int version, string host, int port) : base(host, port)
		{
			SocksVersion = version;
		}

		protected SocksClient(int version, string host, int port, NetworkCredential credentials) : base(host, port, credentials)
		{
			SocksVersion = version;
		}

		public int SocksVersion
		{
			get; private set;
		}
	}
}
