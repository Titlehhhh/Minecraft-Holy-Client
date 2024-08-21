using ReactiveUI;

namespace HolyClient.ViewModels;

public interface IHomeViewModel
{
    string Description { get; }
    IScreen HostScreen { get; }
    string ReleaseNotes { get; }
    string? UrlPathSegment { get; }
    string Version { get; }
}