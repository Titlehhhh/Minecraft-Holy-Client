using QuickProxyNet;

namespace HolyClient.StressTest
{
	public interface IProxyProvider
	{
		public ValueTask<IProxyClient> GetNextProxy();
	}
}
