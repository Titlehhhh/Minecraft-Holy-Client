using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using DynamicData;
using HolyClient.Common;

namespace HolyClient.Behaviors;

public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
{
    public static readonly DirectProperty<DataGridSelectedItemsBehavior, ISourceList<ProxyInfo>> SelectedItemsProperty =
        AvaloniaProperty.RegisterDirect<DataGridSelectedItemsBehavior, ISourceList<ProxyInfo>>(
            nameof(SelectedItems),
            o => o.SelectedItems,
            (o, v) => o.SelectedItems = v);

    private ISourceList<ProxyInfo> _items;

    public ISourceList<ProxyInfo> SelectedItems
    {
        get => _items;
        set => SetAndRaise(SelectedItemsProperty, ref _items, value);
    }

    protected override void OnAttached()
    {
        AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        base.OnAttached();
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;

        base.OnDetaching();
    }

    private void AssociatedObject_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedItems is not null)
            SelectedItems.Edit(list =>
            {
                list.RemoveMany(e.RemovedItems.Cast<ProxyInfo>());
                list.AddRange(e.AddedItems.Cast<ProxyInfo>());
            });
    }
}