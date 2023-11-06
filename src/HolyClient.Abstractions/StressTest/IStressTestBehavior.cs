using System.Reactive.Disposables;

namespace HolyClient.Abstractions.StressTest
{
	public interface IStressTestBehavior
	{

		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken);
	}
}
