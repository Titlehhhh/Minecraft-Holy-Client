using Avalonia.Controls.Notifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stateless.Graph;
using System;
using System.Reactive;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public sealed class FileImportProxyDialogViewModel : ImportProxyViewModel
{
	[Reactive]
	public string FilePath
	{
		get;
		set;
	}

	

	public FileImportProxyDialogViewModel()
	{

	}
}
