using System.Reactive;
using System.Reactive.Subjects;
using McProtoNet.Abstractions;
using McProtoNet.NBT;
using McProtoNet.Protocol;
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

public sealed class LoginPacket
{
    public int Id { get; }

    public LoginPacket(int id)
    {
        Id = id;
    }
}

public sealed class MapItemDataPacket
{
    public int MapId { get; }
    public sbyte Scale { get; }
    public bool Locked { get; }

    public MapIcon[] Icons { get; }
    public MapData? Data { get; }

    public MapItemDataPacket(int mapId, sbyte scale, bool locked, MapIcon[] icons, MapData? data)
    {
        MapId = mapId;
        Scale = scale;
        Locked = locked;
        Icons = icons;
        Data = data;
    }
}

public sealed class MapIcon
{
    public int Type { get; }
    public sbyte X { get; }
    public sbyte Z { get; }
    public sbyte Direction { get; }
    public NbtTag? DisplayName { get; }

    public MapIcon(int type, sbyte x, sbyte z, sbyte direction, NbtTag? displayName)
    {
        Type = type;
        X = x;
        Z = z;
        Direction = direction;
        DisplayName = displayName;
    }
}

public sealed class MapData
{
    public byte Columns { get; }
    public byte Rows { get; }
    public byte X { get; }
    public byte Z { get; }
    public byte[] Data { get; }

    public MapData(byte columns, byte rows, byte x, byte z, byte[] data)
    {
        Columns = columns;
        Rows = rows;
        X = x;
        Z = z;
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}

#region Entity

public class EntityMovePacket
{
    public EntityMovePacket(int entityId, short dX, short dY, short dZ, bool onGround)
    {
        EntityId = entityId;
        DeltaX = dX;
        DeltaY = dY;
        DeltaZ = dZ;
        OnGround = onGround;
    }

    public int EntityId { get; internal set; }
    public short DeltaX { get; internal set; }
    public short DeltaY { get; internal set; }
    public short DeltaZ { get; internal set; }
    public bool OnGround { get; internal set; }
}

public class EntityMoveLookPacket
{
    public EntityMoveLookPacket(int entityId, short dX, short dY, short dZ, sbyte yaw, sbyte pitch, bool onGround)
    {
        EntityId = entityId;
        DeltaX = dX;
        DeltaY = dY;
        DeltaZ = dZ;
        Yaw = yaw;
        Pitch = pitch;
        OnGround = onGround;
    }

    public int EntityId { get; internal set; }
    public short DeltaX { get; internal set; }
    public short DeltaY { get; internal set; }
    public short DeltaZ { get; internal set; }
    public sbyte Yaw { get; internal set; }
    public sbyte Pitch { get; internal set; }
    public bool OnGround { get; internal set; }
}

public class EntityTeleportPacket
{
    public EntityTeleportPacket(int entityId, double x, double y, double z, sbyte yaw, sbyte pitch, bool onGround)
    {
        EntityId = entityId;
        X = x;
        Y = y;
        Z = z;
        Yaw = yaw;
        Pitch = pitch;
        OnGround = onGround;
    }

    public int EntityId { get; internal set; }
    public double X { get; internal set; }
    public double Y { get; internal set; }
    public double Z { get; internal set; }
    public sbyte Yaw { get; internal set; }
    public sbyte Pitch { get; internal set; }
    public bool OnGround { get; internal set; }
}

#endregion

#region PlayerInfoUpdate

public class PlayerInfoUpdateEntry
{
    public GameProfile? Profile { get; set; }
    public int GameMode { get; set; }
    public int Latency { get; set; }
}

public class GameProfileProperty
{
    public string Name { get; }
    public string Value { get; }
    public string? Signature { get; }

    public GameProfileProperty(string name, string value, string? signature)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Signature = signature;
    }
}

public class GameProfile
{
    public string Username { get; }
    public GameProfileProperty[] Properties { get; }

    public GameProfile(string username, GameProfileProperty[] properties)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Properties = properties ?? throw new ArgumentNullException(nameof(properties));
    }
}

