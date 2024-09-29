using System.Threading.Channels;
using Fody;

namespace QuickProxyNet.ProxyChecker;

internal class ProxyCheckerChunk : IProxyChecker
{
    private event Action Disposed;

    private readonly string _targetHost;
    private readonly int _targetPort;
    private readonly int chunkSize;
    private readonly int conntectTimeout;
    private Channel<IProxyClient> _channel;
    private IEnumerable<ProxyRecord> _proxies;
    private ChannelWriter<IProxyClient> _writer;
    private ChannelReader<IProxyClient> _reader;
    private CancellationTokenSource _cts;
    private bool sendAlive;

    private bool disposed;

    public ProxyCheckerChunk(IEnumerable<ProxyRecord> proxies, int chunkSize, int connectTimeout, string targetHost,
        int targetPort, bool sendAlive)
    {
        this.chunkSize = chunkSize;
        this.conntectTimeout = connectTimeout;
        _targetHost = targetHost;
        _targetPort = targetPort;
        this.sendAlive = sendAlive;
        _proxies = proxies;
        _cts = new CancellationTokenSource();
        _channel = Channel.CreateBounded<IProxyClient>(new BoundedChannelOptions(this.chunkSize)
        {
            SingleWriter = true
        });

        _writer = _channel.Writer;
        _reader = _channel.Reader;
    }

    private void CheckDisposed()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(ProxyCheckerChunk));
    }

    public void Dispose()
    {
        if (disposed)
            return;
        disposed = true;
        _writer.TryComplete();
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _proxies= null;
        _channel = null;
        
        
        Disposed?.Invoke();
    }

    public void Start()
    {
        CheckDisposed();
        RunCore();
    }

    public ValueTask<IProxyClient> GetNextProxy(CancellationToken cancellationToken)
    {
        CheckDisposed();
        return _reader.ReadAsync(cancellationToken);
    }

    private Task RunCore()
    {
        return Task.Run(async () =>
        {
            try
            {
                ProxyClientFactory proxyClientFactory = new();
                var clients = _proxies.Select(proxy => proxyClientFactory.Create(proxy.Type, proxy.Host, proxy.Port,proxy.Credentials));

                var tasks = new List<Task<CheckResult>>(chunkSize);
                while (!_cts.IsCancellationRequested)
                {
                    foreach (var chunk in clients.Chunk(chunkSize))
                    {
                        try
                        {
                           


                            using (var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token))
                            {
                                foreach (var client in chunk)
                                {
                                    _cts.Token.ThrowIfCancellationRequested();
                                    tasks.Add(CheckProxy(client, linked.Token));
                                }

                                linked.CancelAfter(conntectTimeout);
                                _cts.Token.ThrowIfCancellationRequested();


                                await Task.WhenAll(tasks).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
                            }


                          

                            var count = 0;
                            for (var i = 0; i < Math.Min(chunkSize, tasks.Count); i++)
                            {
                                var task = tasks[i];
                                if (task.Result.Connected)
                                {
                                    count++;
                                    var client = chunk[i];

                                    IProxyClient sendClient = sendAlive
                                        ? new ProxyClientCache(client, task.Result.Stream!, this.Disposed)
                                        : client;
                                    
                                    await _writer.WriteAsync(sendClient, _cts.Token);
                                }
                            }
                        }
                        finally
                        {
                            tasks.Clear();
                        }
                    }


                    await Task.Delay(1000, _cts.Token);
                }
            }
            catch (Exception ex)
            {
                _writer.TryComplete(ex);
            }
            finally
            {
                _writer.TryComplete();
            }
        });
    }

    [ConfigureAwait(false)]
    private async Task<CheckResult> CheckProxy(IProxyClient client, CancellationToken cancellationToken)
    {
        try
        {
            var stream = await client.ConnectAsync(_targetHost, _targetPort, cancellationToken);
            return new CheckResult(true, null, stream, client);
        }
        catch (Exception exception)
        {
            return new CheckResult(false, exception, null, null);
        }
    }

    private struct CheckResult
    {
        public CheckResult(bool connected, Exception? exception, Stream? stream, IProxyClient? client)
        {
            Connected = connected;
            Error = exception;
            Stream = stream;
            Client = client;
        }

        public bool Connected { get; }
        public Exception? Error { get; }
        public Stream? Stream { get; }
        public IProxyClient? Client { get; }
    }
}