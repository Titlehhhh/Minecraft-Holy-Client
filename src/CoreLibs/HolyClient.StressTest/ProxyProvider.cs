using Fody;
using HolyClient.Common;
using QuickProxyNet;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace HolyClient.StressTest
{
	[ConfigureAwait(false)]
	class ProxyProvider : IProxyProvider
	{
		private readonly ChannelReader<ProxyCheckResult> reader;
		private Channel<IProxyClient> channel;
		private ChannelReader<IProxyClient> _reader;
		private ChannelWriter<IProxyClient> _writer;

		private CancellationTokenSource cts = new();


		public ProxyProvider(ChannelReader<ProxyCheckResult> reader, int capacity = 100)
		{
			this.reader = reader;
			this.channel = Channel.CreateBounded<IProxyClient>(new BoundedChannelOptions(capacity)
			{
				SingleWriter = true
			});

			_writer = channel.Writer;
			_reader = channel.Reader;

		}
		private List<IProxyClient> proxies = new();




		public async Task Run()
		{
			await foreach (var item in reader.ReadAllAsync())
			{
				await _writer.WriteAsync(item.ProxyClient);
				proxies.Add(item.ProxyClient);
			}

			await Task.Run(async () =>
			{
				try
				{
					while (!cts.IsCancellationRequested)
					{
						foreach (var item in proxies)
						{
							await _writer.WriteAsync(item, cts.Token);
						}
					}
				}
				catch
				{

				}
				finally
				{
					_writer.TryComplete();
				}
			});
		}



		public ValueTask<IProxyClient> GetNextProxy()
		{
			return _reader.ReadAsync();
		}
		private bool _disposed = false;
		public void Dispose()
		{
			if (_disposed)
				return;
			_disposed = true;
			cts.Dispose();

			GC.SuppressFinalize(this);
		}
	}

}
