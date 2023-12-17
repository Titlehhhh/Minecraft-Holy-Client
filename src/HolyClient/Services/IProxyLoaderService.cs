using DynamicData;
using HolyClient.Common;
using QuickProxyNet;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace HolyClient.Services
{
	public interface IProxyLoaderService
	{
		Task<int> Load(Stream stream, ProxyType type, ISourceList<ProxyInfo> sourceList);
	}

}
public class ProxyComparer : IEqualityComparer<ProxyInfo>
{
	public bool Equals(ProxyInfo? x, ProxyInfo? y)
	{
		if (x is null)
		{
			return false;
		}
		if (y is null)
		{
			return false;
		}



		return x.Host == y.Host && x.Port == y.Port && x.Type == y.Type;
	}

	public int GetHashCode([DisallowNull] ProxyInfo obj)
	{
		unchecked
		{
			int hash = 27;
			hash = (13 * hash) + obj.Host.GetHashCode();
			hash = (13 * hash) + obj.Port.GetHashCode();

			return hash;
		}
	}
}
