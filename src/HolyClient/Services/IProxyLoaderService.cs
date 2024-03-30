using DynamicData;
using HolyClient.Common;
using QuickProxyNet;
using System.IO;
using System.Threading.Tasks;

namespace HolyClient.Services
{
	public interface IProxyLoaderService
	{
		Task<int> Load(Stream stream, ProxyType type, ISourceList<ProxyInfo> sourceList);
	}

}
