using System.Net;

namespace QuickProxyNet.ProxyChecker;

public record ProxyRecord(ProxyType Type, string Host, int Port,NetworkCredential? Credentials);