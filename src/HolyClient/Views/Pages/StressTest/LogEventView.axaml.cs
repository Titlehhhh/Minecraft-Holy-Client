using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using HolyClient.ViewModels;

namespace HolyClient.Views;

public partial class LogEventView : UserControl
{
    public LogEventView()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is LogEventViewModel vm)
        {
            InlineCollection inlines = new InlineCollection();

            Run TimestampTextblock = new Run();

            TimestampTextblock.Foreground = Brushes.LightGray;

            Run MessageText = new Run();
            MessageText.Foreground = Brushes.LightGray;


            TimestampTextblock.Text = vm.Timestamp;
            EventIcon.Data = (Geometry)App.Current.Resources[$"{vm.Level}IconPath"];
            EventIcon.Foreground = (IBrush)App.Current.Resources[$"{vm.Level}Color"];
            
            ToolTip.SetTip(IconBorder, vm.Level);
            ToolTip.SetBetweenShowDelay(IconBorder, 2500);


            MessageText.Text = vm.Message;


            inlines.Add(TimestampTextblock);
            inlines.Add(new Run(" "));
            inlines.Add(MessageText);
            inlines.Add(" ");
            if (vm.Exception is not null)
            {
                string errorText = $"{vm.Exception.Message}\n{vm.Exception.StackTrace}";
                ExceptionTextBlock.Text = errorText + " ";
                ExceptionTextBlock.IsVisible = true;
            }
            else
            {
                ExceptionTextBlock.IsVisible = false;
            }

            MainTextBlock.Inlines = inlines;
        }

        base.OnDataContextChanged(e);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
    }
}