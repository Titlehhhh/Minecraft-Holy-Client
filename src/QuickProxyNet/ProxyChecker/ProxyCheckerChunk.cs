using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Channels;
using DotNext.Collections.Generic;
using Fody;

namespace QuickProxyNet.ProxyChecker;

internal class ProxyCheckerChunk : IProxyChecker
{

    private readonly ProxyCheckerChunkedOptions _options;
    private Channel<IProxyClient> _channel;
    private IEnumerable<ProxyRecord> _proxies;
    private ChannelWriter<IProxyClient> _writer;
    private ChannelReader<IProxyClient> _reader;
    private CancellationTokenSource _cts;

    private bool _disposed;

    public ProxyCheckerChunk(IEnumerable<ProxyRecord> proxies, ProxyCheckerChunkedOptions options)
    {
        _proxies = proxies;
        _options = options;
        _cts = new CancellationTokenSource();
        _channel = Channel.CreateBounded<IProxyClient>(new BoundedChannelOptions(options.QueueSize)
        {
            SingleWriter = true
        });

        _writer = _channel.Writer;
        _reader = _channel.Reader;
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ProxyCheckerChunk));
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        _writer.TryComplete();
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _proxies = null;
        _channel = null;
    }

    public Task Start()
    {
        CheckDisposed();
        return RunCore();
    }

    public ValueTask<IProxyClient> GetNextProxy(CancellationToken cancellationToken)
    {
        CheckDisposed();
        return _reader.ReadAsync(cancellationToken);
    }

    private static IEnumerable<IProxyClient> Convert(IEnumerable<ProxyRecord> records,
        ProxyClientFactory proxyClientFactory)
    {
        foreach (var proxy in records)
        {
            bool success = false;
            IProxyClient? client = null;

            try
            {
                client = proxyClientFactory.Create(proxy.Type, proxy.Host, proxy.Port, proxy.Credentials);
                success = true;
            }
            catch
            {
                // ignored
            }

            if (success)
                yield return client!;
        }
    }

    private Task RunCore()
    {
        return Task.Run(async () =>
        {
            try
            {
                ProxyClientFactory proxyClientFactory = new();
                var clients = Convert(_proxies, proxyClientFactory);

                ArgumentOutOfRangeException.ThrowIfNegative(_options.ChunkSize, nameof(_options.ChunkSize));

                List<Task<CheckResult>> tasks = new();

                while (!_cts.IsCancellationRequested)
                {
                    foreach (var chunk in clients.Chunk(_options.ChunkSize))
                    {
                        try
                        {
                            _cts.Token.ThrowIfCancellationRequested();

                            foreach (var client in chunk)
                            {
                                Task<CheckResult> task = CheckProxy(client, _options.ConnectTimeout, _cts.Token);
                                tasks.Add(task);
                            }

                            await Task.WhenAll(tasks);


                            foreach (Task<CheckResult> task in tasks)
                            {
                                if (task.Result.Connected)
                                {
                                    await task.Result.Stream!.DisposeAsync();
                                    await _writer.WriteAsync(task.Result.Client!, _cts.Token);
                                }
                            }
                        }
                        finally
                        {
                            tasks.Clear();
                        }
                    }

                    if (!_options.InfinityLoop)
                        return;
                }
            }
            catch (Exception ex)
            {
                _writer.TryComplete(ex);
                Dispose();
                throw;
            }
            finally
            {
                _writer.TryComplete();
            }
        });
    }

    private async Task<CheckResult> CheckProxy(IProxyClient client, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        try
        {
            linked.CancelAfter(timeout);
            Stream stream = await client.ConnectAsync(_options.TargetHost, _options.TargetPort, linked.Token)
                .ConfigureAwait(false);
            return new CheckResult(true, null, stream, client);
        }
        catch (Exception e)
        {
            return new CheckResult(false, e, null, client);
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