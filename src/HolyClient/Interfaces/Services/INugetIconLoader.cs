using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace HolyClient.Contracts.Services;

public interface INugetIconLoader
{
    void AddLoadableIcon();
    void RemoveLoadableIcon();

    ValueTask<Bitmap?> LoadAsync(string uri, CancellationToken cancellationToken = default);
}

public interface INugetIconCache
{
    void TryGet(string url, out Bitmap bitmap);
}