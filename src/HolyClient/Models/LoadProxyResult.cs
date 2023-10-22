using QuickProxyNet;

namespace HolyClient.Models;

public class ImportProxyResult
{
	public string Path { get; }
	public ProxyType Type { get; }

	public ImportProxyResult(string path, ProxyType type)
	{
		Path = path;
		Type = type;
	}
}




