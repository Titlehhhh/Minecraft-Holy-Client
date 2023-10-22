using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;

namespace HolyClient.Browser;

internal static partial class Interop
{
	internal static Subject<IDisposable>? OnStop { get; set; }

	[JSExport]
	internal static void SaveSettings()
	{
		if(OnStop is { })
		{
			using(var manual = new ManualResetEvent(false))
			{
				OnStop.OnNext(Disposable.Create(()=>manual.Set()));
				manual.WaitOne();
			}
		}
	}
}