public enum PlayerInfoUpdateActions : sbyte
{
    AddPlayer = 0x01,
    InitializeChat = 0x02,
    UpdateGameMode = 0x04,
    UpdateListed = 0x08,
    UpdateLatency = 0x10,
    UpdateDisplayName = 0x20
}

public class PlayerInfoUpdatePacket
{
    public PlayerInfoUpdateActions Actions { get; }
    public Dictionary<Guid, PlayerInfoUpdateEntry> Entries { get; }

    public PlayerInfoUpdatePacket(PlayerInfoUpdateActions actions, Dictionary<Guid, PlayerInfoUpdateEntry> entries)
    {
        Actions = actions;
        Entries = entries;
    }
}

public class PositionPacket
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public float Yaw { get; }
    public float Pitch { get; }
    public sbyte Flags { get; }
    public int TeleportId { get; }

    public PositionPacket(double x, double y, double z, float yaw, float pitch, sbyte flags, int teleportId)
    {
        X = x;
        Y = y;
        Z = z;
        Yaw = yaw;
        Pitch = pitch;
        Flags = flags;
        TeleportId = teleportId;
    }
}

#endregion

public class SpawnEntityPacket
{
    public SpawnEntityPacket(int entityId, Guid objectUUID, int type, double x, double y, double z, sbyte pitch,
        sbyte yaw, sbyte headPitch, int objectData, short velocityX, short velocityY, short velocityZ)
    {
        EntityId = entityId;
        ObjectUUID = objectUUID;
        Type = type;
        X = x;
        Y = y;
        Z = z;
        Pitch = pitch;
        Yaw = yaw;
        HeadPitch = headPitch;
        ObjectData = objectData;
        VelocityX = velocityX;
        VelocityY = velocityY;
        VelocityZ = velocityZ;
    }

    public int EntityId { get; internal set; }
    public Guid ObjectUUID { get; internal set; }
    public int Type { get; internal set; }
    public double X { get; internal set; }
    public double Y { get; internal set; }
    public double Z { get; internal set; }
    public sbyte Pitch { get; internal set; }
    public sbyte Yaw { get; internal set; }
    public sbyte HeadPitch { get; internal set; }
    public int ObjectData { get; internal set; }
    public short VelocityX { get; internal set; }
    public short VelocityY { get; internal set; }
    public short VelocityZ { get; internal set; }
}

[Experimental]
public sealed class MultiProtocol : ProtocolBase
{
    private static readonly byte[] bitset = new byte[3];

    private readonly Subject<KeepAlivePacket> _onKeepAlive = new();
    private readonly Subject<LoginPacket> _onLoginPacket = new();
    private readonly Subject<SpawnEntityPacket> _onSpawnEntity = new();
    private readonly Subject<PlayerInfoUpdatePacket> _onPlayerInfoUpdate = new();
    private readonly Subject<PositionPacket> _onPosition = new();
    private readonly Subject<EntityMovePacket> _onMoveEntity = new();
    private readonly Subject<EntityTeleportPacket> _onEntityTeleport = new();
    private readonly Subject<EntityMoveLookPacket> _onEntityMoveLook = new();
    private readonly Subject<MapItemDataPacket> _onMapItemData = new();

    public MultiProtocol(IPacketBroker client) : base(client)
    {
        //SupportedVersion = 755;
    }

    public IObservable<LoginPacket> OnLogin => _onLoginPacket;
    public IObservable<SpawnEntityPacket> OnSpawnEntity => _onSpawnEntity;
    public IObservable<PlayerInfoUpdatePacket> OnPlayerInfoUpdate => _onPlayerInfoUpdate;
    public IObservable<PositionPacket> OnPosition => _onPosition;

