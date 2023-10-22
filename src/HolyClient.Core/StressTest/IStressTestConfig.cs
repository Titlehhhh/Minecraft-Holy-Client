using HolyClient.Core.Services;
using McProtoNet.MultiVersion;

namespace HolyClient.Core.StressTest
{
	public interface IStressTestConfig
    {
        public INickProvider NickProvider { get; }
        public IProxyProvider ProxyProvider { get; }

        public string Host { get; }
        public ushort Port { get; }

        public int NumberOfBots { get; }

        public MinecraftVersion Version { get; }


    }

}
