using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using HolyClient.Localization;
using HolyClient.ViewModels;
using HolyClient.Views;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;

namespace HolyClient.Views;

public partial class StressTestConfigurationView : ReactiveUserControl<StressTestConfigurationViewModel>
{

	public StressTestConfigurationView()
	{
		InitializeComponent();



		this.WhenActivated(d =>
		{

			var NotificationManager = new Avalonia.Controls.Notifications.WindowNotificationManager(TopLevel.GetTopLevel(this));

			NotificationManager.Position = Avalonia.Controls.Notifications.NotificationPosition.BottomRight;

			this.ViewModel.SelectProxyImportSourceDialog.RegisterHandler(async x =>
			{
				ContentDialog dialog = new ContentDialog()
				{
					Title = "Импорт прокси",
					PrimaryButtonText = "Далее",
					IsSecondaryButtonEnabled = false,
					CloseButtonText = "Отмена",
					Content = new SelectImportSourceProxyDialogContent()
					{
						DataContext = x.Input
					}
				};
				var result = await dialog.ShowAsync();


				x.SetOutput(result == ContentDialogResult.Primary);
			}).DisposeWith(d);
			this.ViewModel.ImportProxyDialog.RegisterHandler(async x =>
			{
				ContentDialog dialog = new ContentDialog()
				{
					Title = "Импорт прокси",
					PrimaryButtonText = "Импорт",
					IsSecondaryButtonEnabled = false,
					CloseButtonText = "Отмена",
					Content = x.Input
					
				};
				var result = await dialog.ShowAsync();


				x.SetOutput(result == ContentDialogResult.Primary);
			}).DisposeWith(d);




			this.ViewModel.ConfirmDeleteProxyDialog.RegisterHandler(async x =>
			{
				ContentDialog dialog = new ContentDialog()
				{
					Title = Loc.Tr("StressTest.Configuration.Proxy.Dialog.ConfirmDeleteQuestion"),
					PrimaryButtonText = Loc.Tr("Yes"),
					IsSecondaryButtonEnabled = false,
					CloseButtonText = Loc.Tr("No")

				};



				var result = await dialog.ShowAsync();

				x.SetOutput(result == ContentDialogResult.Primary);
			});



		});
	}
	protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
	{
		base.OnAttachedToVisualTree(e);



	}
}