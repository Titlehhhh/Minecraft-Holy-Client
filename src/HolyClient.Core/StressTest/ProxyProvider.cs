using Fody;
using HolyClient.Core.Services;
using QuickProxyNet;

namespace HolyClient.Core.StressTest
{
	[ConfigureAwait(false)]
	class ProxyProvider : IProxyProvider
	{
		private IEnumerable<ProxyInfo> proxies;

		private volatile int _index;

		private IProxyClient[] _clients;
		private static ProxyClientFactory factory = new();

		public ProxyProvider(IEnumerable<ProxyInfo> proxies)
		{
			this.proxies = proxies;

			var clients = proxies.Select(x => factory.Create(x.Type, x.Host, x.Port));

			_clients = clients.ToArray();
		}

		private volatile object _lock = new();

		public IProxyClient GetNextProxy()
		{
			lock (_lock)
			{
				if (_index >= _clients.Length)
					_index = 0;

				return _clients[_index++];
			}
		}
		private SemaphoreSlim _lockAsync = new(1, 1);

		async ValueTask<IProxyClient> IProxyProvider.GetNextProxy()
		{
			try
			{
				await _lockAsync.WaitAsync();

				if (_index >= _clients.Length)
					_index = 0;

				return _clients[_index++];
			}
			finally
			{
				_lockAsync.Release();
			}

		}
	}

}
