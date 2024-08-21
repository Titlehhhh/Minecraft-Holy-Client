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

    public static bool TryParse(string line, ProxyType type, out ProxyInfo proxy)
    {
        return TryParse(line, "", type, out proxy);
    }

    //host:port
    public static bool TryParse(string line, string format, ProxyType type, out ProxyInfo proxy)
    {
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
            return Host == proxyInfo.Host
                   && Port == proxyInfo.Port
                   && Type == proxyInfo.Type
                   && Login == proxyInfo.Login
                   && Password == proxyInfo.Password;
        return false;
    }
}