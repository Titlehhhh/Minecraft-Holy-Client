using HolyClient.Abstractions.StressTest;
using HolyClient.SDK.Attributes;
using System.Reactive.Disposables;

namespace TestStressTestBehavior
{
	[PluginAuthor("Title")]
	[PluginDescription("asdasd")]
	public class Class2 : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}

}
