using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace HolyClient.ViewModels;

public abstract class ImportProxyViewModel : ReactiveObject
{
	

	[Reactive]
	public ProxyType Type { get; set; }
	
	public ProxyType[] AvailableTypes { get; } = Enum.GetValues<ProxyType>();
}
