using HolyClient.Abastractions.StressTest;


namespace HolyClient.Contracts;
public interface IStressTestBehaviorFactory
{

	string FullName { get; }
	IStressTestBehavior Create();
}

