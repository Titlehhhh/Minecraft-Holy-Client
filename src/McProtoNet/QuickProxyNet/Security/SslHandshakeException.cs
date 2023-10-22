//
// SslHandshakeException.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>


using System.Text;
using System.Net.Security;
using System.Globalization;
#if SERIALIZABLE
using System.Security;
using System.Runtime.Serialization;
#endif
using System.Security.Cryptography.X509Certificates;

namespace QuickProxyNet
{

#if SERIALIZABLE
	[Serializable]
#endif
    public class SslHandshakeException : Exception
    {
        const string SslHandshakeHelpLink = "https://github.com/jstedfast/MailKit/blob/master/FAQ.md#SslHandshakeException";
        const string DefaultMessage = "An error occurred while attempting to establish an SSL or TLS connection.";

#if SERIALIZABLE
																								protected SslHandshakeException (SerializationInfo info, StreamingContext context) : base (info, context)
		{
			var base64 = info.GetString ("ServerCertificate");

			if (base64 != null)
				ServerCertificate = new X509Certificate2 (Convert.FromBase64String (base64));

			base64 = info.GetString ("RootCertificateAuthority");

			if (base64 != null)
				RootCertificateAuthority = new X509Certificate2 (Convert.FromBase64String (base64));
		}
#endif

        public SslHandshakeException(string message, Exception innerException) : base(message, innerException)
        {
            HelpLink = SslHandshakeHelpLink;
        }

        public SslHandshakeException(string message) : base(message)
        {
            HelpLink = SslHandshakeHelpLink;
        }

        public SslHandshakeException() : base(DefaultMessage)
        {
            HelpLink = SslHandshakeHelpLink;
        }

        public X509Certificate ServerCertificate
        {
            get; private set;
        }

        public X509Certificate RootCertificateAuthority
        {
            get; private set;
        }

#if SERIALIZABLE
																												[SecurityCritical]
		public override void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);

			if (ServerCertificate != null)
				info.AddValue ("ServerCertificate", Convert.ToBase64String (ServerCertificate.GetRawCertData ()));
			else
				info.AddValue ("ServerCertificate", null, typeof (string));

			if (RootCertificateAuthority != null)
				info.AddValue ("RootCertificateAuthority", Convert.ToBase64String (RootCertificateAuthority.GetRawCertData ()));
			else
				info.AddValue ("RootCertificateAuthority", null, typeof (string));
		}
#endif

        internal static SslHandshakeException Create(ref SslCertificateValidationInfo validationInfo, Exception ex, bool starttls, string protocol, string host, int port, int sslPort, params int[] standardPorts)
        {
            var message = new StringBuilder(DefaultMessage);
            X509Certificate certificate = null;
            X509Certificate root = null;

            if (ex is AggregateException aggregate)
            {
                aggregate = aggregate.Flatten();

                if (aggregate.InnerExceptions.Count == 1)
                    ex = aggregate.InnerExceptions[0];
                else
                    ex = aggregate;
            }

            message.AppendLine();
            message.AppendLine();

            if (validationInfo != null)
            {
                try
                {
                    int rootIndex = validationInfo.ChainElements.Count - 1;
                    if (rootIndex > 0)
                        root = new X509Certificate2(validationInfo.ChainElements[rootIndex].Certificate.RawData);
                    certificate = new X509Certificate2(validationInfo.Certificate.RawData);

                    if ((validationInfo.SslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != 0)
                    {
                        message.AppendLine("The SSL certificate for the server was not available.");
                    }
                    else if ((validationInfo.SslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
                    {
                        message.AppendLine("The host name did not match the name given in the server's SSL certificate.");
                    }
                    else
                    {
                        message.AppendLine("The server's SSL certificate could not be validated for the following reasons:");

                        bool haveReason = false;

                        for (int chainIndex = 0; chainIndex < validationInfo.ChainElements.Count; chainIndex++)
                        {
                            var element = validationInfo.ChainElements[chainIndex];

                            if (element.ChainElementStatus == null || element.ChainElementStatus.Length == 0)
                                continue;

                            if (chainIndex == 0)
                            {
                                message.AppendLine("\u2022 The server certificate has the following errors:");
                            }
                            else if (chainIndex == rootIndex)
                            {
                                message.AppendLine("\u2022 The root certificate has the following errors:");
                            }
                            else
                            {
                                message.AppendLine("\u2022 An intermediate certificate has the following errors:");
                            }

                            foreach (var status in element.ChainElementStatus)
                            {
                                message.Append("  \u2022 ");
                                message.AppendLine(status.StatusInformation);
                            }

                            haveReason = true;
                        }

                        // Note: Because Mono does not include any elements in the chain (at least on macOS), we need
                        // to find the inner-most exception and append its Message.
                        if (!haveReason)
                        {
                            var innerException = ex;

                            while (innerException.InnerException != null)
                                innerException = innerException.InnerException;

                            message.AppendLine("\u2022 " + innerException.Message);
                        }
                    }
                }
                finally
                {
                    validationInfo.Dispose();
                    validationInfo = null;
                }
            }
            else if (!starttls && standardPorts.Contains(port))
            {
                string an = "AEHIOS".IndexOf(protocol[0]) != -1 ? "an" : "a";

                message.AppendFormat(CultureInfo.InvariantCulture, "When connecting to {0} {1} service, port {2} is typically reserved for plain-text connections. If{3}", an, protocol, port, Environment.NewLine);
                message.AppendFormat(CultureInfo.InvariantCulture, "you intended to connect to {0} on the SSL port, try connecting to port {1} instead. Otherwise,{2}", protocol, sslPort, Environment.NewLine);
                message.AppendLine("if you intended to use STARTTLS, make sure to use the following code:");
                message.AppendLine();
                message.AppendFormat("client.Connect (\"{0}\", {1}, SecureSocketOptions.StartTls);{2}", host, port, Environment.NewLine);
            }
            else
            {
                message.AppendLine("This usually means that the SSL certificate presented by the server is not trusted by the system for one or more of");
                message.AppendLine("the following reasons:");
                message.AppendLine();
                message.AppendLine("1. The server is using a self-signed certificate which cannot be verified.");
                message.AppendLine("2. The local system is missing a Root or Intermediate certificate needed to verify the server's certificate.");
                message.AppendLine("3. A Certificate Authority CRL server for one or more of the certificates in the chain is temporarily unavailable.");
                message.AppendLine("4. The certificate presented by the server is expired or invalid.");
                message.AppendLine("5. The set of SSL/TLS protocols supported by the client and server do not match.");
                if (!starttls)
                    message.AppendLine("6. You are trying to connect to a port which does not support SSL/TLS.");
                message.AppendLine();
                message.AppendLine("See " + SslHandshakeHelpLink + " for possible solutions.");
            }

            return new SslHandshakeException(message.ToString(), ex) { ServerCertificate = certificate, RootCertificateAuthority = root };
        }
    }

    sealed class SslChainElement : IDisposable
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

    sealed class SslCertificateValidationInfo : IDisposable
    {
        public readonly List<SslChainElement> ChainElements;
        public readonly X509ChainStatus[] ChainStatus;
        public readonly SslPolicyErrors SslPolicyErrors;
        public readonly X509Certificate2 Certificate;
        public readonly string Host;

        public SslCertificateValidationInfo(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
