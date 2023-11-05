using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HolyClient.CustomControls
{
	public class HyperlinkButton : Button
	{
		public static readonly DirectProperty<HyperlinkButton, Uri> NavigateUriProperty =
			AvaloniaProperty.RegisterDirect<HyperlinkButton, Uri>(nameof(NavigateUri), x => x.NavigateUri, (x, v) => x.NavigateUri = v);

		public Uri NavigateUri
		{
			get => _navigateUri;
			set => SetAndRaise(NavigateUriProperty, ref _navigateUri, value);
		}

		protected override Type StyleKeyOverride => typeof(HyperlinkButton);

		protected override void OnClick()
		{
			base.OnClick();

			if (NavigateUri != null)
			{
				var url = NavigateUri.ToString();
				try
				{
					if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					{
						//https://stackoverflow.com/a/2796367/241446
						using var proc = new Process { StartInfo = { UseShellExecute = true, FileName = url } };
						proc.Start();

						return;
					}

					if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					{
						Process.Start("x-www-browser", url);
						return;
					}

					if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) throw new ArgumentException("invalid url: " + url);
					Process.Start("open", url);
				}
				catch
				{
					
				}
			}
		}

		protected override bool RegisterContentPresenter(ContentPresenter presenter)
		{
			if (presenter.Name == "ContentPresenter")
			{
				return true;
			}

			return false;
		}

		private Uri _navigateUri;
	}

}
