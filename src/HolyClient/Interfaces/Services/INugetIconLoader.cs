using Avalonia.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;

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



