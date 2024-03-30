using McProtoNet;

namespace HolyClient.Abstractions.StressTest
{

	public interface IStressTestBot
	{
		public Task Restart(bool changeNickAndProxy);


		public MinecraftClient Client { get; }
	}

}
