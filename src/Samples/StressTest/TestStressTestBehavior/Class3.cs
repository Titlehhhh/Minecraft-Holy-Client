using HolyClient.Abstractions.StressTest;
using HolyClient.SDK.Attributes;
using System.Reactive.Disposables;

namespace TestStressTestBehavior
{
	[PluginAuthor("Title")]
	[PluginDescription("asdasd")]
	[PluginTitle("ASdasd")]
	public class Class3 : IStressTestBehavior
	{
		public Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}

}
