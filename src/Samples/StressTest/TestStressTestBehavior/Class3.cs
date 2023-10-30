using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;

namespace TestStressTestBehavior
{
	public class Class3 : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}

}
