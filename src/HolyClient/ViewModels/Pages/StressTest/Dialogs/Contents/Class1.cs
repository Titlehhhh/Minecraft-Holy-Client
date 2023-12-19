using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyClient.ViewModels;

public abstract class ImportProxyViewModel : ReactiveObject
{
	//public abstract string Title { get; }

	[Reactive]
	public ProxyType Type { get; set; }

	public ProxyType[] AvailableTypes { get; } = Enum.GetValues<ProxyType>();
}

public sealed class InMemoryImportProxyDialogViewModel : ImportProxyViewModel
{
	[Reactive]
	public string Lines { get; set; }

	
}
