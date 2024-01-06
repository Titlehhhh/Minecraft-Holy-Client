using MessagePack;
using QuickProxyNet;

namespace HolyClient.AppState;

[MessagePackObject(keyAsPropertyName: true)]
[MessagePack.Union(0, typeof(ManualEntryProxySourceState))]
[MessagePack.Union(1, typeof(FileProxySourceState))]
[MessagePack.Union(2, typeof(UrlProxySourceState))]
public abstract class ProxySourceState
{
	public ProxyType Type { get; set; }
}
