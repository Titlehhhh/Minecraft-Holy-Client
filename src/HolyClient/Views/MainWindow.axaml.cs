using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using FluentAvalonia.UI.Windowing;

namespace HolyClient.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

#if DEBUG

        this.AttachDevTools(new DevToolsOptions());
#endif

        
        SplashScreen = new ApplicationSplashScreen(this);
    }
}