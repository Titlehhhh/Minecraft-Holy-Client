using Android.App;
using Android.Content.PM;
using Android.OS;
using Avalonia.Android;
using Avalonia.Markup.Xaml.MarkupExtensions;
using ReactiveUI;

namespace HolyClient.Android
{
	[Activity(Label = "HolyClient.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public class MainActivity : AvaloniaMainActivity
	{
		
		protected override void OnCreate(Bundle savedInstanceState)
		{	
			
			base.OnCreate(savedInstanceState);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Internal.SaveSettings();
		}
		protected override void OnStop()
		{
			base.OnStop();
			Internal.SaveSettings();
		}
		protected override void OnPause()
		{
			base.OnPause();
			Internal.SaveSettings();
		}
	}
}