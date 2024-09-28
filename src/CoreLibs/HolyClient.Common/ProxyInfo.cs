using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using QuickProxyNet;

namespace HolyClient.Common;

public struct ProxyInfo
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
                "http" => ProxyType.HTTP,
                "https" => ProxyType.HTTPS,
                "socks4" => ProxyType.SOCKS4,
                "socks4a" => ProxyType.SOCKS4a,
                "socks5" => ProxyType.SOCKS5,
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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ProxyInfo proxyInfo)
            return Host == proxyInfo.Host
                   && Port == proxyInfo.Port
                   && Type == proxyInfo.Type
                   && Login == proxyInfo.Login
                   && Password == proxyInfo.Password;
        return false;
    }
}