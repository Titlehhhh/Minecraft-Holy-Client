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

		private readonly CancellationTokenSource _cts = new();


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
			_targetHost = options.TargetHost;
			_targetPort = options.TargetPort;
		}


		public async Task Run(Serilog.ILogger logger)
		{
			try
			{
				ProxyClientFactory proxyClientFactory = new();
				foreach (var chunk in _proxies.Chunk(_parallelCount))
				{
					_cts.Token.ThrowIfCancellationRequested();


					var clients = new List<IProxyClient>();
					foreach (var proxy in chunk)
					{
						_cts.Token.ThrowIfCancellationRequested();
						var client = proxyClientFactory.Create(proxy.Type, proxy.Host, proxy.Port);
						clients.Add(client);
					}

					var tasks = new List<Task<ProxyCheckResult>>();

					logger.Information($"[Proxy Checker] Checking...");
					

					using var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);

					foreach (var client in clients)
					{
						_cts.Token.ThrowIfCancellationRequested();
						tasks.Add(CheckProxy(client, linked.Token));
					}
					linked.CancelAfter(_connectTimeout);
					_cts.Token.ThrowIfCancellationRequested();
					var result = await Task.WhenAll(tasks);

					_cts.Token.ThrowIfCancellationRequested();

					int c = 0;
					for (int i = 0; i < result.Length; i++)
					{
						_cts.Token.ThrowIfCancellationRequested();
						ProxyCheckResult checkResult = result[i];
						if (checkResult.Success)
						{
							c++;
						}
					}

					logger.Information($"[Proxy Checker] {c}/{result.Length}");

					for (int i = 0; i < result.Length; i++)
					{
						_cts.Token.ThrowIfCancellationRequested();
						ProxyCheckResult checkResult = result[i];
						if (checkResult.Success)
						{
							await _writer.WriteAsync(checkResult, _cts.Token);
						}
					}


				}
			}
			catch (TaskCanceledException)
			{
				Console.WriteLine("TaskCancel");
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("OperationCancel");
			}
			catch (Exception ex)
			{
				logger.Error("[Proxy Checker] Failed run", ex);
			}
			finally
			{
				Console.WriteLine("Complete");
				_writer.Complete();
			}
		}


		bool disposed = false;
		public void Dispose()
		{
			if (disposed)
				return;
			disposed = true;
			_cts.Dispose();
			GC.SuppressFinalize(this);
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
			catch (Exception ex)
			{
				if (_cts.IsCancellationRequested)
					throw;
				return new ProxyCheckResult(false, null);
			}

		}
	}


}
