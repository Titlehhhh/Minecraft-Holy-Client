using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Abstractions;
using McProtoNet.Cryptography;
using McProtoNet.Net;
using McProtoNet.Serialization;

namespace McProtoNet.Client;

internal sealed class MinecraftClientLogin
{
    private static readonly byte[] VarIntLoginIntent;
    private static readonly byte[] LoginAcknowledged;

    private static readonly MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

    static MinecraftClientLogin()
    {
        VarIntLoginIntent = VarIntToBytes(0x02);
        LoginAcknowledged = VarIntToBytes(0x03);
    }

    static byte[] VarIntToBytes(int val)
    {
        var ms = new MemoryStream();
        ms.WriteVarInt(val);
        return ms.ToArray();
    }

    public event Action<MinecraftClientState> StateChanged;


    public async Task<LoginizationResult> Login(Stream source, LoginOptions options,
        CancellationToken cancellationToken = default)
    {
        var mainStream = new AesStream(source);
        try
        {
            var sender = new MinecraftPacketSender();
            var reader = new MinecraftPacketReader();

            sender.BaseStream = mainStream;
            reader.BaseStream = mainStream;
            StateChanged?.Invoke(MinecraftClientState.Handshaking);
            var handshake = CreateHandshake(options.Host, options.Port, options.ProtocolVersion);
            try
            {
                await sender.SendPacketAsync(handshake, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                handshake.Dispose();
            }


            StateChanged?.Invoke(MinecraftClientState.Login);

            var loginStart = CreateLoginStart(options);
            try
            {
                await sender.SendPacketAsync(loginStart, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                loginStart.Dispose();
            }


            var threshold = 0;

            bool configState = false;

            while (true)
            {
                var inputPacket = await reader.ReadNextPacketAsync(cancellationToken).ConfigureAwait(false);

                try
                {
                    var needBreak = false;
                    switch (inputPacket.Id)
                    {
                        case 0x00: // Login Disconnect
                            //inputPacket.Data.TryReadString(out var reason, out _);
                            string reason = ReadLoginDisconnect(inputPacket);


                            throw new LoginRejectedException("Login Disconnect: " + reason);
                            break;
                        case 0x01: // Encryption Request
                            var encryptBegin = ReadEncryptionPacket(inputPacket, options.ProtocolVersion);

                            var RSAService = CryptoHandler.DecodeRSAPublicKey(encryptBegin.PublicKey);
                            var secretKey = CryptoHandler.GenerateAESPrivateKey();


                            var sharedSecret = RSAService.Encrypt(secretKey, false);
                            var verifyToken = RSAService.Encrypt(encryptBegin.VerifyToken, false);

                            using (var response = CreateEncryptionResponse(sharedSecret, verifyToken))
                            {
                                await sender.SendPacketAsync(response, cancellationToken);
                            }

                            mainStream.SwitchEncryption(secretKey);

                            break;
                        case 0x02:
                            needBreak = true;

                            if (options.ProtocolVersion >= 764)
                            {
                                await sender.SendPacketAsync(LoginAcknowledged, cancellationToken);
                                configState = true;
                            }

                            break;
                        case 0x03: //Compress

                            //if (!inputPacket.Data.TryReadVarInt(out threshold, out _))


                            threshold = ReadTreshold(inputPacket);
                            reader.SwitchCompression(threshold);
                            sender.SwitchCompression(threshold);

                            break;
                        case 0x04: //Login Plugin Request

                            var buffer = inputPacket.Data;
                            //buffer.TryReadVarInt(out var messageId, out var offset);
                            // buffer = buffer.Slice(offset);
                            // buffer.TryReadString(out var channel, out offset);
                            //var data = buffer.Slice(offset);
                            break;

                        default: throw new Exception("Unknown packet: " + inputPacket.Id);
                    }

                    if (needBreak)
                        break;
                }
                finally
                {
                    inputPacket.Dispose();
                }
            }

            // >= 764
            if (configState)
            {
                var info = CreateDefaultClientInformation(options.ProtocolVersion);
                await sender.SendAndDisposeAsync(info, cancellationToken).ConfigureAwait(false);
                while (true)
                {
                    var inputPacket = await reader.ReadNextPacketAsync(cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var needBreak = false;


                        // int cookieRequest = options.ProtocolVersion switch
                        // {
                        // };
                        // int pluginMessage = options.ProtocolVersion switch
                        // {
                        // };
                        int finishConfig = options.ProtocolVersion switch
                        {
                            <= 765 => 0x02,
                            <= 767 => 0x03,
                        };
                        int keepAlive = options.ProtocolVersion switch
                        {
                            765 => 0x03,
                            >= 766 and <= 767 => 0x04
                        };
                        int ping = options.ProtocolVersion switch
                        {
                            765 => 0x04,
                            >= 766 and <= 767 => 0x05
                        };
                        int knownPacks = options.ProtocolVersion switch
                        {
                            >= 340 and <= 765 => -1,
                            >= 766 and <= 767 => 0x0E,
                        };
                        int disconnect = options.ProtocolVersion switch
                        {
                            >= 340 and <= 765 => 0x01,
                            >= 766 and <= 767 => 0x02,
                        };

                        if (inputPacket.Id == finishConfig)
                        {
                            int packetId = options.ProtocolVersion switch
                            {
                                >= 764 and <= 765 => 0x02,
                                >= 766 and <= 767 => 0x03
                            };
                            var finishConfigServerbound = CreateFinishConfig(packetId);
                            await sender.SendAndDisposeAsync(finishConfigServerbound, cancellationToken);
                            needBreak = true;
                        }
                        else if (inputPacket.Id == keepAlive)
                        {
                            long id = ReadKeepAlive(inputPacket);
                            OutputPacket response = CreateKeepAlive(options.ProtocolVersion, id);
                            await sender.SendAndDisposeAsync(response, cancellationToken);
                        }
                        else if (inputPacket.Id == ping)
                        {
                            await sender.SendAndDisposeAsync(
                                PingPong(options.ProtocolVersion, inputPacket),
                                cancellationToken);
                        }
                        else if (inputPacket.Id == knownPacks)
                        {
                            await sender.SendAndDisposeAsync(CreateZeroKnownPacks(options.ProtocolVersion),
                                cancellationToken);
                        }
                        else if (inputPacket.Id == disconnect)
                        {
                            ThrowConfigDisconnect(options.ProtocolVersion, inputPacket);
                        }


                        if (options.ProtocolVersion >= 765)
                        {
                            int pushResourcePack = options.ProtocolVersion switch
                            {
                                765 => 0x07,
                                >= 766 and <= 767 => 0x09,
                            };
                            if (inputPacket.Id == pushResourcePack)
                            {
                                var resourcePackPacket = ReadPushResourcePack(inputPacket);

                                OutputPacket responseRP =
                                    CreateResourcePack(options.ProtocolVersion, 0, resourcePackPacket.UUID);

                                await sender.SendAndDisposeAsync(responseRP, cancellationToken);
                            }
                        }
                        else
                        {
                            int resourcePack = options.ProtocolVersion switch
                            {
                                >= 340 and <= 764 => 0x06
                            };
                            if (inputPacket.Id == resourcePack)
                            {
                                var resourcePackPacket = ReadResourcePack(inputPacket);

                                OutputPacket responseRP =
                                    CreateResourcePack(options.ProtocolVersion, 0, resourcePackPacket.UUID);

                                await sender.SendAndDisposeAsync(responseRP, cancellationToken);
                            }
                        }


                        if (needBreak)
                            break;
                    }
                    finally
                    {
                        inputPacket.Dispose();
                    }
                }
            }


            return new LoginizationResult(mainStream, threshold);
        }
        catch (Exception e)
        {
            mainStream.Dispose();
            throw;
        }
    }

    private static OutputPacket CreateDefaultClientInformation(int protocolVersion)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = protocolVersion switch
            {
                >= 340 and <= 767 => 0x00,
            };
            writer.WriteVarInt(packetId);
            writer.WriteString("ru_ru");
            writer.WriteSignedByte(16);
            writer.WriteVarInt(0);
            writer.WriteBoolean(true);
            writer.WriteUnsignedByte(127);
            writer.WriteVarInt(0);
            writer.WriteBoolean(true);
            writer.WriteBoolean(true);

            return new OutputPacket(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static OutputPacket CreateZeroKnownPacks(int protocolVersion)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = protocolVersion switch
            {
                >= 766 and <= 767 => 0x07,
            };
            writer.WriteVarInt(packetId);
            writer.WriteVarInt(0);
            return new OutputPacket(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static OutputPacket CreateFinishConfig(int id)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(id);
            return new OutputPacket(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static OutputPacket CreateResourcePack(int protocolVersion, int action, Guid uuid)
    {
        int packetId = protocolVersion switch
        {
            >= 340 and <= 765 => 0x05,
            >= 766 and <= 767 => 0x06
        };

        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(packetId);
            if (protocolVersion < 765)
            {
                writer.WriteVarInt(action);
            }
            else
            {
                writer.WriteUUID(uuid);
                writer.WriteVarInt(action);
            }

            return new OutputPacket(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static ClientBoundResourcePackPacket ReadResourcePack(InputPacket packet)
    {
        scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
        string url = reader.ReadString();
        Guid uuid = Guid.Empty;
        return new ClientBoundResourcePackPacket(uuid, url);
    }

    private static ClientBoundResourcePackPacket ReadPushResourcePack(InputPacket packet)
    {
        scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
        Guid uuid = reader.ReadUUID();
        string url = reader.ReadString();
        return new ClientBoundResourcePackPacket(uuid, url);
    }

    private static string ReadLoginDisconnect(InputPacket packet)
    {
        scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
        return reader.ReadString();
    }

    private static void ThrowConfigDisconnect(int protocolVersion, InputPacket packet)
    {
        scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
        if (protocolVersion < 765)
        {
            throw new ConfigurationDisconnectException(reader.ReadString());
        }
        else
        {
            throw new ConfigurationDisconnectException(reader.ReadNbt(protocolVersion < 764).ToString());
        }
    }

    private static OutputPacket PingPong(int protocolVersion, InputPacket packet)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = protocolVersion switch
            {
                <= 765 => 0x04,
                <= 767 => 0x05
            };
            writer.WriteVarInt(packetId);
            writer.WriteBuffer(packet.Data.Span);
            return new OutputPacket(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static OutputPacket CreateKeepAlive(int protocolVersion, long id)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = protocolVersion switch
            {
                <= 765 => 0x03,
                <= 767 => 0x04,
            };
            writer.WriteVarInt(packetId); // Packet id
            writer.WriteSignedLong(id);
            return new OutputPacket(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static int ReadTreshold(InputPacket p)
    {
        scoped MinecraftPrimitiveSpanReader r = new MinecraftPrimitiveSpanReader(p.Data);
        return r.ReadVarInt();
    }

    private static long ReadKeepAlive(InputPacket p)
    {
        scoped MinecraftPrimitiveSpanReader r = new MinecraftPrimitiveSpanReader(p.Data);
        return r.ReadSignedLong();
    }

    private static ClientboundConfigurationPluginMessagePacket ReadConfigPluginMessagePacket(InputPacket packet)
    {
        scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
        var id = reader.ReadString();
        var data = reader.ReadRestBuffer();

        return new ClientboundConfigurationPluginMessagePacket(id, data);
    }

    private static EncryptionBeginPacket ReadEncryptionPacket(InputPacket inputPacket, int protocolVersion)
    {
        scoped var reader = new MinecraftPrimitiveSpanReader(inputPacket.Data);
        var serverId = reader.ReadString();
        var len = reader.ReadVarInt();
        var publicKey = reader.ReadBuffer(len);
        len = reader.ReadVarInt();
        var verifyToken = reader.ReadBuffer(len);
        bool? shouldAuthenticate = null;
        if (protocolVersion > 765)
        {
            shouldAuthenticate = reader.ReadBoolean();
        }

        return new EncryptionBeginPacket(serverId, publicKey, verifyToken, shouldAuthenticate);
    }

    private static OutputPacket CreatePluginResponse(string channel, byte[] data)
    {
        //scoped var writer = new MinecraftPrimitiveWriterSlim();
        //writer.WriteVarInt(0); // Packet Id
        throw new NotImplementedException();
    }

    private static OutputPacket CreateEncryptionResponse(byte[] sharedSecret, byte[] verifyToken)
    {
        var length = sharedSecret.Length + verifyToken.Length + 4;
        scoped var writer = new BufferWriterSlim<byte>(length, s_allocator);

        try
        {
            writer.WriteVarInt(0x01);
            writer.WriteBuffer(sharedSecret);
            writer.WriteBuffer(verifyToken);


            writer.TryDetachBuffer(out var buffer);
            return new OutputPacket(buffer);
        }
        finally
        {
            writer.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static OutputPacket CreateLoginStart(LoginOptions options)
    {
        if (options.Username.Length > 16) throw new ArgumentOutOfRangeException();

        scoped var writer = new MinecraftPrimitiveSpanWriter();

        try
        {
            FillLoginStartPacket(ref writer, options);
            var buffer = writer.GetWrittenMemory();
            return new OutputPacket(buffer);
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static void FillLoginStartPacket(ref MinecraftPrimitiveSpanWriter spanWriter, LoginOptions options)
    {
        spanWriter.WriteVarInt(0x00); // Packet Id

        //spanWriter.WriteString(options.Username);

        if (options.ProtocolVersion < 759)
        {
            spanWriter.WriteString(options.Username);
        }
        else if (options.ProtocolVersion < 760)
        {
            spanWriter.WriteString(options.Username);
            spanWriter.WriteBoolean(false);
        }
        else if (options.ProtocolVersion < 761)
        {
            spanWriter.WriteString(options.Username);
            spanWriter.WriteBoolean(false);
            spanWriter.WriteBoolean(false);
        }
        else if (options.ProtocolVersion < 764)
        {
            spanWriter.WriteString(options.Username);
            spanWriter.WriteBoolean(false);
        }
        else
        {
            spanWriter.WriteString(options.Username);
            spanWriter.WriteUUID(Guid.NewGuid());
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static OutputPacket CreateHandshake(string host, ushort port, int version)
    {
        if (host.Length > 255) throw new ArgumentOutOfRangeException();

        scoped var writer = new BufferWriterSlim<byte>(10, s_allocator);
        try
        {
            writer.WriteVarInt(0x00); //Packet Id

            writer.WriteVarInt(version);
            writer.WriteString(host);
            writer.WriteBigEndian(port);
            writer.Write(VarIntLoginIntent);


            writer.TryDetachBuffer(out var owner);
            return new OutputPacket(owner);
        }
        finally
        {
            writer.Dispose();
        }
    }

    internal readonly struct EncryptionBeginPacket
    {
        public readonly string ServerId;
        public readonly byte[] PublicKey;
        public readonly byte[] VerifyToken;
        public readonly bool? ShouldAuthenticate;

        public EncryptionBeginPacket(string serverId, byte[] publicKey, byte[] verifyToken, bool? shouldAuthenticate)
        {
            ServerId = serverId;
            PublicKey = publicKey;
            VerifyToken = verifyToken;
            ShouldAuthenticate = shouldAuthenticate;
        }
    }

    internal readonly struct ClientboundConfigurationPluginMessagePacket
    {
        public readonly string Identifier;

        public ClientboundConfigurationPluginMessagePacket(string identifier, byte[] data)
        {
            Identifier = identifier;
            Data = data;
        }

        public readonly byte[] Data;
    }

    internal readonly struct ClientBoundResourcePackPacket
    {
        public readonly Guid UUID;
        public readonly string Url;

        public ClientBoundResourcePackPacket(Guid uuid, string url)
        {
            UUID = uuid;
            Url = url;
        }
    }
}

public class LoginizationResult
{
    public readonly int CompressionThreshold;
    public readonly Stream Stream;

    public LoginizationResult(Stream stream, int compressionThreshold)
    {
        Stream = stream;
        CompressionThreshold = compressionThreshold;
    }
}