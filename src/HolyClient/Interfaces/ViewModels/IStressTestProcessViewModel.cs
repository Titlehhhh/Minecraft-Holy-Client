using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;

namespace HolyClient.ViewModels;

public interface IStressTestProcessViewModel : IReactiveObject, IRoutableViewModel, IActivatableViewModel
{
    ObservableCollection<LogEventViewModel> Logs { get; }
    string Host { get; }

    string Version { get; }

    string ParallelCount { get; }

    int BotsOnline { get; }

    int CPS { get; }

    int PeakCPS { get; }

    string ProxyQuality { get; }

    IEnumerable<ISeries> Proxy_Series { get; }

    ICommand CancelCommand { get; }
    ObservableCollection<ISeries> BotsOnlineSeries { get; }
    ObservableCollection<ISeries> CPSSeries { get; }

    object BotsOnline_Sync { get; }
    object CPS_Sync { get; }

    Axis[] BotsAxis { get; }
    Axis[] CPSAxis { get; }
    Margin DrawMargin { get; }

    IEnumerable<ExceptionInfoViewModel> Exceptions { get; }
}