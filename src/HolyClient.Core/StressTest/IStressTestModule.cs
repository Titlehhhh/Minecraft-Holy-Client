using System.Reactive.Disposables;

namespace HolyClient.Core.StressTest
{
    public interface IStressTestModule
    {
        public void Activate(CompositeDisposable disposables);
    }
}
