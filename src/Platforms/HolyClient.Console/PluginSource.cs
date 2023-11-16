using HolyClient.Abstractions.StressTest;
using HolyClient.Core.Infrastructure;

internal partial class Program
{
	internal class PluginSource : IPluginSource
	{
		public PluginMetadata Metadata => new("", "", "");

		public PluginTypeReference Reference => new PluginTypeReference("Test", "Test");

		public T CreateInstance<T>() where T : IStressTestBehavior
		{
			IStressTestBehavior stressTest = new TestBehavior();
			return (T)stressTest;
		}
	}


}
