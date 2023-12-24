using Avalonia.Controls.Notifications;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Stateless.Graph;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Windows.Input;

namespace HolyClient.ViewModels;

public sealed class FileImportProxyDialogViewModel : ImportProxyViewModel
{
	
	public string FilePath
	{
		get;
		set;
	}

	

	public FileImportProxyDialogViewModel()
	{

	}

	public override bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(FilePath);
	}
}
