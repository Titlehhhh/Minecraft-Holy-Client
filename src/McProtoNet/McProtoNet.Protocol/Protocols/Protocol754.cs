using McProtoNet.Serialization;
using McProtoNet.Protocol;
using McProtoNet.Abstractions;
using System.Reactive.Subjects;

namespace McProtoNet.Protocol754
{
    public sealed class Protocol_754 : ProtocolBase
    {
        public Protocol_754(IPacketBroker client) : base(client)
        {
        }

        private readonly Subject<PacketSpawnEntity> _onspawn_entity = new();
        private readonly Subject<PacketSpawnEntityExperienceOrb> _onspawn_entity_experience_orb = new();
        private readonly Subject<PacketSpawnEntityLiving> _onspawn_entity_living = new();
        private readonly Subject<PacketNamedEntitySpawn> _onnamed_entity_spawn = new();
        private readonly Subject<PacketAnimation> _onanimation = new();
        private readonly Subject<PacketDifficulty> _ondifficulty = new();
        private readonly Subject<PacketChat> _onchat = new();
        private readonly Subject<PacketTransaction> _ontransaction = new();
        private readonly Subject<PacketCloseWindow> _onclose_window = new();
        private readonly Subject<PacketOpenWindow> _onopen_window = new();
        private readonly Subject<PacketCraftProgressBar> _oncraft_progress_bar = new();
        private readonly Subject<PacketSetCooldown> _onset_cooldown = new();
        private readonly Subject<PacketNamedSoundEffect> _onnamed_sound_effect = new();
        private readonly Subject<PacketKickDisconnect> _onkick_disconnect = new();
        private readonly Subject<PacketEntityStatus> _onentity_status = new();
        private readonly Subject<PacketUnloadChunk> _onunload_chunk = new();
        private readonly Subject<PacketGameStateChange> _ongame_state_change = new();
        private readonly Subject<PacketOpenHorseWindow> _onopen_horse_window = new();
        private readonly Subject<PacketKeepAlive> _onkeep_alive = new();
        private readonly Subject<PacketRelEntityMove> _onrel_entity_move = new();
        private readonly Subject<PacketEntityMoveLook> _onentity_move_look = new();
        private readonly Subject<PacketEntityLook> _onentity_look = new();
        private readonly Subject<PacketEntity> _onentity = new();
        private readonly Subject<PacketVehicleMove> _onvehicle_move = new();
        private readonly Subject<PacketOpenBook> _onopen_book = new();
        private readonly Subject<PacketCraftRecipeResponse> _oncraft_recipe_response = new();
        private readonly Subject<PacketAbilities> _onabilities = new();
        private readonly Subject<PacketPosition> _onposition = new();
        private readonly Subject<PacketEntityDestroy> _onentity_destroy = new();
        private readonly Subject<PacketRemoveEntityEffect> _onremove_entity_effect = new();
        private readonly Subject<PacketResourcePackSend> _onresource_pack_send = new();
        private readonly Subject<PacketEntityHeadRotation> _onentity_head_rotation = new();
        private readonly Subject<PacketCamera> _oncamera = new();
        private readonly Subject<PacketHeldItemSlot> _onheld_item_slot = new();
        private readonly Subject<PacketUpdateViewPosition> _onupdate_view_position = new();
        private readonly Subject<PacketUpdateViewDistance> _onupdate_view_distance = new();
        private readonly Subject<PacketScoreboardDisplayObjective> _onscoreboard_display_objective = new();
        private readonly Subject<PacketAttachEntity> _onattach_entity = new();
        private readonly Subject<PacketEntityVelocity> _onentity_velocity = new();
        private readonly Subject<PacketExperience> _onexperience = new();
        private readonly Subject<PacketUpdateHealth> _onupdate_health = new();
        private readonly Subject<PacketSetPassengers> _onset_passengers = new();
        private readonly Subject<PacketUpdateTime> _onupdate_time = new();
        private readonly Subject<PacketEntitySoundEffect> _onentity_sound_effect = new();
        private readonly Subject<PacketSoundEffect> _onsound_effect = new();
        private readonly Subject<PacketPlayerlistHeader> _onplayerlist_header = new();
        private readonly Subject<PacketCollect> _oncollect = new();
        private readonly Subject<PacketEntityTeleport> _onentity_teleport = new();
        private readonly Subject<PacketEntityEffect> _onentity_effect = new();
        private readonly Subject<PacketSelectAdvancementTab> _onselect_advancement_tab = new();

        public IObservable<PacketSpawnEntity> OnSpawnEntityPacket => _onspawn_entity;

        public IObservable<PacketSpawnEntityExperienceOrb> OnSpawnEntityExperienceOrbPacket =>
            _onspawn_entity_experience_orb;

