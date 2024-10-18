using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace QuickProxyNet;

internal static class SocksHelper
{
    // Largest possible message size is 513 bytes (Socks5 username & password auth)
    private const int BufferSize = 513;
    private const int ProtocolVersion4 = 4;
    private const int ProtocolVersion5 = 5;
    private const int SubnegotiationVersion = 1; // Socks5 username & password auth
    private const byte METHOD_NO_AUTH = 0;
    private const byte METHOD_USERNAME_PASSWORD = 2;
    private const byte CMD_CONNECT = 1;
    private const byte ATYP_IPV4 = 1;
    private const byte ATYP_DOMAIN_NAME = 3;
    private const byte ATYP_IPV6 = 4;
    private const byte Socks5_Success = 0;
    private const byte Socks4_Success = 90;
    private const byte Socks4_AuthFailed = 93;


    internal static async ValueTask EstablishSocks5TunnelAsync(Stream stream, string host, int port,
        NetworkCredential? credentials, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
        try
        {
            // https://tools.ietf.org/html/rfc1928

            // +----+----------+----------+
            // |VER | NMETHODS | METHODS  |
            // +----+----------+----------+
            // | 1  |    1     | 1 to 255 |
            // +----+----------+----------+
            buffer[0] = ProtocolVersion5;
            if (credentials is null)
            {
                buffer[1] = 1;
                buffer[2] = METHOD_NO_AUTH;
            }
            else
            {
                buffer[1] = 2;
                buffer[2] = METHOD_NO_AUTH;
                buffer[3] = METHOD_USERNAME_PASSWORD;
            }

            await WriteAsync(stream, buffer.AsMemory(0, buffer[1] + 2), cancellationToken).ConfigureAwait(false);

            // +----+--------+
            // |VER | METHOD |
            // +----+--------+
            // | 1  |   1    |
            // +----+--------+
            await stream.ReadExactlyAsync(buffer.AsMemory(0, 2), cancellationToken).ConfigureAwait(false);
            VerifyProtocolVersion(ProtocolVersion5, buffer[0]);

            switch (buffer[1])
            {
                case METHOD_NO_AUTH:
                    // continue
                    break;

                case METHOD_USERNAME_PASSWORD:
                {
                    // https://tools.ietf.org/html/rfc1929
                    if (credentials is null)
                        // If the server is behaving well, it shouldn't pick username and password auth
                        // because we don't claim to support it when we don't have credentials.
                        // Just being defensive here.
                        throw new ProxyProtocolException("SOCKS server requested username & password authentication.");

                    // +----+------+----------+------+----------+
                    // |VER | ULEN |  UNAME   | PLEN |  PASSWD  |
                    // +----+------+----------+------+----------+
                    // | 1  |  1   | 1 to 255 |  1   | 1 to 255 |
                    // +----+------+----------+------+----------+
                    buffer[0] = SubnegotiationVersion;
                    var usernameLength = EncodeString(credentials.UserName, buffer.AsSpan(2),
                        nameof(credentials.UserName));
                    buffer[1] = usernameLength;
                    var passwordLength = EncodeString(credentials.Password, buffer.AsSpan(3 + usernameLength),
                        nameof(credentials.Password));
                    buffer[2 + usernameLength] = passwordLength;
                    await WriteAsync(stream, buffer.AsMemory(0, 3 + usernameLength + passwordLength), cancellationToken)
                        .ConfigureAwait(false);

                    // +----+--------+
                    // |VER | STATUS |
                    // +----+--------+
                    // | 1  |   1    |
                    // +----+--------+
                    await stream.ReadExactlyAsync(buffer.AsMemory(0, 2), cancellationToken).ConfigureAwait(false);
                    if (buffer[0] != SubnegotiationVersion || buffer[1] != Socks5_Success)
                        throw new ProxyProtocolException("Failed to authenticate with the SOCKS server.");
                    break;
                }

                default:
                    throw new ProxyProtocolException("SOCKS server did not return a suitable authentication method.");
            }


            // +----+-----+-------+------+----------+----------+
            // |VER | CMD |  RSV  | ATYP | DST.ADDR | DST.PORT |
            // +----+-----+-------+------+----------+----------+
            // | 1  |  1  | X'00' |  1   | Variable |    2     |
            // +----+-----+-------+------+----------+----------+
            buffer[0] = ProtocolVersion5;
            buffer[1] = CMD_CONNECT;
            buffer[2] = 0;
            int addressLength;

            if (IPAddress.TryParse(host, out var hostIP))
            {
                if (hostIP.AddressFamily == AddressFamily.InterNetwork)
                {
                    buffer[3] = ATYP_IPV4;
                    hostIP.TryWriteBytes(buffer.AsSpan(4), out var bytesWritten);
                    Debug.Assert(bytesWritten == 4);
                    addressLength = 4;
                }
                else
                {
                    Debug.Assert(hostIP.AddressFamily == AddressFamily.InterNetworkV6);
                    buffer[3] = ATYP_IPV6;
                    hostIP.TryWriteBytes(buffer.AsSpan(4), out var bytesWritten);
                    Debug.Assert(bytesWritten == 16);
                    addressLength = 16;
                }
            }
            else
            {
                buffer[3] = ATYP_DOMAIN_NAME;
                var hostLength = EncodeString(host, buffer.AsSpan(5), nameof(host));
                buffer[4] = hostLength;
                addressLength = hostLength + 1;
            }

            BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(addressLength + 4), (ushort)port);

            await WriteAsync(stream, buffer.AsMemory(0, addressLength + 6), cancellationToken).ConfigureAwait(false);

            // +----+-----+-------+------+----------+----------+
            // |VER | REP |  RSV  | ATYP | DST.ADDR | DST.PORT |
            // +----+-----+-------+------+----------+----------+
            // | 1  |  1  | X'00' |  1   | Variable |    2     |
            // +----+-----+-------+------+----------+----------+
            await stream.ReadExactlyAsync(buffer.AsMemory(0, 5), cancellationToken).ConfigureAwait(false);
            VerifyProtocolVersion(ProtocolVersion5, buffer[0]);
            if (buffer[1] != Socks5_Success)
                throw new ProxyProtocolException("SOCKS server failed to connect to the destination.");
            var bytesToSkip = buffer[3] switch
            {
                ATYP_IPV4 => 5,
                ATYP_IPV6 => 17,
                ATYP_DOMAIN_NAME => buffer[4] + 2,
                _ => throw new ProxyProtocolException("SOCKS server returned an unknown address type.")
            };
            await stream.ReadExactlyAsync(buffer.AsMemory(0, bytesToSkip), cancellationToken).ConfigureAwait(false);
            // response address not used
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    internal static async ValueTask EstablishSocks4TunnelAsync(Stream stream, bool isVersion4a, string host, int port,
        NetworkCredential? credentials, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);

        try
        {
            // https://www.openssh.com/txt/socks4.protocol

            // +----+----+----+----+----+----+----+----+----+----+....+----+
            // | VN | CD | DSTPORT |      DSTIP        | USERID       |NULL|
            // +----+----+----+----+----+----+----+----+----+----+....+----+
            //    1    1      2              4           variable       1
            buffer[0] = ProtocolVersion4;
            buffer[1] = CMD_CONNECT;

            BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(2), (ushort)port);

            IPAddress? ipv4Address = null;
            if (IPAddress.TryParse(host, out var hostIP))
            {
                if (hostIP.AddressFamily == AddressFamily.InterNetwork)
                    ipv4Address = hostIP;
                else if (hostIP.IsIPv4MappedToIPv6)
                    ipv4Address = hostIP.MapToIPv4();
                else
                    throw new ProxyProtocolException("SOCKS4 does not support IPv6 addresses.");
            }
            else if (!isVersion4a)
            {
                // Socks4 does not support domain names - try to resolve it here
                IPAddress[] addresses;
                try
                {
                    addresses =
                        await Dns.GetHostAddressesAsync(host, AddressFamily.InterNetwork, cancellationToken)
                            .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new ProxyProtocolException("Failed to resolve the destination host to an IPv4 address.s", ex);
                }

                if (addresses.Length == 0)
                    throw new ProxyProtocolException("Failed to resolve the destination host to an IPv4 address.s");

                ipv4Address = addresses[0];
            }

            if (ipv4Address is null)
            {
                Debug.Assert(isVersion4a);
                buffer[4] = 0;
                buffer[5] = 0;
                buffer[6] = 0;
                buffer[7] = 255;
            }
            else
            {
                ipv4Address.TryWriteBytes(buffer.AsSpan(4), out var bytesWritten);
                Debug.Assert(bytesWritten == 4);
            }

            var usernameLength = EncodeString(credentials?.UserName, buffer.AsSpan(8), nameof(credentials.UserName));
            buffer[8 + usernameLength] = 0;
            var totalLength = 9 + usernameLength;

            if (ipv4Address is null)
            {
                // https://www.openssh.com/txt/socks4a.protocol
                var hostLength = EncodeString(host, buffer.AsSpan(totalLength), nameof(host));
                buffer[totalLength + hostLength] = 0;
                totalLength += hostLength + 1;
            }

            await WriteAsync(stream, buffer.AsMemory(0, totalLength), cancellationToken).ConfigureAwait(false);

            // +----+----+----+----+----+----+----+----+
            // | VN | CD | DSTPORT |      DSTIP        |
            // +----+----+----+----+----+----+----+----+
            //    1    1      2              4


            await stream.ReadExactlyAsync(buffer.AsMemory(0, 8), cancellationToken).ConfigureAwait(false);

            switch (buffer[1])
            {
                case Socks4_Success:
                    // Nothing to do
                    break;
                case Socks4_AuthFailed:
                    throw new ProxyProtocolException("Failed to authenticate with the SOCKS server.");
                default:
                    throw new ProxyProtocolException("SOCKS server failed to connect to the destination.");
            }
            // response address not used
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static byte EncodeString(ReadOnlySpan<char> chars, Span<byte> buffer, string parameterName)
    {
        try
        {
            return checked((byte)Encoding.UTF8.GetBytes(chars, buffer));
        }
        catch
        {
            Debug.Assert(Encoding.UTF8.GetByteCount(chars) > 255);
            throw new ProxyProtocolException($"Encoding the {parameterName} took more than the maximum of 255 bytes");
        }
    }

    private static void VerifyProtocolVersion(byte expected, byte version)
    {
        if (expected != version)
            throw new ProxyProtocolException(
                $"Unexpected SOCKS protocol version. Required {expected}, got {version}.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ValueTask WriteAsync(Stream stream, Memory<byte> buffer, CancellationToken cancellationToken)
    {
        return stream.WriteAsync(buffer, cancellationToken);
    }
}