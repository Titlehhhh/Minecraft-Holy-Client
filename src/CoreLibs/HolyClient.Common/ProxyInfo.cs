using System.Diagnostics.CodeAnalysis;
using System.Net;
using QuickProxyNet;

namespace HolyClient.Common;

public record struct ProxyInfo
{
    public ProxyType Type { get; set; }
    public string Host { get; set; }
    public ushort Port { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }

    public static bool TryParse(string line, ProxyType? type, out ProxyInfo proxy)
    {
        return TryParse(line, "", type, out proxy);
    }

    
    public static bool TryParse(string line, string format, ProxyType? type, out ProxyInfo proxy)
    {
        if (type is null)
        {
            Uri uri = new Uri(line);
            ProxyType typeFromScheme = uri.Scheme switch
            {
                "http" => ProxyType.Http,
                "https" => ProxyType.Https,
                "socks4" => ProxyType.Socks4,
                "socks4a" => ProxyType.Socks4a,
                "socks5" => ProxyType.Socks5,
                _ => throw new NotSupportedException("No support proxy type: " + uri.Scheme)
            };
            string? login = "";
            string? pass = "";
            if (!string.IsNullOrEmpty(uri.UserInfo) && uri.UserInfo.Contains(':'))
            {
                login = uri.UserInfo.Split(':')[0];
                pass = uri.UserInfo.Split(':')[1];
            }

            proxy = new ProxyInfo()
            {
                Host = uri.Host,
                Port = (ushort)uri.Port,
                Login = login,
                Password = pass,
                Type = typeFromScheme
            };
            return true;
        }


        var hostPort = line.Split(':');
        if (hostPort.Length != 2)
        {
            proxy = default;
            return false;
        }

        if (ushort.TryParse(hostPort[1], out var port))
        {
            proxy = new ProxyInfo
            {
                Host = hostPort[0],
                Port = port,
                Type = (ProxyType)type
            };

            return true;
        }

        proxy = default;
        return false;
    }
    
}