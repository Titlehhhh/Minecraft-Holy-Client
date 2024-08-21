using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace HolyClient.CustomControls;

[TemplatePart("PART_StackPanel", typeof(StackPanel))]
public class FillStackPanel : TemplatedControl
{
    public static StyledProperty<IDataTemplate> ItemTemplateProperty =
        AvaloniaProperty.Register<FillStackPanel, IDataTemplate>(nameof(ItemTemplate));

    private StackPanel _panel;
    private readonly double ItemHeight = 48 + 10;

    public IDataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _panel = e.NameScope.Find<StackPanel>("PART_StackPanel");
        base.OnApplyTemplate(e);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        var count = (int)(e.NewSize.Height / ItemHeight) + 1;

        if (_panel.Children.Count > count)
        {
        }
        else if (_panel.Children.Count < count)
        {
            var delta = count - _panel.Children.Count;
            for (var i = 0; i < delta; i++)
            {
            }
        }

        base.OnSizeChanged(e);
    }
}