using HolyClient.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HolyClient.DesignTime
{
	public class DesignStressTestProcessViewModel : ReactiveObject, IStressTestProcessViewModel
	{
		public IEnumerable<ISeries> BotsOnline_Series { get; set; }

		public ICommand CancelCommand { get; set; }

		public IEnumerable<ISeries> CPS_Series { get; set; }

		public Axis[] XAxes { get; set; }

		public string? UrlPathSegment => throw new NotImplementedException();

		public IScreen HostScreen { get; set; }

		public ViewModelActivator Activator { get; set; }

		public IEnumerable<ISeries> Proxy_Series => Enumerable.Empty<ISeries>();

		public string Host => "DDOS: example.org";

		public string Version => "Version: 1.16.5";

		public string ParallelCount => "1000";

		public int BotsOnline => 70;

		public int CPS => 5;

		public int PeakCPS => 0;

		public string ProxyQuality => "100%";

		public ObservableCollection<LogEventViewModel> Logs => throw new NotImplementedException();

		public DesignStressTestProcessViewModel()
		{

		}

	}
}
