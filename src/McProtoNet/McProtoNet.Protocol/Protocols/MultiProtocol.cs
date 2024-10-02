using System.Reactive;
using System.Reactive.Subjects;
using McProtoNet.Abstractions;
using McProtoNet.NBT;
using McProtoNet.Serialization;

namespace McProtoNet.MultiVersionProtocol;

public sealed class KeepAlivePacket
{
    public KeepAlivePacket(long keepAliveId)
    {
        KeepAliveId = keepAliveId;
    }

    public long KeepAliveId { get; }
}

[Experimental]
public sealed class MultiProtocol : ProtocolBase
{
    private static readonly byte[] bitset = new byte[3];

    private readonly Subject<KeepAlivePacket> _onKeepAlive = new();

    public MultiProtocol(IPacketBroker client) : base(client)
    {
        //SupportedVersion = 755;
    }

    public Subject<Unit> OnJoinGame { get; } = new();

    protected override void OnPacketReceived(InputPacket packet)
    {
        var keepAlive = ProtocolVersion switch
        {
            340 => 0x1F,
            >= 341 and <= 392 => 0x22,
            >= 393 and <= 404 => 0x21,
            >= 405 and <= 476 => 0x22,
            >= 477 and <= 498 => 0x20,
            >= 499 and <= 734 => 0x21,
            >= 735 and <= 750 => 0x20,
            >= 751 and <= 754 => 0x1F,
            >= 755 and <= 758 => 0x21,
            759 => 0x1E,
            760 => 0x20,
            761 => 0x1F,
            >= 762 and <= 763 => 0x23,
            >= 764 and <= 765 => 0x24,
            >= 766 and <= 767 => 0x26
        };
        var disconnect = ProtocolVersion switch
        {
            340 => 0x1A,
            >= 341 and <= 392 => 0x1C,
            >= 393 and <= 476 => 0x1B,
            >= 477 and <= 498 => 0x1A,
            >= 499 and <= 734 => 0x1B,
            >= 735 and <= 750 => 0x1A,
            >= 751 and <= 754 => 0x19,
            >= 755 and <= 758 => 0x1A,
            759 => 0x17,
            760 => 0x19,
            761 => 0x17,
            >= 762 and <= 763 => 0x1A,
            >= 764 and <= 765 => 0x1B,
            >= 766 and <= 767 => 0x1D
        };
        if (packet.Id == 0x2B)
        {
            OnJoinGame.OnNext(Unit.Default);
            return;
        }

        if (keepAlive == packet.Id)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            //_onKeepAlive.OnNext(new KeepAlivePacket(reader.ReadSignedLong()));
            _ = SendKeepAlive(reader.ReadSignedLong());
        }
        else if (disconnect == packet.Id)
        {
            _client.Stop(new DisconnectException("Play disconnect"));
            if (false)
            {
                scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                if (ProtocolVersion >= 765)
                {
                    // var reason = reader.ReadNbt();
                    var nbtReader = new NbtSpanReader(packet.Data.Span);

                    var nbt = nbtReader.ReadAsTag<NbtTag>(false);
                }
            }
        }

        base.OnPacketReceived(packet);
    }

    public ValueTask SendChatPacket(string message)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            var id = ProtocolVersion switch
            {
                340 => 0x02,
                >= 341 and <= 392 => 0x03,
                >= 393 and <= 404 => 0x02,
                >= 405 and <= 758 => 0x03,
                759 => 0x04,
                >= 760 and <= 765 => 0x05,
                >= 766 and <= 767 => 0x06,
                _ => throw new Exception("Unknown protocol version")
            };
            writer.WriteVarInt(id); // Packet Id
            writer.WriteString(message);
            if (ProtocolVersion >= 759)
            {
                var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                writer.WriteSignedLong(timeStamp);
                writer.WriteSignedLong(0);
                if (ProtocolVersion < 761)
                    writer.WriteVarInt(0);
                else
                    writer.WriteBoolean(false);

                if (ProtocolVersion <= 760)
                    writer.WriteBoolean(false);

                switch (ProtocolVersion)
                {
                    case >= 761:
                        writer.WriteVarInt(0);
                        writer.WriteBuffer(bitset);
                        break;
                    case 760:
                        writer.WriteVarInt(0);
                        writer.WriteBoolean(false);
                        break;
                }
            }

            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }


    public ValueTask SendKeepAlive(long id)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            var packetId = ProtocolVersion switch
            {
                340 => 0x0B,
                >= 341 and <= 392 => 0x0F,
                >= 393 and <= 404 => 0x0E,
                >= 405 and <= 476 => 0x10,
                >= 477 and <= 578 => 0x0F,
                >= 579 and <= 754 => 0x10,
                >= 755 and <= 758 => 0x0F,
                759 => 0x11,
                760 => 0x12,
                761 => 0x11,
                >= 762 and <= 763 => 0x12,
                764 => 0x14,
                765 => 0x15,
                >= 766 and <= 767 => 0x18,
                _ => throw new Exception("Unknown protocol version")
            };
            writer.WriteVarInt(packetId); // Packet Id
            writer.WriteSignedLong(id);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public override void Dispose()
    {
        OnJoinGame.Dispose();
        _onKeepAlive.Dispose();
        base.Dispose();
    }
}