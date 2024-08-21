using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using HolyClient.Contracts.Services;

namespace HolyClient.Services;

public class NugetIconLoader : INugetIconLoader
{
    private readonly ConcurrentDictionary<string, byte[]> _cache = new();
    private volatile int _count;


    private readonly HttpClient httpClient = new();

    public async ValueTask<Bitmap?> LoadAsync(string url, CancellationToken cancellation = default)
    {
        if (_cache.TryGetValue(url, out var value))
        {
            await Task.Yield(); // <===========================
            using var stream = new MemoryStream(value);
            return new Bitmap(stream);
        }

        try
        {
            var externalBytes = await LoadDataFromExternalAsync(url, cancellation).ConfigureAwait(false);
            if (externalBytes == null) return null;
            if (externalBytes.Length == 0)
                return null;
            using var memoryStream = new MemoryStream(externalBytes);
            var bitmap = new Bitmap(memoryStream);
            _cache.TryAdd(url, externalBytes);
            return bitmap;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public void AddLoadableIcon()
    {
        Interlocked.Increment(ref _count);
    }

    public void RemoveLoadableIcon()
    {
        if (Interlocked.Decrement(ref _count) == 1) _cache.Clear();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    private async Task<byte[]?> LoadDataFromExternalAsync(string url, CancellationToken cancellation)
    {
        try
        {
            return await httpClient.GetByteArrayAsync(url, cancellation).ConfigureAwait(false);
        }
        catch (Exception)
        {
            return null;
        }
    }


    ~NugetIconLoader()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) httpClient.Dispose();
    }
}