using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using HolyClient.Localization;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class StressTestView : ReactiveUserControl<StressTestViewModel>
{
    public StressTestView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel.ConfirmRemoveDialog.RegisterHandler(async x =>
            {
                var dialog = new ContentDialog
                {
                    Title = "Вы точно хотите удалить?",
                    PrimaryButtonText = Loc.Tr("Yes"),
                    IsSecondaryButtonEnabled = false,
                    CloseButtonText = Loc.Tr("No")
                };

                var result = await dialog.ShowAsync();

                x.SetOutput(result == ContentDialogResult.Primary);
            });
        });
    }
}