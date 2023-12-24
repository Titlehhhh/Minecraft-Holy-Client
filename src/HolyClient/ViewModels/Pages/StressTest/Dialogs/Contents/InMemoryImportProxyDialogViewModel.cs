using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Collections;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.States;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;

namespace HolyClient.ViewModels;

public sealed class InMemoryImportProxyDialogViewModel : ImportProxyViewModel
{

	[Reactive]
	public string Lines { get; set; } = "";



	public InMemoryImportProxyDialogViewModel()
	{
		

		Init();
	}
	private void Init()
	{
		//TODO extract from clipboard
		//TopLevel.GetTopLevel().Clipboard.GetTextAsync();
	}

	public override bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(Lines);
	}
}
