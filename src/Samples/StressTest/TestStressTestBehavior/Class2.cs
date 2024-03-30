using HolyClient.Abstractions.StressTest;
using HolyClient.SDK.Attributes;
using Serilog;

namespace TestStressTestBehavior
{
	[PluginAuthor("Title")]
	[PluginDescription("asdasd")]
	public class Class2 : BaseStressTestBehavior
	{
		public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}

}
