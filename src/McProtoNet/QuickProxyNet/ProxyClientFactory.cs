using System.Net;

namespace QuickProxyNet
{
    public sealed class ProxyClientFactory
    {
        public IProxyClient Create(ProxyType type, string host, int port)
        {
            return type switch
            {
                ProxyType.HTTP => new HttpProxyClient(host, port),
                ProxyType.HTTPS => new HttpsProxyClient(host, port),
                ProxyType.SOCKS4 => new Socks4Client(host, port),
                ProxyType.SOCKS4a => new Socks4aClient(host, port),
                ProxyType.SOCKS5 => new Socks5Client(host, port)
            };
        }
        public IProxyClient Create(ProxyType type, string host, int port, NetworkCredential networkCredential)
        {
            return type switch
            {
                ProxyType.HTTP => new HttpProxyClient(host, port, networkCredential),
                ProxyType.HTTPS => new HttpsProxyClient(host, port, networkCredential),
                ProxyType.SOCKS4 => new Socks4Client(host, port, networkCredential),
                ProxyType.SOCKS4a => new Socks4aClient(host, port, networkCredential),
                ProxyType.SOCKS5 => new Socks5Client(host, port, networkCredential)
            };
        }
    }
}
