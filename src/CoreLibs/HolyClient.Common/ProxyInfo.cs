using MessagePack;
using QuickProxyNet;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace HolyClient.Common
{

	public struct ProxyInfo
	{


		public ProxyType Type { get; set; }



		public string Host { get; set; }

		public ushort Port { get; set; }



		public string? Login { get; set; }



		public string? Password { get; set; }

		public static bool TryParse(string line, ProxyType type, out ProxyInfo proxy)
		{
			return TryParse(line, "", type, out proxy);
		}
		//host:port
		public static bool TryParse(string line, string format, ProxyType type, out ProxyInfo proxy)
		{
			string[] hostPort = line.Split(':');
			if (hostPort.Length != 2)
			{
				proxy = default;
				return false;
			}

			if (ushort.TryParse(hostPort[1], out var port))
			{

				proxy = new ProxyInfo()
				{
					Host = hostPort[0],
					Port = port,
					Type = type
				};

				return true;
			}
			proxy = default;
			return false;
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if (obj is ProxyInfo proxyInfo)
			{
				return this.Host == proxyInfo.Host
					&& this.Port == proxyInfo.Port
					&& this.Type == proxyInfo.Type
					&& this.Login == proxyInfo.Login
					&& this.Password == proxyInfo.Password;
			}
			return false;
		}

	}

}
