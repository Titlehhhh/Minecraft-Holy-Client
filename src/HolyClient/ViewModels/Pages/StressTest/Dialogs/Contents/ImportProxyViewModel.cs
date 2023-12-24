using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;
using System;

namespace HolyClient.ViewModels;

public abstract class ImportProxyViewModel 
{
	
	public ProxyType Type { get; set; }
	
	public ProxyType[] AvailableTypes { get; } = Enum.GetValues<ProxyType>();

	public abstract bool IsValid();
}
