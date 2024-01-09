using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;

namespace HolyClient.StressTest
{
	public class TidePVPBehaviorAttack : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}

}
