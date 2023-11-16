using HolyClient.Common;
using System.Diagnostics.CodeAnalysis;

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



		return x.Host == y.Host && x.Port == y.Port;
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