    public IObservable<EntityMovePacket> OnEntityMove => _onMoveEntity;
    public IObservable<EntityTeleportPacket> OnEntityTeleport => _onEntityTeleport;
    public IObservable<EntityMoveLookPacket> OnEntityMoveLook => _onEntityMoveLook;
    public IObservable<MapItemDataPacket> OnMapItemData => _onMapItemData;

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
        int loginPlay = ProtocolVersion switch
        {
            >= 751 and <= 754 => 0x24,
            >= 755 and <= 758 => 0x26,
            759 => 0x23,
            760 => 0x25,
            761 => 0x24,
            >= 762 and <= 763 => 0x28,
            >= 764 and <= 765 => 0x29,
            >= 766 and <= 767 => 0x2B
        };
        int spawnEntity = ProtocolVersion switch
        {
            >= 340 and <= 761 => 0x00,
            >= 762 and <= 767 => 0x01
        };


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
                    // var reason = reader.ReadNbt(true);
                    var nbtReader = new NbtSpanReader(packet.Data.Span);

                    var nbt = nbtReader.ReadAsTag<NbtTag>(false);
                }
            }
        }
        else if (packet.Id == loginPlay)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            int id = reader.ReadSignedInt();
            _onLoginPacket.OnNext(new LoginPacket(id));
            if (ProtocolVersion < 477)
            {
            }
            else if (ProtocolVersion < 573)
            {
            }
            else if (ProtocolVersion < 735)
            {
            }
            else if (ProtocolVersion < 751)
            {
            }
            else if (ProtocolVersion < 757)
            {
            }
            else if (ProtocolVersion < 759)
            {
            }
            else if (ProtocolVersion < 763)
            {
            }
            else if (ProtocolVersion < 764)
            {
            }
            else if (ProtocolVersion < 766)
            {
            }
            else
            {
            }
        }
        else if (packet.Id == spawnEntity)
        {
            return;
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            var entityId = reader.ReadVarInt();
            var objectUUID = reader.ReadUUID();

            int type = 0;
            if (ProtocolVersion < 477)
            {
                type = reader.ReadUnsignedByte();
            }
            else
            {
                type = reader.ReadVarInt();
            }

            var x = reader.ReadDouble();
            var y = reader.ReadDouble();
            var z = reader.ReadDouble();
            var pitch = reader.ReadSignedByte();
            var yaw = reader.ReadSignedByte();
            sbyte headPitch = 0;

            if (ProtocolVersion < 759)
            {
                headPitch = reader.ReadSignedByte();
            }

            int objectData = 0;

            objectData = ProtocolVersion < 759 ? reader.ReadSignedInt() : reader.ReadVarInt();


            var velocityX = reader.ReadSignedShort();
            var velocityY = reader.ReadSignedShort();
            var velocityZ = reader.ReadSignedShort();
            _onSpawnEntity.OnNext(new SpawnEntityPacket(entityId, objectUUID, type, x, y, z, pitch, yaw,
                headPitch, objectData, velocityX, velocityY, velocityZ));
        }

        if (ProtocolVersion != 767)
        {
            return;
        }

        //Experimental
        if (packet.Id == 0x3E)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);

            PlayerInfoUpdateActions actions =
                (PlayerInfoUpdateActions)reader.ReadSignedByte();
            int length = reader.ReadVarInt();
            PlayerInfoUpdateActions[] allActions =
                Enum.GetValues<PlayerInfoUpdateActions>();

            Dictionary<Guid, PlayerInfoUpdateEntry> entries = new Dictionary<Guid, PlayerInfoUpdateEntry>(length);

            for (int i = 0; i < length; i++)
            {
                Guid uuid = reader.ReadUUID();
                PlayerInfoUpdateEntry entry = new PlayerInfoUpdateEntry();

                if (actions.HasFlag(PlayerInfoUpdateActions.AddPlayer))
                {
                    string username = reader.ReadString();

                    int numberOfProperties = reader.ReadVarInt();

                    GameProfileProperty[] props = numberOfProperties <= 0
                        ? []
                        : new GameProfileProperty[numberOfProperties];
                    for (int j = 0; j < numberOfProperties; j++)
                    {
                        string name = reader.ReadString();
                        string value = reader.ReadString();

                        string? signature = reader.ReadBoolean() ? reader.ReadString() : null;
                        props[j] = new GameProfileProperty(name, value, signature);
                    }

                    GameProfile profile = new GameProfile(username, props);
                    entry.Profile = profile;
                }

                if (actions.HasFlag(PlayerInfoUpdateActions.InitializeChat))
                {
                    if (reader.ReadBoolean())
                    {
                        reader.ReadUUID();
                        reader.ReadSignedLong();

                        reader.Advance(reader.ReadVarInt());
                        reader.Advance(reader.ReadVarInt());
                    }
                }

                if (actions.HasFlag(PlayerInfoUpdateActions.UpdateGameMode))
                {
                    entry.GameMode = reader.ReadVarInt();
                }

                if (actions.HasFlag(PlayerInfoUpdateActions.UpdateListed))
                {
                    reader.ReadBoolean();
                }

                if (actions.HasFlag(PlayerInfoUpdateActions.UpdateLatency))
                {
                    reader.ReadVarInt();
                }

                if (actions.HasFlag(PlayerInfoUpdateActions.UpdateDisplayName))
                {
                    reader.ReadOptionalNbt(false);
                }

                entries[uuid] = entry;
            }

            _onPlayerInfoUpdate.OnNext(new PlayerInfoUpdatePacket(actions, entries));
        }
        else if (packet.Id == 0x40)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            var x = reader.ReadDouble();
            var y = reader.ReadDouble();
            var z = reader.ReadDouble();
            var yaw = reader.ReadFloat();
            var pitch = reader.ReadFloat();
            var flags = reader.ReadSignedByte();
            var teleportId = reader.ReadVarInt();
            _onPosition.OnNext(new PositionPacket(x, y, z, yaw, pitch, flags, teleportId));
        }
        else if (packet.Id == 0x2E)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            var entityId = reader.ReadVarInt();
            var dX = reader.ReadSignedShort();
            var dY = reader.ReadSignedShort();
            var dZ = reader.ReadSignedShort();
            var onGround = reader.ReadBoolean();
            _onMoveEntity.OnNext(new EntityMovePacket(entityId, dX, dY, dZ, onGround));
        }
        else if (packet.Id == 0x2F)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            var entityId = reader.ReadVarInt();
            var dX = reader.ReadSignedShort();
            var dY = reader.ReadSignedShort();
            var dZ = reader.ReadSignedShort();
            var yaw = reader.ReadSignedByte();
            var pitch = reader.ReadSignedByte();
            var onGround = reader.ReadBoolean();
            _onEntityMoveLook.OnNext(new EntityMoveLookPacket(entityId, dX, dY, dZ, yaw, pitch, onGround));
        }
        else if (packet.Id == 0x70)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
            var entityId = reader.ReadVarInt();
            var x = reader.ReadDouble();
            var y = reader.ReadDouble();
            var z = reader.ReadDouble();
            var yaw = reader.ReadSignedByte();
            var pitch = reader.ReadSignedByte();
            var onGround = reader.ReadBoolean();
            _onEntityTeleport.OnNext(new EntityTeleportPacket(entityId, x, y, z, yaw, pitch, onGround));
        }
        else if (packet.Id == 0x2C)
        {
            scoped var reader = new MinecraftPrimitiveSpanReader();
            int mapId = reader.ReadVarInt();
            sbyte scale = reader.ReadSignedByte();
            bool locked = reader.ReadBoolean();
            bool hasIcons = reader.ReadBoolean();
            MapIcon[] icons = Array.Empty<MapIcon>();
            if (hasIcons)
            {
                int iconCount = reader.ReadVarInt();
                icons = new MapIcon[iconCount];
                for (int i = 0; i < iconCount; i++)
                {
                    int type = reader.ReadVarInt();
                    sbyte x = reader.ReadSignedByte();
                    sbyte z = reader.ReadSignedByte();
                    sbyte direction = reader.ReadSignedByte();
                    NbtTag? displayName = reader.ReadOptionalNbt(false);
                    icons[i] = new MapIcon(type, x, z, direction, displayName);
                }
            }

            MapData? data = null;
            byte columns = reader.ReadUnsignedByte();
            if (columns > 0)
            {
                byte rows = reader.ReadUnsignedByte();
                byte x = reader.ReadUnsignedByte();
                byte z = reader.ReadUnsignedByte();
                byte[] buffer = reader.ReadBuffer(reader.ReadVarInt());
                data = new MapData(columns, rows, x, z, buffer);
            }

            MapItemDataPacket mapPacket = new MapItemDataPacket(mapId, scale, locked, icons, data);
            _onMapItemData.OnNext(mapPacket);
        }

        base.OnPacketReceived(packet);
    }

    public ValueTask SendClientInformation(string locale, sbyte viewDistance, int chatMode, bool chatColors, byte skin,
        int mainHand, bool enableTextFiltering, bool allowServerListings)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x0A);
            writer.WriteString(locale);
            writer.WriteSignedByte(viewDistance);
            writer.WriteVarInt(chatMode);
            writer.WriteBoolean(chatColors);
            writer.WriteUnsignedByte(skin);
            writer.WriteVarInt(mainHand);
            writer.WriteBoolean(enableTextFiltering);
            writer.WriteBoolean(allowServerListings);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendPosition(double x, double y, double z, bool onGround)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = ProtocolVersion switch
            {
                340 => 0x0D,
                >= 341 and <= 392 => 0x11,
                >= 393 and <= 404 => 0x10,
                >= 405 and <= 476 => 0x13,
                >= 477 and <= 578 => 0x11,
                >= 579 and <= 754 => 0x12,
                >= 755 and <= 758 => 0x11,
                759 => 0x13,
                760 => 0x14,
                761 => 0x13,
                >= 762 and <= 763 => 0x14,
                764 => 0x16,
                765 => 0x17,
                >= 766 and <= 767 => 0x1A
            };
            writer.WriteVarInt(packetId);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteBoolean(onGround);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendTeleportConfirm(int id)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x00);
            writer.WriteVarInt(id);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = ProtocolVersion switch
            {
                340 => 0x0E,
                >= 341 and <= 392 => 0x12,
                >= 393 and <= 404 => 0x11,
                >= 405 and <= 476 => 0x14,
                >= 477 and <= 578 => 0x12,
                >= 579 and <= 754 => 0x13,
                >= 755 and <= 758 => 0x12,
                759 => 0x14,
                760 => 0x15,
                761 => 0x14,
                >= 762 and <= 763 => 0x15,
                764 => 0x17,
                765 => 0x18,
                >= 766 and <= 767 => 0x1B
            };
            writer.WriteVarInt(packetId);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            writer.WriteBoolean(onGround);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendLook(float yaw, float pitch, bool onGround)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            int packetId = ProtocolVersion switch
            {
                340 => 0x0F,
                >= 341 and <= 392 => 0x13,
                >= 393 and <= 404 => 0x12,
                >= 405 and <= 476 => 0x15,
                >= 477 and <= 578 => 0x13,
                >= 579 and <= 754 => 0x14,
                >= 755 and <= 758 => 0x13,
                759 => 0x15,
                760 => 0x16,
                761 => 0x15,
                >= 762 and <= 763 => 0x16,
                764 => 0x18,
                765 => 0x19,
                >= 766 and <= 767 => 0x1C
            };
            writer.WriteVarInt(packetId);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            writer.WriteBoolean(onGround);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
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
        finally
        {
            writer.Dispose();
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
        finally
        {
            writer.Dispose();
        }
    }

    public ValueTask SendPlayerAction(int status, Position location, byte face, int sequence)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            var packetId = ProtocolVersion switch
            {
                767 => 0x24,
                _ => throw new NotImplementedException("Unsupported protocol")
            };
            writer.WriteVarInt(packetId); // Packet Id
            writer.WriteVarInt(status);
            writer.WritePosition(location);
            writer.WriteUnsignedByte(face);
            writer.WriteVarInt(sequence);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    public ValueTask SendUseItem(int hand, int sequence, float yaw, float pitch)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            var packetId = ProtocolVersion switch
            {
                767 => 0x39,
                _ => throw new NotImplementedException("Unsupported protocol")
            };
            writer.WriteVarInt(packetId); // Packet Id
            writer.WriteVarInt(hand);
            writer.WriteVarInt(sequence);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        finally
        {
            writer.Dispose();
        }
    }

    public override void Dispose()
    {
        _onLoginPacket.Dispose();
        _onKeepAlive.Dispose();
        base.Dispose();
    }
}