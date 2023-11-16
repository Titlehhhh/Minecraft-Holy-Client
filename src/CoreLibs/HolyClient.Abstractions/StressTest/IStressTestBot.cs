using McProtoNet;

namespace HolyClient.Abstractions.StressTest
{

	public interface IStressTestBot
	{
		public Task Restart(bool changeNickAndProxy);
		IObservable<Exception> OnError { get; }

		public MinecraftClient Client { get; }
	}

}
