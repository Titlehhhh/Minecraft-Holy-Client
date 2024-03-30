using MessagePack;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.StressTest
{
	[MessagePackObject(keyAsPropertyName: true)]
	public sealed class ProxyCheckerOptions : ReactiveObject
	{
		[Reactive]
		public int ParallelCount { get; set; }
		public int ConnectTimeout { get; set; } = 2500;
		public int SendTimeout { get; set; } = 10_000;
		public int ReadTimeout { get; set; } = 10_000;
		public string TargetHost { get; set; }
		public ushort TargetPort { get; set; }
	}


}
