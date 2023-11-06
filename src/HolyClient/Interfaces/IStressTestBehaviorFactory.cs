using HolyClient.Abstractions.StressTest;


namespace HolyClient.Contracts;
public interface IStressTestBehaviorFactory
{

	string FullName { get; }
	IStressTestBehavior Create();
}

