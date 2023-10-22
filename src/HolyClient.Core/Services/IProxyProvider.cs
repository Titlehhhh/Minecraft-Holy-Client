using QuickProxyNet;

namespace HolyClient.Core.Services
{
	public interface IProxyProvider
	{
		public ValueTask<IProxyClient> GetNextProxy();
	}
}
