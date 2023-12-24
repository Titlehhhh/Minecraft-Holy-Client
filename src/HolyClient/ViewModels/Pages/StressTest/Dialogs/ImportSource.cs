using Avalonia.Controls.Notifications;
using HolyClient.Services;
using HolyClient.StressTest;
using QuickProxyNet;
using Splat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Windows.Input;

namespace HolyClient.ViewModels.Pages.StressTest.Dialogs
{

	public enum ImportSource
	{
		InMemory,
		File,
		Url
	}
}
