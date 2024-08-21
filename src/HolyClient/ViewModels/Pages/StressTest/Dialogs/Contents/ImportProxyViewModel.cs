using System;
using QuickProxyNet;

namespace HolyClient.ViewModels;

public abstract class ImportProxyViewModel
{
    protected ImportProxyViewModel(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
    public ProxyType Type { get; set; }

    public ProxyType[] AvailableTypes { get; } = Enum.GetValues<ProxyType>();

    public abstract bool IsValid();
}