        public IObservable<PacketSpawnEntityLiving> OnSpawnEntityLivingPacket => _onspawn_entity_living;
        public IObservable<PacketNamedEntitySpawn> OnNamedEntitySpawnPacket => _onnamed_entity_spawn;
        public IObservable<PacketAnimation> OnAnimationPacket => _onanimation;
        public IObservable<PacketDifficulty> OnDifficultyPacket => _ondifficulty;
        public IObservable<PacketChat> OnChatPacket => _onchat;
        public IObservable<PacketTransaction> OnTransactionPacket => _ontransaction;
        public IObservable<PacketCloseWindow> OnCloseWindowPacket => _onclose_window;
        public IObservable<PacketOpenWindow> OnOpenWindowPacket => _onopen_window;
        public IObservable<PacketCraftProgressBar> OnCraftProgressBarPacket => _oncraft_progress_bar;
        public IObservable<PacketSetCooldown> OnSetCooldownPacket => _onset_cooldown;
        public IObservable<PacketNamedSoundEffect> OnNamedSoundEffectPacket => _onnamed_sound_effect;
        public IObservable<PacketKickDisconnect> OnKickDisconnectPacket => _onkick_disconnect;
        public IObservable<PacketEntityStatus> OnEntityStatusPacket => _onentity_status;
        public IObservable<PacketUnloadChunk> OnUnloadChunkPacket => _onunload_chunk;
        public IObservable<PacketGameStateChange> OnGameStateChangePacket => _ongame_state_change;
        public IObservable<PacketOpenHorseWindow> OnOpenHorseWindowPacket => _onopen_horse_window;
        public IObservable<PacketKeepAlive> OnKeepAlivePacket => _onkeep_alive;
        public IObservable<PacketRelEntityMove> OnRelEntityMovePacket => _onrel_entity_move;
        public IObservable<PacketEntityMoveLook> OnEntityMoveLookPacket => _onentity_move_look;
        public IObservable<PacketEntityLook> OnEntityLookPacket => _onentity_look;
        public IObservable<PacketEntity> OnEntityPacket => _onentity;
        public IObservable<PacketVehicleMove> OnVehicleMovePacket => _onvehicle_move;
        public IObservable<PacketOpenBook> OnOpenBookPacket => _onopen_book;
        public IObservable<PacketCraftRecipeResponse> OnCraftRecipeResponsePacket => _oncraft_recipe_response;
        public IObservable<PacketAbilities> OnAbilitiesPacket => _onabilities;
        public IObservable<PacketPosition> OnPositionPacket => _onposition;
        public IObservable<PacketEntityDestroy> OnEntityDestroyPacket => _onentity_destroy;
        public IObservable<PacketRemoveEntityEffect> OnRemoveEntityEffectPacket => _onremove_entity_effect;
        public IObservable<PacketResourcePackSend> OnResourcePackSendPacket => _onresource_pack_send;
        public IObservable<PacketEntityHeadRotation> OnEntityHeadRotationPacket => _onentity_head_rotation;
        public IObservable<PacketCamera> OnCameraPacket => _oncamera;
        public IObservable<PacketHeldItemSlot> OnHeldItemSlotPacket => _onheld_item_slot;
        public IObservable<PacketUpdateViewPosition> OnUpdateViewPositionPacket => _onupdate_view_position;
        public IObservable<PacketUpdateViewDistance> OnUpdateViewDistancePacket => _onupdate_view_distance;

        public IObservable<PacketScoreboardDisplayObjective> OnScoreboardDisplayObjectivePacket =>
            _onscoreboard_display_objective;

        public IObservable<PacketAttachEntity> OnAttachEntityPacket => _onattach_entity;
        public IObservable<PacketEntityVelocity> OnEntityVelocityPacket => _onentity_velocity;
        public IObservable<PacketExperience> OnExperiencePacket => _onexperience;
        public IObservable<PacketUpdateHealth> OnUpdateHealthPacket => _onupdate_health;
        public IObservable<PacketSetPassengers> OnSetPassengersPacket => _onset_passengers;
        public IObservable<PacketUpdateTime> OnUpdateTimePacket => _onupdate_time;
        public IObservable<PacketEntitySoundEffect> OnEntitySoundEffectPacket => _onentity_sound_effect;
        public IObservable<PacketSoundEffect> OnSoundEffectPacket => _onsound_effect;
        public IObservable<PacketPlayerlistHeader> OnPlayerlistHeaderPacket => _onplayerlist_header;
        public IObservable<PacketCollect> OnCollectPacket => _oncollect;
        public IObservable<PacketEntityTeleport> OnEntityTeleportPacket => _onentity_teleport;
        public IObservable<PacketEntityEffect> OnEntityEffectPacket => _onentity_effect;
        public IObservable<PacketSelectAdvancementTab> OnSelectAdvancementTabPacket => _onselect_advancement_tab;

