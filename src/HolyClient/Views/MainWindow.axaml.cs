using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;
using FluentAvalonia.UI.Windowing;

namespace HolyClient.Views
{
	public partial class MainWindow : AppWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			
#if DEBUG
			
			this.AttachDevTools(new DevToolsOptions()
			{
				
			});
#endif

			this.WindowState = Avalonia.Controls.WindowState.Maximized;
			SplashScreen = new ApplicationSplashScreen(this);
		}


	}
}
