using HolyClient.Abstractions.StressTest;
using System.Reactive.Disposables;

namespace TestStressTestBehavior
{
	public class Class1 : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
	public class Class2 : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
	public class Class3 : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}
