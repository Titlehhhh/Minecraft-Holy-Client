using McProtoNet.MultiVersion;

namespace HolyClient.Abastractions.StressTest
{
    public interface IStressTestBot
    {
        public Task Restart(bool changeNickAndProxy);
        IObservable<Exception> OnError { get; }

		public MinecraftClient Client { get; }
    }

}
