#if SERIALIZABLE
using System.Security;
using System.Runtime.Serialization;
#endif

namespace QuickProxyNet
{
#if SERIALIZABLE
	[Serializable]
#endif
    public class ProxyProtocolException : Exception
    {
#if SERIALIZABLE
																								[SecuritySafeCritical]
		protected ProxyProtocolException (SerializationInfo info, StreamingContext context) : base (info, context)
		{
		}
#endif

        public ProxyProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ProxyProtocolException(string message) : base(message)
        {
        }

        public ProxyProtocolException()
        {
        }
    }
}
