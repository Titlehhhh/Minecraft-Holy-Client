using McProtoNet.MultiVersion;
using System.Reactive;
using System.Reactive.Disposables;

namespace HolyClient.Abastractions.StressTest
{
    /// <summary>
    /// Интерфейс представляющий поведение для стресс-теста
    /// </summary>
    public interface IStressTestBehavior
    {
        public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots,CancellationToken cancellationToken);
    }







}
