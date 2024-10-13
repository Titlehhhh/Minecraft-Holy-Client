using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace QuickProxyNet;

public class HttpsProxyClient : ProxyClient
{
    private SslCertificateValidationInfo sslValidationInfo;

    public HttpsProxyClient(string host, int port) : base("https", host, port)
    {
    }

    public HttpsProxyClient(string host, int port, NetworkCredential credentials) : base("https", host, port,
        credentials)
    {
    }

    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

    public bool CheckCertificateRevocation { get; set; }

    public X509CertificateCollection ClientCertificates { get; set; }

    public CipherSuitesPolicy SslCipherSuitesPolicy { get; set; }

    public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls12 | SslProtocols.Tls13;

    public override ProxyType Type => ProxyType.HTTPS;

    private SslClientAuthenticationOptions GetSslClientAuthenticationOptions(string host,
        RemoteCertificateValidationCallback remoteCertificateValidationCallback)
    {
        return new SslClientAuthenticationOptions
        {
            CertificateRevocationCheckMode =
                CheckCertificateRevocation ? X509RevocationMode.Online : X509RevocationMode.NoCheck,
            ApplicationProtocols = new List<SslApplicationProtocol> { SslApplicationProtocol.Http11 },
            RemoteCertificateValidationCallback = remoteCertificateValidationCallback,

            CipherSuitesPolicy = SslCipherSuitesPolicy,

            ClientCertificates = ClientCertificates,
            EnabledSslProtocols = SslProtocols,
            TargetHost = host
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override async ValueTask<Stream> ConnectAsync(Stream stream, string host, int port,
        CancellationToken cancellationToken = default)
    {
        var ssl = new SslStream(stream,false, ValidateRemoteCertificate);
        try
        {
            //await Task.Delay(100);
            //await ssl.AuthenticateAsClientAsync(host);
            
           // if(false)
            await ssl.AuthenticateAsClientAsync(GetSslClientAuthenticationOptions(host, ValidateRemoteCertificate),
                cancellationToken);
        }
        catch (Exception e)
        {
            ssl.Dispose();
            throw;
        }

        var result =
            await HttpHelper.EstablishHttpTunnelAsync(ssl, ProxyUri, host, port, ProxyCredentials, cancellationToken);
        return result;
    }

    private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        bool valid;

        sslValidationInfo?.Dispose();
        sslValidationInfo = null;

        if (ServerCertificateValidationCallback != null)
            valid = ServerCertificateValidationCallback(ProxyHost, certificate, chain, sslPolicyErrors);
        else if (ServicePointManager.ServerCertificateValidationCallback != null)
            valid = ServicePointManager.ServerCertificateValidationCallback(ProxyHost, certificate, chain,
                sslPolicyErrors);
        else
            valid = sslPolicyErrors == SslPolicyErrors.None;

        if (!valid)
            // Note: The SslHandshakeException.Create() method will nullify this once it's done using it.
            sslValidationInfo = new SslCertificateValidationInfo(sender, certificate, chain, sslPolicyErrors);

        return valid;
    }

    private sealed class SslChainElement : IDisposable
    {
        public readonly X509Certificate2 Certificate;
        public readonly X509ChainStatus[] ChainElementStatus;
        public readonly string Information;

        public SslChainElement(X509ChainElement element)
        {
            Certificate = new X509Certificate2(element.Certificate.RawData);
            ChainElementStatus = element.ChainElementStatus;
            Information = element.Information;
        }

        public void Dispose()
        {
            Certificate.Dispose();
        }
    }

    private sealed class SslCertificateValidationInfo : IDisposable
    {
        public readonly X509Certificate2 Certificate;
        public readonly List<SslChainElement> ChainElements;
        public readonly X509ChainStatus[] ChainStatus;
        public readonly string Host;
        public readonly SslPolicyErrors SslPolicyErrors;

        public SslCertificateValidationInfo(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            Certificate = new X509Certificate2(certificate.Export(X509ContentType.Cert));
            ChainElements = new List<SslChainElement>();
            SslPolicyErrors = sslPolicyErrors;
            ChainStatus = chain.ChainStatus;
            Host = sender as string;

            // Note: we need to copy the ChainElements because the chain will be destroyed
            foreach (var element in chain.ChainElements)
                ChainElements.Add(new SslChainElement(element));
        }

        public void Dispose()
        {
            Certificate.Dispose();
            foreach (var element in ChainElements)
                element.Dispose();
        }
    }
}