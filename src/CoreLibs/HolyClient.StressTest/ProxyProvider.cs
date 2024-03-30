using Fody;
using HolyClient.Common;
using QuickProxyNet;
using System.Collections.Concurrent;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace HolyClient.StressTest
{


	[ConfigureAwait(false)]
	class ProxyProvider : IProxyProvider
	{


		private readonly ChannelReader<IProxyClient> reader;


		private CancellationTokenSource cts = new();


		public ProxyProvider(ChannelReader<IProxyClient> reader)
		{
			this.reader = reader;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public async ValueTask<IProxyClient> GetNextProxy()
		{
			return await reader.ReadAsync(cts.Token);
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
