using System.Net.Sockets;
using System.Threading.Channels;
using Fody;
using HolyClient.Common;
using QuickProxyNet;
using Serilog;

namespace HolyClient.StressTest;

public sealed class ProxyChecker : IDisposable
{
    private readonly int _connectTimeout;
    private readonly int _parallelCount;
    private readonly int _readTimeout;
    private readonly int _sendTimeout;
    private readonly string _targetHost;
    private readonly ushort _targetPort;

    private CancellationTokenSource _cts = new();
    private IEnumerable<ProxyInfo> _proxies;
    private ChannelWriter<IProxyClient> _writer;

    private bool disposed;

    public ProxyChecker(
        ChannelWriter<IProxyClient> writer,
        IEnumerable<ProxyInfo> proxies,
        ProxyCheckerOptions options, string targetHost, ushort targetPort)
    {
        _writer = writer;
        _proxies = proxies;
        _parallelCount = options.ParallelCount;
        _connectTimeout = options.ConnectTimeout;
        _sendTimeout = options.SendTimeout;
        _readTimeout = options.ReadTimeout;
        _targetHost = targetHost;
        _targetPort = targetPort;
    }

    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _writer.TryComplete();
        _writer = null;
        _proxies = null;
        GC.SuppressFinalize(this);
    }


    public Task Run(ILogger logger)
    {
        return Task.Run(async () =>
        {
            try
            {
                ProxyClientFactory proxyClientFactory = new();
                var clients = _proxies.Select(proxy => proxyClientFactory.Create(proxy.Type, proxy.Host, proxy.Port));


                while (!_cts.IsCancellationRequested)
                {
                    foreach (var chunk in clients.Chunk(_parallelCount))
                    {
                        var tasks = new List<Task>(_parallelCount);


                        _cts.Token.ThrowIfCancellationRequested();


                        using (var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token))
                        {
                            foreach (var client in chunk)
                            {
                                _cts.Token.ThrowIfCancellationRequested();
                                tasks.Add(CheckProxy(client, linked.Token));
                            }

                            linked.CancelAfter(_connectTimeout);
                            _cts.Token.ThrowIfCancellationRequested();


                            await Task.WhenAll(tasks).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
                        }


                        _cts.Token.ThrowIfCancellationRequested();

                        var count = 0;
                        for (var i = 0; i < Math.Min(_parallelCount, tasks.Count); i++)
                        {
                            _cts.Token.ThrowIfCancellationRequested();
                            var task = tasks[i];


                            if (task.IsCompletedSuccessfully)
                            {
                                count++;
                                var client = chunk[i];
                                await _writer.WriteAsync(client, _cts.Token);
                            }
                        }
                    }

                    await Task.Delay(1000, _cts.Token);
                }
            }
            catch (Exception ex)
            {
                _writer.TryComplete(ex);
                logger.Error(ex, "ProxyChecker завершился с ошибкой");
            }
            finally
            {
                _writer.TryComplete();
            }
        });
    }

    [ConfigureAwait(false)]
    private async Task CheckProxy(IProxyClient client, CancellationToken cancellationToken)
    {
        using TcpClient tcpClient = new();

        tcpClient.SendTimeout = _sendTimeout;
        tcpClient.ReceiveTimeout = _readTimeout;

        await using var cancel = cancellationToken.Register(tcp => ((TcpClient)tcp).Dispose(), tcpClient);

        await tcpClient.ConnectAsync(client.ProxyHost, client.ProxyPort, cancellationToken);


        await using var ns = tcpClient.GetStream();

        await using var stream = await client.ConnectAsync(ns, _targetHost, _targetPort, cancellationToken);
    }
}