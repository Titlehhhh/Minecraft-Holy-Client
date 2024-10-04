using System;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Controls.Primitives;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using HolyClient.Localization;
using HolyClient.Models;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        
        this.WhenActivated(d =>
        {

            ViewModel.WhenAnyValue(x => x.SelectedPage)
                .Subscribe(page =>
                {
                    var list = Nav.MenuItems.Cast<NavigationViewItem>()
                        .Concat(Nav.FooterMenuItems.Cast<NavigationViewItem>());

                    NavigationViewItem item;

                    if ((item = list.FirstOrDefault(x => (Page)x.Tag == page)) is not null) Nav.SelectedItem = item;
                }).DisposeWith(d);
        });

        CultureInfo.CurrentCulture = new CultureInfo(ConvertCode(Loc.Instance.CurrentLanguage));
        Loc.Instance.CurrentLanguageChanged += (__, e) =>
        {
            CultureInfo.CurrentCulture = new CultureInfo(ConvertCode(e.NewLanguageId));
            //Nav.SwitchLang();
        };
    }


    private static string ConvertCode(string v)
    {
        return v switch
        {
            "ru" => "ru-RU",
            "en" => "en-US"
        };
    }

    private void NavigationView_ItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
    {
        if (e.InvokedItemContainer is NavigationViewItem nvi && nvi.Tag is Page typ) 
            ViewModel.SelectedPage = typ;
    }
}