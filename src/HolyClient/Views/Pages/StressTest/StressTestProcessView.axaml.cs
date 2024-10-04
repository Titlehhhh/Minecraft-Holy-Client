using System;
using System.Collections.Specialized;
using System.Reactive.Disposables;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views;

public partial class StressTestProcessView : ReactiveUserControl<StressTestProcessViewModel>
{
    public StressTestProcessView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        ViewModel.Logs.CollectionChanged += Logs_CollectionChanged;
        base.OnLoaded(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        ViewModel.Logs.CollectionChanged -= Logs_CollectionChanged;
        base.OnUnloaded(e);
    }

    private void Logs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        LogsScroll.ScrollToEnd();
    }
}