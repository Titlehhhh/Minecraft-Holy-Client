using Fody;
using HolyClient.Common;
using QuickProxyNet;
using System.Net.Sockets;
using System.Threading.Channels;

namespace HolyClient.StressTest
{
	[ConfigureAwait(false)]
	public sealed class ProxyChecker : IDisposable
	{
		private readonly ChannelWriter<IProxyClient> _writer;
		private readonly IEnumerable<ProxyInfo> _proxies;
		private readonly int _parallelCount;
		private readonly int _connectTimeout;
		private readonly int _sendTimeout;
		private readonly int _readTimeout;
		private readonly string _targetHost;
		private readonly ushort _targetPort;

		private readonly CancellationTokenSource _cts = new();


		public ProxyChecker(
			ChannelWriter<IProxyClient> writer,
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
				var clients = _proxies.Select(proxy => proxyClientFactory.Create(proxy.Type, proxy.Host, proxy.Port));
				var tasks = new List<Task>(_parallelCount);

				while (!_cts.IsCancellationRequested)
				{

					foreach (var chunk in clients.Chunk(_parallelCount))
					{
						tasks.Clear();

						_cts.Token.ThrowIfCancellationRequested();


						using var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);



						foreach (var client in chunk)
						{
							_cts.Token.ThrowIfCancellationRequested();
							tasks.Add(CheckProxy(client, linked.Token));
						}
						linked.CancelAfter(_connectTimeout);
						_cts.Token.ThrowIfCancellationRequested();
						await Task.WhenAll(tasks);



						_cts.Token.ThrowIfCancellationRequested();


						for (int i = 0; i < _parallelCount; i++)
						{
							_cts.Token.ThrowIfCancellationRequested();
							var task = tasks[i];


							if (task.IsCompletedSuccessfully)
							{
								var client = chunk[i];
								await _writer.WriteAsync(client, _cts.Token);
							}
						}




					}
					await Task.Delay(1000, _cts.Token);
				}


			}
			catch
			{

			}
			finally
			{

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

		private async Task CheckProxy(IProxyClient client, CancellationToken cancellationToken)
		{

			using TcpClient tcpClient = new();



			tcpClient.SendTimeout = _sendTimeout;
			tcpClient.ReceiveTimeout = _readTimeout;

			await tcpClient.ConnectAsync(client.ProxyHost, client.ProxyPort, cancellationToken);

			using var stream = await client.ConnectAsync(tcpClient.GetStream(), _targetHost, _targetPort, cancellationToken);



		}
	}


}
