using HolyClient.Abstractions.StressTest;
using HolyClient.SDK.Attributes;
using Serilog;

namespace TestStressTestBehavior
{
	[PluginAuthor("Title")]
	[PluginDescription("asdasd")]
	[PluginTitle("ASdasd")]
	public class Class3 : BaseStressTestBehavior
	{
		public override Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, ILogger logger, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}

}
