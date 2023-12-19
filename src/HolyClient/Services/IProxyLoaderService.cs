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
