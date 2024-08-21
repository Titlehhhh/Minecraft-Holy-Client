using ReactiveUI;

namespace HolyClient.ViewModels;

public class LoadingViewModel : ReactiveObject, IRoutableViewModel
{
    public LoadingViewModel(IScreen screen, string text)
    {
        HostScreen = screen;
        Text = text;
    }

    public string Text { get; }
    public string? UrlPathSegment => "/loading";

    public IScreen HostScreen { get; }
}