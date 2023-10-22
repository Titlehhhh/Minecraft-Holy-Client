using HolyClient.ViewModels;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using System;
using System.Collections.Generic;
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

		public IEnumerable<ISeries> Proxy_Series => throw new NotImplementedException();

		public DesignStressTestProcessViewModel()
		{
			var bots = new List<TimeSpanPoint>();
			var cps = new List<TimeSpanPoint>();
			TimeSpan time = TimeSpan.Zero;
			Random r = new();
			for (int i = 0; i < 30; i++)
			{
				bots.Add(new(time, r.Next(0, 100)));
				cps.Add(new(time, r.Next(0, 10)));
				time += TimeSpan.FromSeconds(1);

			}
			BotsOnline_Series = new ISeries[]
			{
				new LineSeries<TimeSpanPoint>
				{

				}
			};
		}

	}
}
