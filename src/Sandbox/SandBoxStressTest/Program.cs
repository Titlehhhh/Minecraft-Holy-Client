using System.Runtime.Intrinsics.X86;
using HolyClient.StressTest;
using McProtoNet;
using McProtoNet.Client;
using McProtoNet.Protocol;
using McProtoNet.Protocol754;
using MemoryPack;
using Serilog;


internal class Program
{
    public static async Task Main(string[] args)
    {
        
        string host = args[0];



        StressTestProfile stressTestProfile = new StressTestProfile();

        stressTestProfile.Version = 754;
        stressTestProfile.BotsNickname = "_Title";
        stressTestProfile.UseProxy = true;

        stressTestProfile.Server = host;

        stressTestProfile.ProxyCheckerOptions = new ProxyCheckerOptions()
        {
            ParallelCount = 30_000
        };
        stressTestProfile.SetBehavior(new DefaultPluginSource());
        stressTestProfile.NumberOfBots = 1000;

        var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

        await stressTestProfile.Start(logger);

        await Task.Delay(-1);
    }
    
}
