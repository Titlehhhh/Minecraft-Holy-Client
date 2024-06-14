using System.ComponentModel;
using System.Threading.Tasks;
using DynamicData;
using NuGet.Protocol.Core.Types;

namespace HolyClient.Contracts.Models;

public interface INugetClient : INotifyPropertyChanged
{
    ISourceCache<IPackageSearchMetadata, string> Packages { get; }
    string Filter { get; set; }
    bool EnablePreviewVersions { get; set; }

    bool IsLoading { get; }
    bool CanLoadNew { get; }

    Task LoadNewItems(int count = 20);
    Task Refresh();
}