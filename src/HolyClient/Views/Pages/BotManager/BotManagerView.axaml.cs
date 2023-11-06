using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using HolyClient.Localization;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class BotManagerView : ReactiveUserControl<IBotManagerViewModel>
{
	public BotManagerView()
	{
		InitializeComponent();
		this.WhenActivated(x =>
		{
			this.ViewModel.Dialog.RegisterHandler(async x =>
			{
				ContentDialog contentDialog = new ContentDialog()
				{
					Title = Loc.Tr("BotManager.DialogRemoveConfirmRequest"),
					
					IsSecondaryButtonEnabled = true,
					IsPrimaryButtonEnabled = true,
					PrimaryButtonText = Loc.Tr("Yes"),
					SecondaryButtonText = Loc.Tr("No"),					
				};

				var result = await contentDialog.ShowAsync();

				if (result == ContentDialogResult.Primary)
				{
					x.SetOutput(true);
				}
				else
				{
					x.SetOutput(false);
					//	throw new System.Exception("Unkown type dialog");
				}
				//this.InvalidateVisual();
			});
		});
	}

}