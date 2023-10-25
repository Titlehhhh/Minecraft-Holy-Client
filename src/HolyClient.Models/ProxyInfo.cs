using MessagePack;
using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace HolyClient.Common
{
	[MessagePackObject(keyAsPropertyName: true)]
	public sealed class ProxyInfo : ReactiveObject
	{

		[Reactive]
		public ProxyType Type { get; set; }


		[Reactive]
		public string Host { get; set; }
		[Reactive]
		public ushort Port { get; set; }


		[Reactive]
		public string? Login { get; set; }


		[Reactive]
		public string? Password { get; set; }
	}

}
