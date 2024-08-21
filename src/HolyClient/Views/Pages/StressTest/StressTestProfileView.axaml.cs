using System;
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using HolyClient.Localization;
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
            this.WhenAnyValue(x => x.ViewModel)
                .Subscribe(vm =>
                {
                    if (vm is null)
                        return;
                    vm.SelectProxyImportSourceDialog.RegisterHandler(async x =>
                    {
                        var dialog = new ContentDialog
                        {
                            Title = Loc.Tr("StressTest.Configuration.Proxy.Dialog.SelectSource"),
                            PrimaryButtonText = Loc.Tr("Next"),
                            IsSecondaryButtonEnabled = false,
                            CloseButtonText = Loc.Tr("Cancel"),
                            Content = new SelectImportSourceProxyDialogContent
                            {
                                DataContext = x.Input
                            }
                        };
                        var result = await dialog.ShowAsync();


                        x.SetOutput(result == ContentDialogResult.Primary);
                    }).DisposeWith(d);
                    vm.ImportProxyDialog.RegisterHandler(async x =>
                    {
                        var dialog = new ContentDialog
                        {
                            Title = Loc.Tr($"StressTest.Configuration.Proxy.Dialog.SelectSource.{x.Input.Title}"),
                            PrimaryButtonText = Loc.Tr("Add"),
                            IsSecondaryButtonEnabled = false,
                            CloseButtonText = Loc.Tr("Cancel"),
                            Content = x.Input
                        };
                        var result = await dialog.ShowAsync();


                        x.SetOutput(result == ContentDialogResult.Primary);
                    }).DisposeWith(d);


                    vm.ConfirmDeleteProxyDialog.RegisterHandler(async x =>
                    {
                        var dialog = new ContentDialog
                        {
                            Title = Loc.Tr("StressTest.Configuration.Proxy.Dialog.ConfirmDeleteQuestion"),
                            PrimaryButtonText = Loc.Tr("Yes"),
                            IsSecondaryButtonEnabled = false,
                            CloseButtonText = Loc.Tr("No")
                        };

                        var result = await dialog.ShowAsync();

                        x.SetOutput(result == ContentDialogResult.Primary);
                    });
                })
                .DisposeWith(d);
        });
    }
}