        public Task SendTeleportConfirm(int teleportId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x00);
            writer.WriteVarInt(teleportId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetDifficulty(byte newDifficulty)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x02);
            writer.WriteUnsignedByte(newDifficulty);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendQueryEntityNbt(int transactionId, int entityId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0d);
            writer.WriteVarInt(transactionId);
            writer.WriteVarInt(entityId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPickItem(int slot)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x18);
            writer.WriteVarInt(slot);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendNameItem(string name)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x20);
            writer.WriteString(name);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSelectTrade(int slot)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x23);
            writer.WriteVarInt(slot);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetBeaconEffect(int primary_effect, int secondary_effect)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x24);
            writer.WriteVarInt(primary_effect);
            writer.WriteVarInt(secondary_effect);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUpdateCommandBlockMinecart(int entityId, string command, bool track_output)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x27);
            writer.WriteVarInt(entityId);
            writer.WriteString(command);
            writer.WriteBoolean(track_output);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendTabComplete(int transactionId, string text)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x06);
            writer.WriteVarInt(transactionId);
            writer.WriteString(text);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendChat(string message)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x03);
            writer.WriteString(message);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendClientCommand(int actionId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x04);
            writer.WriteVarInt(actionId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSettings(string locale, sbyte viewDistance, int chatFlags, bool chatColors, byte skinParts,
            int mainHand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x05);
            writer.WriteString(locale);
            writer.WriteSignedByte(viewDistance);
            writer.WriteVarInt(chatFlags);
            writer.WriteBoolean(chatColors);
            writer.WriteUnsignedByte(skinParts);
            writer.WriteVarInt(mainHand);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendTransaction(sbyte windowId, short action, bool accepted)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x07);
            writer.WriteSignedByte(windowId);
            writer.WriteSignedShort(action);
            writer.WriteBoolean(accepted);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEnchantItem(sbyte windowId, sbyte enchantment)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x08);
            writer.WriteSignedByte(windowId);
            writer.WriteSignedByte(enchantment);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCloseWindow(byte windowId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0a);
            writer.WriteUnsignedByte(windowId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendKeepAlive(long keepAliveId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x10);
            writer.WriteSignedLong(keepAliveId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendLockDifficulty(bool locked)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x11);
            writer.WriteBoolean(locked);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPosition(double x, double y, double z, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x12);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x13);
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
            writer.WriteVarInt(0x14);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendFlying(bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x15);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendVehicleMove(double x, double y, double z, float yaw, float pitch)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x16);
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
            writer.WriteVarInt(0x17);
            writer.WriteBoolean(leftPaddle);
            writer.WriteBoolean(rightPaddle);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCraftRecipeRequest(sbyte windowId, string recipe, bool makeAll)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x19);
            writer.WriteSignedByte(windowId);
            writer.WriteString(recipe);
            writer.WriteBoolean(makeAll);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendAbilities(sbyte flags)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1a);
            writer.WriteSignedByte(flags);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEntityAction(int entityId, int actionId, int jumpBoost)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1c);
            writer.WriteVarInt(entityId);
            writer.WriteVarInt(actionId);
            writer.WriteVarInt(jumpBoost);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSteerVehicle(float sideways, float forward, byte jump)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1d);
            writer.WriteFloat(sideways);
            writer.WriteFloat(forward);
            writer.WriteUnsignedByte(jump);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendDisplayedRecipe(string recipeId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1f);
            writer.WriteString(recipeId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendRecipeBook(int bookId, bool bookOpen, bool filterActive)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1e);
            writer.WriteVarInt(bookId);
            writer.WriteBoolean(bookOpen);
            writer.WriteBoolean(filterActive);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendResourcePackReceive(int result)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x21);
            writer.WriteVarInt(result);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendHeldItemSlot(short slotId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x25);
            writer.WriteSignedShort(slotId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendArmAnimation(int hand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2c);
            writer.WriteVarInt(hand);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSpectate(Guid target)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2d);
            writer.WriteUUID(target);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUseItem(int hand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2f);
            writer.WriteVarInt(hand);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public override void OnPacketReceived(InputPacket packet)
        {
            switch (packet.Id)
            {
                case 0x00:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var objectUUID = reader.ReadUUID();
                    var type = reader.ReadVarInt();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var pitch = reader.ReadSignedByte();
                    var yaw = reader.ReadSignedByte();
                    var objectData = reader.ReadSignedInt();
                    var velocityX = reader.ReadSignedShort();
                    var velocityY = reader.ReadSignedShort();
                    var velocityZ = reader.ReadSignedShort();
                    _onspawn_entity.OnNext(new PacketSpawnEntity(entityId, objectUUID, type, x, y, z, pitch, yaw,
                        objectData, velocityX, velocityY, velocityZ));
                }
                    break;
                case 0x01:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var count = reader.ReadSignedShort();
                    _onspawn_entity_experience_orb.OnNext(new PacketSpawnEntityExperienceOrb(entityId, x, y, z, count));
                }
                    break;
                case 0x02:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var entityUUID = reader.ReadUUID();
                    var type = reader.ReadVarInt();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var headPitch = reader.ReadSignedByte();
                    var velocityX = reader.ReadSignedShort();
                    var velocityY = reader.ReadSignedShort();
                    var velocityZ = reader.ReadSignedShort();
                    _onspawn_entity_living.OnNext(new PacketSpawnEntityLiving(entityId, entityUUID, type, x, y, z, yaw,
                        pitch, headPitch, velocityX, velocityY, velocityZ));
                }
                    break;
                case 0x04:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var playerUUID = reader.ReadUUID();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    _onnamed_entity_spawn.OnNext(new PacketNamedEntitySpawn(entityId, playerUUID, x, y, z, yaw, pitch));
                }
                    break;
                case 0x05:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var animation = reader.ReadUnsignedByte();
                    _onanimation.OnNext(new PacketAnimation(entityId, animation));
                }
                    break;
                case 0x0d:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var difficulty = reader.ReadUnsignedByte();
                    var difficultyLocked = reader.ReadBoolean();
                    _ondifficulty.OnNext(new PacketDifficulty(difficulty, difficultyLocked));
                }
                    break;
                case 0x0e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var message = reader.ReadString();
                    var position = reader.ReadSignedByte();
                    var sender = reader.ReadUUID();
                    _onchat.OnNext(new PacketChat(message, position, sender));
                }
                    break;
                case 0x11:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadSignedByte();
                    var action = reader.ReadSignedShort();
                    var accepted = reader.ReadBoolean();
                    _ontransaction.OnNext(new PacketTransaction(windowId, action, accepted));
                }
                    break;
                case 0x12:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    _onclose_window.OnNext(new PacketCloseWindow(windowId));
                }
                    break;
                case 0x2d:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadVarInt();
                    var inventoryType = reader.ReadVarInt();
                    var windowTitle = reader.ReadString();
                    _onopen_window.OnNext(new PacketOpenWindow(windowId, inventoryType, windowTitle));
                }
                    break;
                case 0x14:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    var property = reader.ReadSignedShort();
                    var value = reader.ReadSignedShort();
                    _oncraft_progress_bar.OnNext(new PacketCraftProgressBar(windowId, property, value));
                }
                    break;
                case 0x16:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var itemID = reader.ReadVarInt();
                    var cooldownTicks = reader.ReadVarInt();
                    _onset_cooldown.OnNext(new PacketSetCooldown(itemID, cooldownTicks));
                }
                    break;
                case 0x18:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var soundName = reader.ReadString();
                    var soundCategory = reader.ReadVarInt();
                    var x = reader.ReadSignedInt();
                    var y = reader.ReadSignedInt();
                    var z = reader.ReadSignedInt();
                    var volume = reader.ReadFloat();
                    var pitch = reader.ReadFloat();
                    _onnamed_sound_effect.OnNext(new PacketNamedSoundEffect(soundName, soundCategory, x, y, z, volume,
                        pitch));
                }
                    break;
                case 0x19:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var reason = reader.ReadString();
                    _onkick_disconnect.OnNext(new PacketKickDisconnect(reason));
                }
                    break;
                case 0x1a:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadSignedInt();
                    var entityStatus = reader.ReadSignedByte();
                    _onentity_status.OnNext(new PacketEntityStatus(entityId, entityStatus));
                }
                    break;
                case 0x1c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var chunkX = reader.ReadSignedInt();
                    var chunkZ = reader.ReadSignedInt();
                    _onunload_chunk.OnNext(new PacketUnloadChunk(chunkX, chunkZ));
                }
                    break;
                case 0x1d:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var reason = reader.ReadUnsignedByte();
                    var gameMode = reader.ReadFloat();
                    _ongame_state_change.OnNext(new PacketGameStateChange(reason, gameMode));
                }
                    break;
                case 0x1e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    var nbSlots = reader.ReadVarInt();
                    var entityId = reader.ReadSignedInt();
                    _onopen_horse_window.OnNext(new PacketOpenHorseWindow(windowId, nbSlots, entityId));
                }
                    break;
                case 0x1f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var keepAliveId = reader.ReadSignedLong();
                    _onkeep_alive.OnNext(new PacketKeepAlive(keepAliveId));
                }
                    break;
                case 0x27:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var dX = reader.ReadSignedShort();
                    var dY = reader.ReadSignedShort();
                    var dZ = reader.ReadSignedShort();
                    var onGround = reader.ReadBoolean();
                    _onrel_entity_move.OnNext(new PacketRelEntityMove(entityId, dX, dY, dZ, onGround));
                }
                    break;
                case 0x28:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var dX = reader.ReadSignedShort();
                    var dY = reader.ReadSignedShort();
                    var dZ = reader.ReadSignedShort();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var onGround = reader.ReadBoolean();
                    _onentity_move_look.OnNext(new PacketEntityMoveLook(entityId, dX, dY, dZ, yaw, pitch, onGround));
                }
                    break;
                case 0x29:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var onGround = reader.ReadBoolean();
                    _onentity_look.OnNext(new PacketEntityLook(entityId, yaw, pitch, onGround));
                }
                    break;
                case 0x2a:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    _onentity.OnNext(new PacketEntity(entityId));
                }
                    break;
                case 0x2b:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var yaw = reader.ReadFloat();
                    var pitch = reader.ReadFloat();
                    _onvehicle_move.OnNext(new PacketVehicleMove(x, y, z, yaw, pitch));
                }
                    break;
                case 0x2c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var hand = reader.ReadVarInt();
                    _onopen_book.OnNext(new PacketOpenBook(hand));
                }
                    break;
                case 0x2f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadSignedByte();
                    var recipe = reader.ReadString();
                    _oncraft_recipe_response.OnNext(new PacketCraftRecipeResponse(windowId, recipe));
                }
                    break;
                case 0x30:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var flags = reader.ReadSignedByte();
                    var flyingSpeed = reader.ReadFloat();
                    var walkingSpeed = reader.ReadFloat();
                    _onabilities.OnNext(new PacketAbilities(flags, flyingSpeed, walkingSpeed));
                }
                    break;
                case 0x34:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var yaw = reader.ReadFloat();
                    var pitch = reader.ReadFloat();
                    var flags = reader.ReadSignedByte();
                    var teleportId = reader.ReadVarInt();
                    _onposition.OnNext(new PacketPosition(x, y, z, yaw, pitch, flags, teleportId));
                }
                    break;
                case 0x36:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var tempArrayLength_0 = reader.ReadVarInt();
                    var tempArray_0 = new int[tempArrayLength_0];
                    for (int i_0 = 0; i_0 < tempArrayLength_0; i_0++)
                    {
                        var for_item_0 = reader.ReadVarInt();
                        tempArray_0[i_0] = for_item_0;
                    }

                    var entityIds = tempArray_0;
                    _onentity_destroy.OnNext(new PacketEntityDestroy(entityIds));
                }
                    break;
                case 0x37:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var effectId = reader.ReadSignedByte();
                    _onremove_entity_effect.OnNext(new PacketRemoveEntityEffect(entityId, effectId));
                }
                    break;
                case 0x38:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var url = reader.ReadString();
                    var hash = reader.ReadString();
                    _onresource_pack_send.OnNext(new PacketResourcePackSend(url, hash));
                }
                    break;
                case 0x3a:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var headYaw = reader.ReadSignedByte();
                    _onentity_head_rotation.OnNext(new PacketEntityHeadRotation(entityId, headYaw));
                }
                    break;
                case 0x3e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var cameraId = reader.ReadVarInt();
                    _oncamera.OnNext(new PacketCamera(cameraId));
                }
                    break;
                case 0x3f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var slot = reader.ReadSignedByte();
                    _onheld_item_slot.OnNext(new PacketHeldItemSlot(slot));
                }
                    break;
                case 0x40:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var chunkX = reader.ReadVarInt();
                    var chunkZ = reader.ReadVarInt();
                    _onupdate_view_position.OnNext(new PacketUpdateViewPosition(chunkX, chunkZ));
                }
                    break;
                case 0x41:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var viewDistance = reader.ReadVarInt();
                    _onupdate_view_distance.OnNext(new PacketUpdateViewDistance(viewDistance));
                }
                    break;
                case 0x43:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var position = reader.ReadSignedByte();
                    var name = reader.ReadString();
                    _onscoreboard_display_objective.OnNext(new PacketScoreboardDisplayObjective(position, name));
                }
                    break;
                case 0x45:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadSignedInt();
                    var vehicleId = reader.ReadSignedInt();
                    _onattach_entity.OnNext(new PacketAttachEntity(entityId, vehicleId));
                }
                    break;
                case 0x46:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var velocityX = reader.ReadSignedShort();
                    var velocityY = reader.ReadSignedShort();
                    var velocityZ = reader.ReadSignedShort();
                    _onentity_velocity.OnNext(new PacketEntityVelocity(entityId, velocityX, velocityY, velocityZ));
                }
                    break;
                case 0x48:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var experienceBar = reader.ReadFloat();
                    var level = reader.ReadVarInt();
                    var totalExperience = reader.ReadVarInt();
                    _onexperience.OnNext(new PacketExperience(experienceBar, level, totalExperience));
                }
                    break;
                case 0x49:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var health = reader.ReadFloat();
                    var food = reader.ReadVarInt();
                    var foodSaturation = reader.ReadFloat();
                    _onupdate_health.OnNext(new PacketUpdateHealth(health, food, foodSaturation));
                }
                    break;
                case 0x4b:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var tempArrayLength_0 = reader.ReadVarInt();
                    var tempArray_0 = new int[tempArrayLength_0];
                    for (int i_0 = 0; i_0 < tempArrayLength_0; i_0++)
                    {
                        var for_item_0 = reader.ReadVarInt();
                        tempArray_0[i_0] = for_item_0;
                    }

                    var passengers = tempArray_0;
                    _onset_passengers.OnNext(new PacketSetPassengers(entityId, passengers));
                }
                    break;
                case 0x4e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var age = reader.ReadSignedLong();
                    var time = reader.ReadSignedLong();
                    _onupdate_time.OnNext(new PacketUpdateTime(age, time));
                }
                    break;
                case 0x50:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var soundId = reader.ReadVarInt();
                    var soundCategory = reader.ReadVarInt();
                    var entityId = reader.ReadVarInt();
                    var volume = reader.ReadFloat();
                    var pitch = reader.ReadFloat();
                    _onentity_sound_effect.OnNext(new PacketEntitySoundEffect(soundId, soundCategory, entityId, volume,
                        pitch));
                }
                    break;
                case 0x51:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var soundId = reader.ReadVarInt();
                    var soundCategory = reader.ReadVarInt();
                    var x = reader.ReadSignedInt();
                    var y = reader.ReadSignedInt();
                    var z = reader.ReadSignedInt();
                    var volume = reader.ReadFloat();
                    var pitch = reader.ReadFloat();
                    _onsound_effect.OnNext(new PacketSoundEffect(soundId, soundCategory, x, y, z, volume, pitch));
                }
                    break;
                case 0x53:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var header = reader.ReadString();
                    var footer = reader.ReadString();
                    _onplayerlist_header.OnNext(new PacketPlayerlistHeader(header, footer));
                }
                    break;
                case 0x55:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var collectedEntityId = reader.ReadVarInt();
                    var collectorEntityId = reader.ReadVarInt();
                    var pickupItemCount = reader.ReadVarInt();
                    _oncollect.OnNext(new PacketCollect(collectedEntityId, collectorEntityId, pickupItemCount));
                }
                    break;
                case 0x56:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var onGround = reader.ReadBoolean();
                    _onentity_teleport.OnNext(new PacketEntityTeleport(entityId, x, y, z, yaw, pitch, onGround));
                }
                    break;
                case 0x59:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var effectId = reader.ReadSignedByte();
                    var amplifier = reader.ReadSignedByte();
                    var duration = reader.ReadVarInt();
                    var hideParticles = reader.ReadSignedByte();
                    _onentity_effect.OnNext(new PacketEntityEffect(entityId, effectId, amplifier, duration,
                        hideParticles));
                }
                    break;
                case 0x3c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    if (!reader.ReadBoolean())
                    {
                        var id = null;
                    }
                    else
                    {
                        var id = reader.ReadString();
                    }

                    _onselect_advancement_tab.OnNext(new PacketSelectAdvancementTab(id));
                }
                    break;
            }
        }
    }

    public class PacketSpawnEntity
    {
        public PacketSpawnEntity(int entityId, Guid objectUUID, int type, double x, double y, double z, sbyte pitch,
            sbyte yaw, int objectData, short velocityX, short velocityY, short velocityZ)
        {
            EntityId = entityId;
            ObjectUUID = objectUUID;
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Pitch = pitch;
            Yaw = yaw;
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
        public int ObjectData { get; internal set; }
        public short VelocityX { get; internal set; }
        public short VelocityY { get; internal set; }
        public short VelocityZ { get; internal set; }
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

        public int EntityId { get; internal set; }
        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Z { get; internal set; }
        public short Count { get; internal set; }
    }

    public class PacketSpawnEntityLiving
    {
        public PacketSpawnEntityLiving(int entityId, Guid entityUUID, int type, double x, double y, double z, sbyte yaw,
            sbyte pitch, sbyte headPitch, short velocityX, short velocityY, short velocityZ)
        {
            EntityId = entityId;
            EntityUUID = entityUUID;
            Type = type;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            HeadPitch = headPitch;
            VelocityX = velocityX;
            VelocityY = velocityY;
            VelocityZ = velocityZ;
        }

        public int EntityId { get; internal set; }
        public Guid EntityUUID { get; internal set; }
        public int Type { get; internal set; }
        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Z { get; internal set; }
        public sbyte Yaw { get; internal set; }
        public sbyte Pitch { get; internal set; }
        public sbyte HeadPitch { get; internal set; }
        public short VelocityX { get; internal set; }
        public short VelocityY { get; internal set; }
        public short VelocityZ { get; internal set; }
    }

    public class PacketNamedEntitySpawn
    {
        public PacketNamedEntitySpawn(int entityId, Guid playerUUID, double x, double y, double z, sbyte yaw,
            sbyte pitch)
        {
            EntityId = entityId;
            PlayerUUID = playerUUID;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public int EntityId { get; internal set; }
        public Guid PlayerUUID { get; internal set; }
        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Z { get; internal set; }
        public sbyte Yaw { get; internal set; }
        public sbyte Pitch { get; internal set; }
    }

    public class PacketAnimation
    {
        public PacketAnimation(int entityId, byte animation)
        {
            EntityId = entityId;
            Animation = animation;
        }

        public int EntityId { get; internal set; }
        public byte Animation { get; internal set; }
    }

    public class PacketDifficulty
    {
        public PacketDifficulty(byte difficulty, bool difficultyLocked)
        {
            Difficulty = difficulty;
            DifficultyLocked = difficultyLocked;
        }

        public byte Difficulty { get; internal set; }
        public bool DifficultyLocked { get; internal set; }
    }

    public class PacketChat
    {
        public PacketChat(string message, sbyte position, Guid sender)
        {
            Message = message;
            Position = position;
            Sender = sender;
        }

        public string Message { get; internal set; }
        public sbyte Position { get; internal set; }
        public Guid Sender { get; internal set; }
    }

    public class PacketTransaction
    {
        public PacketTransaction(sbyte windowId, short action, bool accepted)
        {
            WindowId = windowId;
            Action = action;
            Accepted = accepted;
        }

        public sbyte WindowId { get; internal set; }
        public short Action { get; internal set; }
        public bool Accepted { get; internal set; }
    }

    public class PacketCloseWindow
    {
        public PacketCloseWindow(byte windowId)
        {
            WindowId = windowId;
        }

        public byte WindowId { get; internal set; }
    }

    public class PacketOpenWindow
    {
        public PacketOpenWindow(int windowId, int inventoryType, string windowTitle)
        {
            WindowId = windowId;
            InventoryType = inventoryType;
            WindowTitle = windowTitle;
        }

        public int WindowId { get; internal set; }
        public int InventoryType { get; internal set; }
        public string WindowTitle { get; internal set; }
    }

    public class PacketCraftProgressBar
    {
        public PacketCraftProgressBar(byte windowId, short property, short value)
        {
            WindowId = windowId;
            Property = property;
            Value = value;
        }

        public byte WindowId { get; internal set; }
        public short Property { get; internal set; }
        public short Value { get; internal set; }
    }

    public class PacketSetCooldown
    {
        public PacketSetCooldown(int itemID, int cooldownTicks)
        {
            ItemID = itemID;
            CooldownTicks = cooldownTicks;
        }

        public int ItemID { get; internal set; }
        public int CooldownTicks { get; internal set; }
    }

    public class PacketNamedSoundEffect
    {
        public PacketNamedSoundEffect(string soundName, int soundCategory, int x, int y, int z, float volume,
            float pitch)
        {
            SoundName = soundName;
            SoundCategory = soundCategory;
            X = x;
            Y = y;
            Z = z;
            Volume = volume;
            Pitch = pitch;
        }

        public string SoundName { get; internal set; }
        public int SoundCategory { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Z { get; internal set; }
        public float Volume { get; internal set; }
        public float Pitch { get; internal set; }
    }

    public class PacketKickDisconnect
    {
        public PacketKickDisconnect(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; internal set; }
    }

    public class PacketEntityStatus
    {
        public PacketEntityStatus(int entityId, sbyte entityStatus)
        {
            EntityId = entityId;
            EntityStatus = entityStatus;
        }

        public int EntityId { get; internal set; }
        public sbyte EntityStatus { get; internal set; }
    }

    public class PacketUnloadChunk
    {
        public PacketUnloadChunk(int chunkX, int chunkZ)
        {
            ChunkX = chunkX;
            ChunkZ = chunkZ;
        }

        public int ChunkX { get; internal set; }
        public int ChunkZ { get; internal set; }
    }

    public class PacketGameStateChange
    {
        public PacketGameStateChange(byte reason, float gameMode)
        {
            Reason = reason;
            GameMode = gameMode;
        }

        public byte Reason { get; internal set; }
        public float GameMode { get; internal set; }
    }

    public class PacketOpenHorseWindow
    {
        public PacketOpenHorseWindow(byte windowId, int nbSlots, int entityId)
        {
            WindowId = windowId;
            NbSlots = nbSlots;
            EntityId = entityId;
        }

        public byte WindowId { get; internal set; }
        public int NbSlots { get; internal set; }
        public int EntityId { get; internal set; }
    }

    public class PacketKeepAlive
    {
        public PacketKeepAlive(long keepAliveId)
        {
            KeepAliveId = keepAliveId;
        }

        public long KeepAliveId { get; internal set; }
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

        public int EntityId { get; internal set; }
        public short DX { get; internal set; }
        public short DY { get; internal set; }
        public short DZ { get; internal set; }
        public bool OnGround { get; internal set; }
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

        public int EntityId { get; internal set; }
        public short DX { get; internal set; }
        public short DY { get; internal set; }
        public short DZ { get; internal set; }
        public sbyte Yaw { get; internal set; }
        public sbyte Pitch { get; internal set; }
        public bool OnGround { get; internal set; }
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

        public int EntityId { get; internal set; }
        public sbyte Yaw { get; internal set; }
        public sbyte Pitch { get; internal set; }
        public bool OnGround { get; internal set; }
    }

    public class PacketEntity
    {
        public PacketEntity(int entityId)
        {
            EntityId = entityId;
        }

        public int EntityId { get; internal set; }
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

        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Z { get; internal set; }
        public float Yaw { get; internal set; }
        public float Pitch { get; internal set; }
    }

    public class PacketOpenBook
    {
        public PacketOpenBook(int hand)
        {
            Hand = hand;
        }

        public int Hand { get; internal set; }
    }

    public class PacketCraftRecipeResponse
    {
        public PacketCraftRecipeResponse(sbyte windowId, string recipe)
        {
            WindowId = windowId;
            Recipe = recipe;
        }

        public sbyte WindowId { get; internal set; }
        public string Recipe { get; internal set; }
    }

    public class PacketAbilities
    {
        public PacketAbilities(sbyte flags, float flyingSpeed, float walkingSpeed)
        {
            Flags = flags;
            FlyingSpeed = flyingSpeed;
            WalkingSpeed = walkingSpeed;
        }

        public sbyte Flags { get; internal set; }
        public float FlyingSpeed { get; internal set; }
        public float WalkingSpeed { get; internal set; }
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

        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Z { get; internal set; }
        public float Yaw { get; internal set; }
        public float Pitch { get; internal set; }
        public sbyte Flags { get; internal set; }
        public int TeleportId { get; internal set; }
    }

    public class PacketEntityDestroy
    {
        public PacketEntityDestroy(int[] entityIds)
        {
            EntityIds = entityIds;
        }

        public int[] EntityIds { get; internal set; }
    }

    public class PacketRemoveEntityEffect
    {
        public PacketRemoveEntityEffect(int entityId, sbyte effectId)
        {
            EntityId = entityId;
            EffectId = effectId;
        }

        public int EntityId { get; internal set; }
        public sbyte EffectId { get; internal set; }
    }

    public class PacketResourcePackSend
    {
        public PacketResourcePackSend(string url, string hash)
        {
            Url = url;
            Hash = hash;
        }

        public string Url { get; internal set; }
        public string Hash { get; internal set; }
    }

    public class PacketEntityHeadRotation
    {
        public PacketEntityHeadRotation(int entityId, sbyte headYaw)
        {
            EntityId = entityId;
            HeadYaw = headYaw;
        }

        public int EntityId { get; internal set; }
        public sbyte HeadYaw { get; internal set; }
    }

    public class PacketCamera
    {
        public PacketCamera(int cameraId)
        {
            CameraId = cameraId;
        }

        public int CameraId { get; internal set; }
    }

    public class PacketHeldItemSlot
    {
        public PacketHeldItemSlot(sbyte slot)
        {
            Slot = slot;
        }

        public sbyte Slot { get; internal set; }
    }

    public class PacketUpdateViewPosition
    {
        public PacketUpdateViewPosition(int chunkX, int chunkZ)
        {
            ChunkX = chunkX;
            ChunkZ = chunkZ;
        }

        public int ChunkX { get; internal set; }
        public int ChunkZ { get; internal set; }
    }

    public class PacketUpdateViewDistance
    {
        public PacketUpdateViewDistance(int viewDistance)
        {
            ViewDistance = viewDistance;
        }

        public int ViewDistance { get; internal set; }
    }

    public class PacketScoreboardDisplayObjective
    {
        public PacketScoreboardDisplayObjective(sbyte position, string name)
        {
            Position = position;
            Name = name;
        }

        public sbyte Position { get; internal set; }
        public string Name { get; internal set; }
    }

    public class PacketAttachEntity
    {
        public PacketAttachEntity(int entityId, int vehicleId)
        {
            EntityId = entityId;
            VehicleId = vehicleId;
        }

        public int EntityId { get; internal set; }
        public int VehicleId { get; internal set; }
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

        public int EntityId { get; internal set; }
        public short VelocityX { get; internal set; }
        public short VelocityY { get; internal set; }
        public short VelocityZ { get; internal set; }
    }

    public class PacketExperience
    {
        public PacketExperience(float experienceBar, int level, int totalExperience)
        {
            ExperienceBar = experienceBar;
            Level = level;
            TotalExperience = totalExperience;
        }

        public float ExperienceBar { get; internal set; }
        public int Level { get; internal set; }
        public int TotalExperience { get; internal set; }
    }

    public class PacketUpdateHealth
    {
        public PacketUpdateHealth(float health, int food, float foodSaturation)
        {
            Health = health;
            Food = food;
            FoodSaturation = foodSaturation;
        }

        public float Health { get; internal set; }
        public int Food { get; internal set; }
        public float FoodSaturation { get; internal set; }
    }

    public class PacketSetPassengers
    {
        public PacketSetPassengers(int entityId, int[] passengers)
        {
            EntityId = entityId;
            Passengers = passengers;
        }

        public int EntityId { get; internal set; }
        public int[] Passengers { get; internal set; }
    }

    public class PacketUpdateTime
    {
        public PacketUpdateTime(long age, long time)
        {
            Age = age;
            Time = time;
        }

        public long Age { get; internal set; }
        public long Time { get; internal set; }
    }

    public class PacketEntitySoundEffect
    {
        public PacketEntitySoundEffect(int soundId, int soundCategory, int entityId, float volume, float pitch)
        {
            SoundId = soundId;
            SoundCategory = soundCategory;
            EntityId = entityId;
            Volume = volume;
            Pitch = pitch;
        }

        public int SoundId { get; internal set; }
        public int SoundCategory { get; internal set; }
        public int EntityId { get; internal set; }
        public float Volume { get; internal set; }
        public float Pitch { get; internal set; }
    }

    public class PacketSoundEffect
    {
        public PacketSoundEffect(int soundId, int soundCategory, int x, int y, int z, float volume, float pitch)
        {
            SoundId = soundId;
            SoundCategory = soundCategory;
            X = x;
            Y = y;
            Z = z;
            Volume = volume;
            Pitch = pitch;
        }

        public int SoundId { get; internal set; }
        public int SoundCategory { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Z { get; internal set; }
        public float Volume { get; internal set; }
        public float Pitch { get; internal set; }
    }

    public class PacketPlayerlistHeader
    {
        public PacketPlayerlistHeader(string header, string footer)
        {
            Header = header;
            Footer = footer;
        }

        public string Header { get; internal set; }
        public string Footer { get; internal set; }
    }

    public class PacketCollect
    {
        public PacketCollect(int collectedEntityId, int collectorEntityId, int pickupItemCount)
        {
            CollectedEntityId = collectedEntityId;
            CollectorEntityId = collectorEntityId;
            PickupItemCount = pickupItemCount;
        }

        public int CollectedEntityId { get; internal set; }
        public int CollectorEntityId { get; internal set; }
        public int PickupItemCount { get; internal set; }
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

        public int EntityId { get; internal set; }
        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double Z { get; internal set; }
        public sbyte Yaw { get; internal set; }
        public sbyte Pitch { get; internal set; }
        public bool OnGround { get; internal set; }
    }

    public class PacketEntityEffect
    {
        public PacketEntityEffect(int entityId, sbyte effectId, sbyte amplifier, int duration, sbyte hideParticles)
        {
            EntityId = entityId;
            EffectId = effectId;
            Amplifier = amplifier;
            Duration = duration;
            HideParticles = hideParticles;
        }

        public int EntityId { get; internal set; }
        public sbyte EffectId { get; internal set; }
        public sbyte Amplifier { get; internal set; }
        public int Duration { get; internal set; }
        public sbyte HideParticles { get; internal set; }
    }

    public class PacketSelectAdvancementTab
    {
        public PacketSelectAdvancementTab(string? id)
        {
            Id = id;
        }

        public string? Id { get; internal set; }
    }
}