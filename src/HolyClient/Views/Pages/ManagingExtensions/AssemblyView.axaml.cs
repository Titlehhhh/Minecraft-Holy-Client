using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using HolyClient.Localization;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class AssemblyView : ReactiveUserControl<AssemblyViewModel>
{
    public AssemblyView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            ViewModel.ConfirmDeleteDialog.RegisterHandler(async context =>
            {
                var contentDialog = new ContentDialog
                {
                    Title = Loc.Tr("ManagingExtension.Assemblies.DeleteAssemblyDialog.Title"),

                    IsSecondaryButtonEnabled = true,
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = Loc.Tr("Yes"),
                    SecondaryButtonText = Loc.Tr("No")
                };

                var result = await contentDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    context.SetOutput(ConfirmDeleteAssemblyAnswer.ForceDelete);
                    return;
                }

                context.SetOutput(ConfirmDeleteAssemblyAnswer.None);
                //	throw new System.Exception("Unkown type dialog");
            }).DisposeWith(d);
        });
    }
}