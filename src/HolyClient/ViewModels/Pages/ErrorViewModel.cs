using System;
using System.ComponentModel;
using ReactiveUI;

namespace HolyClient.ViewModels;

public class ErrorViewModel : IRoutableViewModel
{
    public ErrorViewModel(IScreen hostScreen, string description, string errorText)
    {
        HostScreen = hostScreen;
        ErrorText = errorText;
        Description = description;
    }

    public string ErrorText { get; }
    public string Description { get; }
    public string? UrlPathSegment => "/error";
    public IScreen HostScreen { get; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    public void RaisePropertyChanged(PropertyChangedEventArgs args)
    {
        throw new NotImplementedException();
    }

    public void RaisePropertyChanging(PropertyChangingEventArgs args)
    {
        throw new NotImplementedException();
    }
}