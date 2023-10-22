using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using System.Collections.Generic;
using System.Windows.Input;

namespace HolyClient.ViewModels
{
	public interface IStressTestProcessViewModel : IReactiveObject, IRoutableViewModel, IActivatableViewModel
	{
		IEnumerable<ISeries> Proxy_Series { get; }
		IEnumerable<ISeries> BotsOnline_Series { get; }
		IEnumerable<ISeries> CPS_Series { get; }
		ICommand CancelCommand { get; }


		Axis[] XAxes { get; }
	}
}