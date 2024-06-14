using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LiveChartsCore;
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
}