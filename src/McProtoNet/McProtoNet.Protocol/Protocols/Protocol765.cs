using McProtoNet.Serialization;
using McProtoNet.Protocol;
using McProtoNet.Abstractions;

namespace McProtoNet.Protocol765
{
    public sealed class Protocol_765 : ProtocolBase
    {
        public Protocol_765(IPacketBroker client) : base(client)
        {
        }


        public Task SendTeleportConfirm(int teleportId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x00);
            writer.WriteVarInt(teleportId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendChatMessage(string message, long timestamp, long salt, byte[]? signature, int offset,
            byte[] acknowledged)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x05);
            writer.WriteString(message);
            writer.WriteSignedLong(timestamp);
            writer.WriteSignedLong(salt);
            if (signature is null)
            {
                writer.WriteBoolean(false);
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteVarInt(256);
                writer.WriteBuffer(signature);
            }

            writer.WriteVarInt(offset);
            writer.WriteVarInt(3);
            writer.WriteBuffer(acknowledged);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetDifficulty(byte newDifficulty)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x02);
            writer.WriteUnsignedByte(newDifficulty);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendMessageAcknowledgement(int count)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x03);
            writer.WriteVarInt(count);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEditBook(int hand, string[] pages, string? title)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x11);
            writer.WriteVarInt(hand);
            writer.WriteVarInt(pages.Length);
            for (int i_0 = 0; i_0 < pages.Length; i_0++)
            {
                var value_0 = pages[i_0];
                writer.WriteString(value_0);
            }

            if (title is null)
            {
                writer.WriteBoolean(false);
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteString(title);
            }

            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendQueryEntityNbt(int transactionId, int entityId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x12);
            writer.WriteVarInt(transactionId);
            writer.WriteVarInt(entityId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPickItem(int slot)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1d);
            writer.WriteVarInt(slot);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendNameItem(string name)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x27);
            writer.WriteString(name);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSelectTrade(int slot)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2a);
            writer.WriteVarInt(slot);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetBeaconEffect(int? primary_effect, int? secondary_effect)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2b);
            if (primary_effect is null)
            {
                writer.WriteBoolean(false);
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteVarInt(primary_effect);
            }

            if (secondary_effect is null)
            {
                writer.WriteBoolean(false);
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteVarInt(secondary_effect);
            }

            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUpdateCommandBlockMinecart(int entityId, string command, bool track_output)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2e);
            writer.WriteVarInt(entityId);
            writer.WriteString(command);
            writer.WriteBoolean(track_output);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendTabComplete(int transactionId, string text)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0a);
            writer.WriteVarInt(transactionId);
            writer.WriteString(text);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendClientCommand(int actionId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x08);
            writer.WriteVarInt(actionId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSettings(string locale, sbyte viewDistance, int chatFlags, bool chatColors, byte skinParts,
            int mainHand, bool enableTextFiltering, bool enableServerListing)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x09);
            writer.WriteString(locale);
            writer.WriteSignedByte(viewDistance);
            writer.WriteVarInt(chatFlags);
            writer.WriteBoolean(chatColors);
            writer.WriteUnsignedByte(skinParts);
            writer.WriteVarInt(mainHand);
            writer.WriteBoolean(enableTextFiltering);
            writer.WriteBoolean(enableServerListing);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEnchantItem(sbyte windowId, sbyte enchantment)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0c);
            writer.WriteSignedByte(windowId);
            writer.WriteSignedByte(enchantment);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCloseWindow(byte windowId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0e);
            writer.WriteUnsignedByte(windowId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendKeepAlive(long keepAliveId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x15);
            writer.WriteSignedLong(keepAliveId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendLockDifficulty(bool locked)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x16);
            writer.WriteBoolean(locked);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPosition(double x, double y, double z, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x17);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x18);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendLook(float yaw, float pitch, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x19);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendFlying(bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1a);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendVehicleMove(double x, double y, double z, float yaw, float pitch)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1b);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSteerBoat(bool leftPaddle, bool rightPaddle)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1c);
            writer.WriteBoolean(leftPaddle);
            writer.WriteBoolean(rightPaddle);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCraftRecipeRequest(sbyte windowId, string recipe, bool makeAll)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1f);
            writer.WriteSignedByte(windowId);
            writer.WriteString(recipe);
            writer.WriteBoolean(makeAll);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendAbilities(sbyte flags)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x20);
            writer.WriteSignedByte(flags);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEntityAction(int entityId, int actionId, int jumpBoost)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x22);
            writer.WriteVarInt(entityId);
            writer.WriteVarInt(actionId);
            writer.WriteVarInt(jumpBoost);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSteerVehicle(float sideways, float forward, byte jump)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x23);
            writer.WriteFloat(sideways);
            writer.WriteFloat(forward);
            writer.WriteUnsignedByte(jump);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendDisplayedRecipe(string recipeId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x26);
            writer.WriteString(recipeId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendRecipeBook(int bookId, bool bookOpen, bool filterActive)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x25);
            writer.WriteVarInt(bookId);
            writer.WriteBoolean(bookOpen);
            writer.WriteBoolean(filterActive);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendResourcePackReceive(Guid uuid, int result)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x28);
            writer.WriteUUID(uuid);
            writer.WriteVarInt(result);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendHeldItemSlot(short slotId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2c);
            writer.WriteSignedShort(slotId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendArmAnimation(int hand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x33);
            writer.WriteVarInt(hand);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSpectate(Guid target)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x34);
            writer.WriteUUID(target);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUseItem(int hand, int sequence)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x36);
            writer.WriteVarInt(hand);
            writer.WriteVarInt(sequence);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPong(int id)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x24);
            writer.WriteSignedInt(id);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendChatSessionUpdate(Guid sessionUUID, long expireTime, byte[] publicKey, byte[] signature)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x06);
            writer.WriteUUID(sessionUUID);
            writer.WriteSignedLong(expireTime);
            writer.WriteVarInt(publicKey.Length);
            writer.WriteBuffer(publicKey);
            writer.WriteVarInt(signature.Length);
            writer.WriteBuffer(signature);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendChunkBatchReceived(float chunksPerTick)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x07);
            writer.WriteFloat(chunksPerTick);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendConfigurationAcknowledged()
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0b);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPingRequest(long id)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1e);
            writer.WriteSignedLong(id);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetSlotState(int slot_id, int window_id, bool state)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0f);
            writer.WriteVarInt(slot_id);
            writer.WriteVarInt(window_id);
            writer.WriteBoolean(state);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }
    }

    public class PacketSpawnEntity
    {
        public PacketSpawnEntity(int entityId, Guid objectUUID, int type, double x, double y, double z, sbyte pitch,
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

        public int EntityId { get; }
        public Guid ObjectUUID { get; }
        public int Type { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public sbyte Pitch { get; }
        public sbyte Yaw { get; }
        public sbyte HeadPitch { get; }
        public int ObjectData { get; }
        public short VelocityX { get; }
        public short VelocityY { get; }
        public short VelocityZ { get; }
    }

    public class PacketSpawnEntityExperienceOrb
    {
        public PacketSpawnEntityExperienceOrb(int entityId, double x, double y, double z, short count)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Count = count;
        }

        public int EntityId { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public short Count { get; }
    }

    public class PacketAnimation
    {
        public PacketAnimation(int entityId, byte animation)
        {
            EntityId = entityId;
            Animation = animation;
        }

        public int EntityId { get; }
        public byte Animation { get; }
    }

    public class PacketDifficulty
    {
        public PacketDifficulty(byte difficulty, bool difficultyLocked)
        {
            Difficulty = difficulty;
            DifficultyLocked = difficultyLocked;
        }

        public byte Difficulty { get; }
        public bool DifficultyLocked { get; }
    }

    public class PacketChunkBatchFinished
    {
        public PacketChunkBatchFinished(int batchSize)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }
    }

    public class PacketChunkBatchStart
    {
        public PacketChunkBatchStart()
        {
        }
    }

    public class PacketCloseWindow
    {
        public PacketCloseWindow(byte windowId)
        {
            WindowId = windowId;
        }

        public byte WindowId { get; }
    }

    public class PacketCraftProgressBar
    {
        public PacketCraftProgressBar(byte windowId, short property, short value)
        {
            WindowId = windowId;
            Property = property;
            Value = value;
        }

        public byte WindowId { get; }
        public short Property { get; }
        public short Value { get; }
    }

    public class PacketSetCooldown
    {
        public PacketSetCooldown(int itemID, int cooldownTicks)
        {
            ItemID = itemID;
            CooldownTicks = cooldownTicks;
        }

        public int ItemID { get; }
        public int CooldownTicks { get; }
    }

    public class PacketChatSuggestions
    {
        public PacketChatSuggestions(int action, string[] entries)
        {
            Action = action;
            Entries = entries;
        }

        public int Action { get; }
        public string[] Entries { get; }
    }

    public class PacketEntityStatus
    {
        public PacketEntityStatus(int entityId, sbyte entityStatus)
        {
            EntityId = entityId;
            EntityStatus = entityStatus;
        }

        public int EntityId { get; }
        public sbyte EntityStatus { get; }
    }

    public class PacketUnloadChunk
    {
        public PacketUnloadChunk(int chunkZ, int chunkX)
        {
            ChunkZ = chunkZ;
            ChunkX = chunkX;
        }

        public int ChunkZ { get; }
        public int ChunkX { get; }
    }

    public class PacketGameStateChange
    {
        public PacketGameStateChange(byte reason, float gameMode)
        {
            Reason = reason;
            GameMode = gameMode;
        }

        public byte Reason { get; }
        public float GameMode { get; }
    }

    public class PacketOpenHorseWindow
    {
        public PacketOpenHorseWindow(byte windowId, int nbSlots, int entityId)
        {
            WindowId = windowId;
            NbSlots = nbSlots;
            EntityId = entityId;
        }

        public byte WindowId { get; }
        public int NbSlots { get; }
        public int EntityId { get; }
    }

    public class PacketKeepAlive
    {
        public PacketKeepAlive(long keepAliveId)
        {
            KeepAliveId = keepAliveId;
        }

        public long KeepAliveId { get; }
    }

    public class PacketRelEntityMove
    {
        public PacketRelEntityMove(int entityId, short dX, short dY, short dZ, bool onGround)
        {
            EntityId = entityId;
            DX = dX;
            DY = dY;
            DZ = dZ;
            OnGround = onGround;
        }

        public int EntityId { get; }
        public short DX { get; }
        public short DY { get; }
        public short DZ { get; }
        public bool OnGround { get; }
    }

    public class PacketEntityMoveLook
    {
        public PacketEntityMoveLook(int entityId, short dX, short dY, short dZ, sbyte yaw, sbyte pitch, bool onGround)
        {
            EntityId = entityId;
            DX = dX;
            DY = dY;
            DZ = dZ;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public int EntityId { get; }
        public short DX { get; }
        public short DY { get; }
        public short DZ { get; }
        public sbyte Yaw { get; }
        public sbyte Pitch { get; }
        public bool OnGround { get; }
    }

    public class PacketEntityLook
    {
        public PacketEntityLook(int entityId, sbyte yaw, sbyte pitch, bool onGround)
        {
            EntityId = entityId;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public int EntityId { get; }
        public sbyte Yaw { get; }
        public sbyte Pitch { get; }
        public bool OnGround { get; }
    }

    public class PacketVehicleMove
    {
        public PacketVehicleMove(double x, double y, double z, float yaw, float pitch)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public float Yaw { get; }
        public float Pitch { get; }
    }

    public class PacketOpenBook
    {
        public PacketOpenBook(int hand)
        {
            Hand = hand;
        }

        public int Hand { get; }
    }

    public class PacketCraftRecipeResponse
    {
        public PacketCraftRecipeResponse(sbyte windowId, string recipe)
        {
            WindowId = windowId;
            Recipe = recipe;
        }

        public sbyte WindowId { get; }
        public string Recipe { get; }
    }

    public class PacketAbilities
    {
        public PacketAbilities(sbyte flags, float flyingSpeed, float walkingSpeed)
        {
            Flags = flags;
            FlyingSpeed = flyingSpeed;
            WalkingSpeed = walkingSpeed;
        }

        public sbyte Flags { get; }
        public float FlyingSpeed { get; }
        public float WalkingSpeed { get; }
    }

    public class PacketEndCombatEvent
    {
        public PacketEndCombatEvent(int duration)
        {
            Duration = duration;
        }

        public int Duration { get; }
    }

    public class PacketEnterCombatEvent
    {
        public PacketEnterCombatEvent()
        {
        }
    }

    public class PacketPosition
    {
        public PacketPosition(double x, double y, double z, float yaw, float pitch, sbyte flags, int teleportId)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            Flags = flags;
            TeleportId = teleportId;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public float Yaw { get; }
        public float Pitch { get; }
        public sbyte Flags { get; }
        public int TeleportId { get; }
    }

    public class PacketEntityDestroy
    {
        public PacketEntityDestroy(int[] entityIds)
        {
            EntityIds = entityIds;
        }

        public int[] EntityIds { get; }
    }

    public class PacketRemoveEntityEffect
    {
        public PacketRemoveEntityEffect(int entityId, int effectId)
        {
            EntityId = entityId;
            EffectId = effectId;
        }

        public int EntityId { get; }
        public int EffectId { get; }
    }

    public class PacketResetScore
    {
        public PacketResetScore(string entity_name, string? objective_name)
        {
            EntityName = entity_name;
            ObjectiveName = objective_name;
        }

        public string EntityName { get; }
        public string? ObjectiveName { get; }
    }

    public class PacketEntityHeadRotation
    {
        public PacketEntityHeadRotation(int entityId, sbyte headYaw)
        {
            EntityId = entityId;
            HeadYaw = headYaw;
        }

        public int EntityId { get; }
        public sbyte HeadYaw { get; }
    }

    public class PacketCamera
    {
        public PacketCamera(int cameraId)
        {
            CameraId = cameraId;
        }

        public int CameraId { get; }
    }

    public class PacketHeldItemSlot
    {
        public PacketHeldItemSlot(sbyte slot)
        {
            Slot = slot;
        }

        public sbyte Slot { get; }
    }

    public class PacketUpdateViewPosition
    {
        public PacketUpdateViewPosition(int chunkX, int chunkZ)
        {
            ChunkX = chunkX;
            ChunkZ = chunkZ;
        }

        public int ChunkX { get; }
        public int ChunkZ { get; }
    }

    public class PacketUpdateViewDistance
    {
        public PacketUpdateViewDistance(int viewDistance)
        {
            ViewDistance = viewDistance;
        }

        public int ViewDistance { get; }
    }

    public class PacketScoreboardDisplayObjective
    {
        public PacketScoreboardDisplayObjective(int position, string name)
        {
            Position = position;
            Name = name;
        }

        public int Position { get; }
        public string Name { get; }
    }

    public class PacketAttachEntity
    {
        public PacketAttachEntity(int entityId, int vehicleId)
        {
            EntityId = entityId;
            VehicleId = vehicleId;
        }

        public int EntityId { get; }
        public int VehicleId { get; }
    }

    public class PacketEntityVelocity
    {
        public PacketEntityVelocity(int entityId, short velocityX, short velocityY, short velocityZ)
        {
            EntityId = entityId;
            VelocityX = velocityX;
            VelocityY = velocityY;
            VelocityZ = velocityZ;
        }

        public int EntityId { get; }
        public short VelocityX { get; }
        public short VelocityY { get; }
        public short VelocityZ { get; }
    }

    public class PacketExperience
    {
        public PacketExperience(float experienceBar, int level, int totalExperience)
        {
            ExperienceBar = experienceBar;
            Level = level;
            TotalExperience = totalExperience;
        }

        public float ExperienceBar { get; }
        public int Level { get; }
        public int TotalExperience { get; }
    }

    public class PacketUpdateHealth
    {
        public PacketUpdateHealth(float health, int food, float foodSaturation)
        {
            Health = health;
            Food = food;
            FoodSaturation = foodSaturation;
        }

        public float Health { get; }
        public int Food { get; }
        public float FoodSaturation { get; }
    }

    public class PacketSetPassengers
    {
        public PacketSetPassengers(int entityId, int[] passengers)
        {
            EntityId = entityId;
            Passengers = passengers;
        }

        public int EntityId { get; }
        public int[] Passengers { get; }
    }

    public class PacketUpdateTime
    {
        public PacketUpdateTime(long age, long time)
        {
            Age = age;
            Time = time;
        }

        public long Age { get; }
        public long Time { get; }
    }

    public class PacketCollect
    {
        public PacketCollect(int collectedEntityId, int collectorEntityId, int pickupItemCount)
        {
            CollectedEntityId = collectedEntityId;
            CollectorEntityId = collectorEntityId;
            PickupItemCount = pickupItemCount;
        }

        public int CollectedEntityId { get; }
        public int CollectorEntityId { get; }
        public int PickupItemCount { get; }
    }

    public class PacketEntityTeleport
    {
        public PacketEntityTeleport(int entityId, double x, double y, double z, sbyte yaw, sbyte pitch, bool onGround)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public int EntityId { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public sbyte Yaw { get; }
        public sbyte Pitch { get; }
        public bool OnGround { get; }
    }

    public class PacketSelectAdvancementTab
    {
        public PacketSelectAdvancementTab(string? id)
        {
            Id = id;
        }

        public string? Id { get; }
    }

    public class PacketAcknowledgePlayerDigging
    {
        public PacketAcknowledgePlayerDigging(int sequenceId)
        {
            SequenceId = sequenceId;
        }

        public int SequenceId { get; }
    }

    public class PacketClearTitles
    {
        public PacketClearTitles(bool reset)
        {
            Reset = reset;
        }

        public bool Reset { get; }
    }

    public class PacketInitializeWorldBorder
    {
        public PacketInitializeWorldBorder(double x, double z, double oldDiameter, double newDiameter, int speed,
            int portalTeleportBoundary, int warningBlocks, int warningTime)
        {
            X = x;
            Z = z;
            OldDiameter = oldDiameter;
            NewDiameter = newDiameter;
            Speed = speed;
            PortalTeleportBoundary = portalTeleportBoundary;
            WarningBlocks = warningBlocks;
            WarningTime = warningTime;
        }

        public double X { get; }
        public double Z { get; }
        public double OldDiameter { get; }
        public double NewDiameter { get; }
        public int Speed { get; }
        public int PortalTeleportBoundary { get; }
        public int WarningBlocks { get; }
        public int WarningTime { get; }
    }

    public class PacketWorldBorderCenter
    {
        public PacketWorldBorderCenter(double x, double z)
        {
            X = x;
            Z = z;
        }

        public double X { get; }
        public double Z { get; }
    }

    public class PacketWorldBorderLerpSize
    {
        public PacketWorldBorderLerpSize(double oldDiameter, double newDiameter, int speed)
        {
            OldDiameter = oldDiameter;
            NewDiameter = newDiameter;
            Speed = speed;
        }

        public double OldDiameter { get; }
        public double NewDiameter { get; }
        public int Speed { get; }
    }

    public class PacketWorldBorderSize
    {
        public PacketWorldBorderSize(double diameter)
        {
            Diameter = diameter;
        }

        public double Diameter { get; }
    }

    public class PacketWorldBorderWarningDelay
    {
        public PacketWorldBorderWarningDelay(int warningTime)
        {
            WarningTime = warningTime;
        }

        public int WarningTime { get; }
    }

    public class PacketWorldBorderWarningReach
    {
        public PacketWorldBorderWarningReach(int warningBlocks)
        {
            WarningBlocks = warningBlocks;
        }

        public int WarningBlocks { get; }
    }

    public class PacketPing
    {
        public PacketPing(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class PacketPingResponse
    {
        public PacketPingResponse(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class PacketSetTitleTime
    {
        public PacketSetTitleTime(int fadeIn, int stay, int fadeOut)
        {
            FadeIn = fadeIn;
            Stay = stay;
            FadeOut = fadeOut;
        }

        public int FadeIn { get; }
        public int Stay { get; }
        public int FadeOut { get; }
    }

    public class PacketSimulationDistance
    {
        public PacketSimulationDistance(int distance)
        {
            Distance = distance;
        }

        public int Distance { get; }
    }

    public class PacketHurtAnimation
    {
        public PacketHurtAnimation(int entityId, float yaw)
        {
            EntityId = entityId;
            Yaw = yaw;
        }

        public int EntityId { get; }
        public float Yaw { get; }
    }

    public class PacketStartConfiguration
    {
        public PacketStartConfiguration()
        {
        }
    }

    public class PacketSetTickingState
    {
        public PacketSetTickingState(float tick_rate, bool is_frozen)
        {
            TickRate = tick_rate;
            IsFrozen = is_frozen;
        }

        public float TickRate { get; }
        public bool IsFrozen { get; }
    }

    public class PacketStepTick
    {
        public PacketStepTick(int tick_steps)
        {
            TickSteps = tick_steps;
        }

        public int TickSteps { get; }
    }
}