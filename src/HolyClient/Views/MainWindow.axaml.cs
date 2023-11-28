using Avalonia;
using Avalonia.Diagnostics;
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
