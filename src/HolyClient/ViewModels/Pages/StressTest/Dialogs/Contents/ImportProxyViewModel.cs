using QuickProxyNet;
using System;

namespace HolyClient.ViewModels;

public abstract class ImportProxyViewModel
{
	public string Title { get; set; }
	public ProxyType Type { get; set; }

	public ProxyType[] AvailableTypes { get; } = Enum.GetValues<ProxyType>();

	public abstract bool IsValid();

	protected ImportProxyViewModel(string title)
	{
		Title = title;
	}
}
