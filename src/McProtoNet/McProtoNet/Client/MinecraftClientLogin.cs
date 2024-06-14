using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DotNext.Buffers;
using McProtoNet.Abstractions;
using McProtoNet.Cryptography;
using McProtoNet.Protocol;

namespace McProtoNet.Client;

public sealed class MinecraftClientLogin
{
    private static readonly byte[] VarIntLoginIntent;
    private static readonly byte[] LoginAcknowledged;

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
    }

    public event Action<MinecraftClientState> StateChanged;


    public async Task<LoginizationResult> Login(Stream source, LoginOptions options,
        CancellationToken cancellationToken = default)
    {
        var mainStream = new AesStream(source);

        using var sender = new MinecraftPacketSender();
        using var reader = new MinecraftPacketReader();

        sender.BaseStream = mainStream;
        reader.BaseStream = mainStream;

        using var handshake = CreateHandshake(options.Host, options.Port, options.ProtocolVersion);

        StateChanged?.Invoke(MinecraftClientState.Handshaking);
        await sender.SendPacketAsync(handshake, cancellationToken).ConfigureAwait(false);


        using var loginStart = CreateLoginStart(options.Username, options);

        StateChanged?.Invoke(MinecraftClientState.Login);
        await sender.SendPacketAsync(loginStart, cancellationToken).ConfigureAwait(false);

        var threshold = 0;


        while (true)
        {
            var inputPacket = await reader.ReadNextPacketAsync().ConfigureAwait(false);

            var needBreak = false;

            Console.WriteLine("ReadLoging: " + inputPacket.Id);

            switch (inputPacket.Id)
            {
                case 0x00:
                    inputPacket.Data.TryReadString(out var reason, out _);
                    throw new LoginRejectedException(reason);
                    break;
                case 0x01:
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

                    if (options.ProtocolVersion == 765)
                        await sender.SendPacketAsync(LoginAcknowledged, cancellationToken);

                    break;
                case 0x03:
                    //Compress

                    if (!inputPacket.Data.TryReadVarInt(out threshold, out _))
                        throw new Exception("asd");
                    reader.SwitchCompression(threshold);
                    sender.SwitchCompression(threshold);

                    Debug.WriteLine("Compress: " + threshold);
                    break;
                case 0x04:
                    //Login plugin request
                    Debug.WriteLine("Plugin");
                    var buffer = inputPacket.Data;
                    var offset = 0;
                    buffer.TryReadVarInt(out var messageId, out offset);
                    buffer = buffer.Slice(offset);
                    buffer.TryReadString(out var channel, out offset);
                    var data = buffer.Slice(offset);
                    break;

                default: throw new Exception("Unknown packet: " + inputPacket.Id);
            }

            if (needBreak)
                break;
        }


        return new LoginizationResult(mainStream, threshold);
    }


    private static OutputPacket CreateLoginAcknowledged()
    {
        scoped var writer = new BufferWriterSlim<byte>(1);

        try
        {
            writer.WriteVarInt(0x03);


            writer.TryDetachBuffer(out var buffer);
            return new OutputPacket(buffer);
        }
        finally
        {
            writer.Dispose();
        }
    }

    private static EncryptionBeginPacket ReadEncryptionPacket(InputPacket inputPacket)
    {
        scoped var reader = new SequenceReader<byte>(inputPacket.Data);

        reader.TryReadString(out var serverId);

        reader.TryReadVarInt(out var len, out _);

        var publicKey = reader.UnreadSequence.Slice(0, len).ToArray();
        reader.Advance(len);

        reader.TryReadVarInt(out len, out _);
        var verifyToken = reader.UnreadSequence.Slice(0, len).ToArray();


        return new EncryptionBeginPacket(serverId, publicKey, verifyToken);
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
    private static OutputPacket CreateLoginStart(string name, LoginOptions options)
    {
        if (name.Length > 16) throw new ArgumentOutOfRangeException();

        scoped var writer = new BufferWriterSlim<byte>(10, s_allocator);

        try
        {
            writer.WriteVarInt(0x00); //Packet Id
            writer.WriteString(name);

            if (options.ProtocolVersion == 765)
            {
                var guid = Guid.NewGuid();

                var data = guid.ToByteArray();

                writer.Write(data);
            }

            writer.TryDetachBuffer(out var buffer);
            return new OutputPacket(buffer);
        }
        finally
        {
            writer.Dispose();
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