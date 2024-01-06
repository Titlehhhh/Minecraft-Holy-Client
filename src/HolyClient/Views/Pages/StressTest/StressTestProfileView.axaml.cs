using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class StressTestProfileView : ReactiveUserControl<StressTestProfileViewModel>
{
	public StressTestProfileView()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{

			var NotificationManager = new Avalonia.Controls.Notifications.WindowNotificationManager(TopLevel.GetTopLevel(this));

			NotificationManager.Position = Avalonia.Controls.Notifications.NotificationPosition.BottomRight;

			//this.ViewModel.SelectProxyImportSourceDialog.RegisterHandler(async x =>
			//{
			//	ContentDialog dialog = new ContentDialog()
			//	{
			//		Title = Loc.Tr("StressTest.Configuration.Proxy.Dialog.SelectSource"),
			//		PrimaryButtonText = Loc.Tr("Next"),
			//		IsSecondaryButtonEnabled = false,
			//		CloseButtonText = Loc.Tr("Cancel"),
			//		Content = new SelectImportSourceProxyDialogContent()
			//		{
			//			DataContext = x.Input
			//		}
			//	};
			//	var result = await dialog.ShowAsync();


			//	x.SetOutput(result == ContentDialogResult.Primary);
			//}).DisposeWith(d);
			//this.ViewModel.ImportProxyDialog.RegisterHandler(async x =>
			//{

			//	ContentDialog dialog = new ContentDialog()
			//	{
			//		Title = Loc.Tr($"StressTest.Configuration.Proxy.Dialog.SelectSource.{x.Input.Title}"),
			//		PrimaryButtonText = Loc.Tr("Add"),
			//		IsSecondaryButtonEnabled = false,
			//		CloseButtonText = Loc.Tr("Cancel"),
			//		Content = x.Input

			//	};
			//	var result = await dialog.ShowAsync();


			//	x.SetOutput(result == ContentDialogResult.Primary);
			//}).DisposeWith(d);




			//this.ViewModel.ConfirmDeleteProxyDialog.RegisterHandler(async x =>
			//{
			//	ContentDialog dialog = new ContentDialog()
			//	{
			//		Title = Loc.Tr("StressTest.Configuration.Proxy.Dialog.ConfirmDeleteQuestion"),
			//		PrimaryButtonText = Loc.Tr("Yes"),
			//		IsSecondaryButtonEnabled = false,
			//		CloseButtonText = Loc.Tr("No")

			//	};



			//	var result = await dialog.ShowAsync();

			//	x.SetOutput(result == ContentDialogResult.Primary);
			//});



		});
	}
}