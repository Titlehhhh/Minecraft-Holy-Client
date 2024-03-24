using HolyClient.Common;
using System.Net.Sockets;
using System.Threading.Channels;
using QuickProxyNet;

namespace HolyClient.StressTest
{
	public sealed class ProxyChecker : IDisposable
	{
		private readonly ChannelWriter<ProxyCheckResult> _writer;
		private readonly IEnumerable<ProxyInfo> _proxies;
		private readonly int _parallelCount;
		private readonly int _connectTimeout;
		private readonly int _sendTimeout;
		private readonly int _readTimeout;
		private readonly string _targetHost;
		private readonly ushort _targetPort;



		public ProxyChecker(
			ChannelWriter<ProxyCheckResult> writer,
			IEnumerable<ProxyInfo> proxies,
			ProxyCheckerOptions options)
		{
			_writer = writer;
			_proxies = proxies;
			_parallelCount = options.ParallelCount;
			_connectTimeout = options.ConnectTimeout;
			_sendTimeout = options.SendTimeout;
			_readTimeout = options.ReadTimeout;
		}


		public async Task Run(Serilog.ILogger logger)
		{
			try
			{
				ProxyClientFactory proxyClientFactory = new();
				foreach (var chunk in _proxies.Chunk(_parallelCount))
				{

					var clients = new List<IProxyClient>();
					foreach (var proxy in chunk)
					{
						var client = proxyClientFactory.Create(proxy.Type, proxy.Host, proxy.Port);
						clients.Add(client);
					}

					var tasks = new List<Task<ProxyCheckResult>>();
					
					logger.Information($"[Proxy Checker] Checking...");
					using var cts = new CancellationTokenSource(this._connectTimeout);
					foreach (var client in clients)
					{
						
						tasks.Add(CheckProxy(client, cts.Token));
					}

					var result = await Task.WhenAll(tasks);

					int c = 0;
					for (int i = 0; i < result.Length; i++)
					{
						ProxyCheckResult checkResult = result[i];
						if (checkResult.Success)
						{
							c++;
							await _writer.WriteAsync(checkResult);
						}
					}

					logger.Information($"[Proxy Checker] {c}/{result.Length}");


				}
			}
			catch(Exception ex)
			{
				logger.Error("[Proxy Checker] Failed run", ex);
			}
		}



		public void Dispose()
		{
			_writer.TryComplete();
		}

		private async Task<ProxyCheckResult> CheckProxy(IProxyClient client, CancellationToken cancellationToken)
		{
			try
			{
				using TcpClient tcpClient = new();

				tcpClient.SendTimeout = _sendTimeout;
				tcpClient.ReceiveTimeout = _readTimeout;

				await tcpClient.ConnectAsync(client.ProxyHost, client.ProxyPort, cancellationToken);

				using var stream = await client.ConnectAsync(tcpClient.GetStream(), _targetHost, _targetPort, cancellationToken);
				return new ProxyCheckResult(true, client);
			}
			catch
			{
				return new ProxyCheckResult(false, null);
			}

		}
	}


}
