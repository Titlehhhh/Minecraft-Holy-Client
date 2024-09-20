﻿using System.Buffers;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Abstractions;
using McProtoNet.Cryptography;
using McProtoNet.Protocol;
using McProtoNet.Serialization;

namespace McProtoNet.Client;

public sealed class MinecraftClientLogin
{
    private static readonly byte[] VarIntLoginIntent;
    private static readonly byte[] LoginAcknowledged;
    private static readonly byte[] KnownPacksZero;

    private static readonly MemoryAllocator<byte> s_allocator = ArrayPool<byte>.Shared.ToAllocator();

    static MinecraftClientLogin()
    {
        var ms = new MemoryStream();
        ms.WriteVarInt(2);
        VarIntLoginIntent = ms.ToArray();
        ms.Position = 0;
        ms.SetLength(0);

        ms.WriteVarInt(0x03);
        LoginAcknowledged = ms.ToArray();

        ms.Position = 0;
        ms.SetLength(0);

        ms.WriteVarInt(0x07); //Packet id
        ms.WriteVarInt(0x00);
        KnownPacksZero = ms.ToArray();
    }

    public event Action<MinecraftClientState> StateChanged;


    public async Task<LoginizationResult> Login(Stream source, LoginOptions options,
        CancellationToken cancellationToken = default)
    {
        var mainStream = new AesStream(source);
        try
        {
            using var sender = new MinecraftPacketSender();
            using var reader = new MinecraftPacketReader();

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
                scoped MinecraftPrimitiveReaderSlim p_reader =
                    new MinecraftPrimitiveReaderSlim(inputPacket.Data);
                try
                {
                    var needBreak = false;


                    switch (inputPacket.Id)
                    {
                        case 0x00: // Login Disconnect
                            //inputPacket.Data.TryReadString(out var reason, out _);
                            throw new LoginRejectedException("Login Disconnect");
                            break;
                        case 0x01: // Encryption Request
                            var encryptBegin = ReadEncryptionPacket(inputPacket);

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


                            threshold = p_reader.ReadVarInt();
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

            if (configState)
            {
                while (true)
                {
                    var inputPacket = await reader.ReadNextPacketAsync(cancellationToken).ConfigureAwait(false);
                    scoped var p_reader = new MinecraftPrimitiveReaderSlim(inputPacket.Data);
                    try
                    {
                        var needBreak = false;


                        switch (inputPacket.Id)
                        {
                            case 0x00: // Cookie Request
                                //if (!inputPacket.Data.TryReadString(out string key, out _))
                            {
                                //   throw new Exception("Failed Read");
                            }


                                break;
                            case 0x01: // Plugin Message
                                //var pluginPacket = ReadConfigPluginMessagePacket(inputPacket);

                                break;
                            case 0x02: // Disconnect (configuration)
                                // inputPacket.Data.TryReadString(out var reason, out _);
                                throw new LoginRejectedException("Login Disconnect");
                                // throw new LoginRejectedException(reason);
                                break;
                            case 0x03:
                                await sender.SendPacketAsync(LoginAcknowledged, cancellationToken);
                                needBreak = true;
                                break; // Finish
                            case 0x04: // KeepAlive

                                long id = p_reader.ReadSignedLong();

                                var outP = CreateKeepAlive(id);
                                try
                                {
                                    await sender.SendPacketAsync(outP, cancellationToken);
                                }
                                finally
                                {
                                    outP.Dispose();
                                }

                                break;
                            case 0x05: // Ping
                                break;
                            case 0x06: // Reset Chat
                                break;
                            case 0x07: // Registry Data
                                break;
                            case 0x08: // Remove Resource Pack
                                break;
                            case 0x09: // Add Resource Pack
                                break;
                            case 0x0A: // Store Cookie
                                break;
                            case 0x0B: // Transfer
                                break;
                            case 0x0C: // Feature Flags
                                break;
                            case 0x0D: // Update Tags
                                break;

                            case 0x0E: // Clientbound Known Packs
                                await sender.SendPacketAsync(KnownPacksZero, cancellationToken);
                                break;

                            case 0x0F: // Custom Report Details
                                break;

                            case 0x10: // Server Links
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
            }


            return new LoginizationResult(mainStream, threshold);
        }
        catch (Exception e)
        {
            mainStream.Dispose();
            throw;
        }
    }

    private static OutputPacket CreateKeepAlive(long id)
    {
        scoped var writer = new MinecraftPrimitiveWriterSlim();
        writer.WriteVarInt(0x04); // Packet id
        writer.WriteSignedLong(id);
        return new OutputPacket(writer.GetWrittenMemory());
    }


    private static ClientboundConfigurationPluginMessagePacket ReadConfigPluginMessagePacket(InputPacket packet)
    {
        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
        var id = reader.ReadString();
        var data = reader.ReadRestBuffer();

        return new ClientboundConfigurationPluginMessagePacket(id, data);
    }

    private static EncryptionBeginPacket ReadEncryptionPacket(InputPacket inputPacket)
    {
        scoped var reader = new MinecraftPrimitiveReaderSlim(inputPacket.Data);
        var serverId = reader.ReadString();
        var len = reader.ReadVarInt();
        var publicKey = reader.ReadBuffer(len);
        var verifyToken = reader.ReadBuffer(len);
        return new EncryptionBeginPacket(serverId, publicKey, verifyToken);
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

        scoped var writer = new MinecraftPrimitiveWriterSlim();

        try
        {
            FillLoginStartPacket(ref writer, options);
            var buffer = writer.GetWrittenMemory();
            return new OutputPacket(buffer);
        }
        finally
        {
            writer.Clear();
        }
    }

    private static void FillLoginStartPacket(ref MinecraftPrimitiveWriterSlim writer, LoginOptions options)
    {
        writer.WriteVarInt(0x00); // Packet Id

        writer.WriteString(options.Username);

        bool authentifier = false; // TODO 

        if (authentifier)
        {
            if (options.ProtocolVersion < 761) /* < 1.19.3 */
            {
                // ProfilePublicKey key;
                // key.SetTimestamp(authentifier->GetKeyTimestamp());
                // key.SetKey(Utilities::RSAToBytes(authentifier->GetPublicKey()));
                // key.SetSignature(Utilities::DecodeBase64(authentifier->GetKeySignature()));
                //
                // loginstart_msg->SetPublicKey(key);
            }
            else
            {
                // message_sent_index = 0;
            }


            if (options.ProtocolVersion > 759) /* > 1.19 */
            {
                //loginstart_msg->SetProfileId(authentifier->GetPlayerUUID());
            }
        }
        else
        {
            if (options.ProtocolVersion < 760) /* < 1.19.1 */
            {
                writer.WriteBoolean(false);
                //     DECLARE_FIELDS(
                //         (std::string, std::optional<ProfilePublicKey>),
                // (Name_, PublicKey)
                //     );
            }
            else if (options.ProtocolVersion < 761) /* < 1.19.3 */
            {
                writer.WriteBoolean(false);
                writer.WriteBoolean(false);
                // DECLARE_FIELDS(
                //     (std::string, std::optional<ProfilePublicKey>, std::optional<UUID>),
                // (Name_, PublicKey, ProfileId)
                //     );
            }
            else if (options.ProtocolVersion < 764) /* < 1.20.2 */
            {
                writer.WriteBoolean(false);
                // DECLARE_FIELDS(
                //     (std::string, std::optional<UUID>),
                // (Name_, ProfileId)
                //     );
            }
            else
            {
                writer.WriteUUID(Guid.NewGuid());
                // DECLARE_FIELDS(
                //     (std::string, UUID),
                // (Name_, ProfileId)
                //     );
            }
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

        public EncryptionBeginPacket(string serverId, byte[] publicKey, byte[] verifyToken)
        {
            ServerId = serverId;
            PublicKey = publicKey;
            VerifyToken = verifyToken;
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