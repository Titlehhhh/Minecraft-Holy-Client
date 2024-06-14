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
        private readonly Subject<PacketSpawnEntityPainting> _onspawn_entity_painting = new();
        private readonly Subject<PacketNamedEntitySpawn> _onnamed_entity_spawn = new();
        private readonly Subject<PacketAnimation> _onanimation = new();
        private readonly Subject<PacketBlockBreakAnimation> _onblock_break_animation = new();
        private readonly Subject<PacketBlockAction> _onblock_action = new();
        private readonly Subject<PacketBlockChange> _onblock_change = new();
        private readonly Subject<PacketDifficulty> _ondifficulty = new();
        private readonly Subject<PacketChat> _onchat = new();
        private readonly Subject<PacketTransaction> _ontransaction = new();
        private readonly Subject<PacketCloseWindow> _onclose_window = new();
        private readonly Subject<PacketOpenWindow> _onopen_window = new();
        private readonly Subject<PacketCraftProgressBar> _oncraft_progress_bar = new();
        private readonly Subject<PacketSetSlot> _onset_slot = new();
        private readonly Subject<PacketSetCooldown> _onset_cooldown = new();
        private readonly Subject<PacketCustomPayload> _oncustom_payload = new();
        private readonly Subject<PacketNamedSoundEffect> _onnamed_sound_effect = new();
        private readonly Subject<PacketKickDisconnect> _onkick_disconnect = new();
        private readonly Subject<PacketEntityStatus> _onentity_status = new();
        private readonly Subject<PacketUnloadChunk> _onunload_chunk = new();
        private readonly Subject<PacketGameStateChange> _ongame_state_change = new();
        private readonly Subject<PacketOpenHorseWindow> _onopen_horse_window = new();
        private readonly Subject<PacketKeepAlive> _onkeep_alive = new();
        private readonly Subject<PacketWorldEvent> _onworld_event = new();
        private readonly Subject<PacketUpdateLight> _onupdate_light = new();
        private readonly Subject<PacketRelEntityMove> _onrel_entity_move = new();
        private readonly Subject<PacketEntityMoveLook> _onentity_move_look = new();
        private readonly Subject<PacketEntityLook> _onentity_look = new();
        private readonly Subject<PacketEntity> _onentity = new();
        private readonly Subject<PacketVehicleMove> _onvehicle_move = new();
        private readonly Subject<PacketOpenBook> _onopen_book = new();
        private readonly Subject<PacketOpenSignEntity> _onopen_sign_entity = new();
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
        private readonly Subject<PacketSpawnPosition> _onspawn_position = new();
        private readonly Subject<PacketUpdateTime> _onupdate_time = new();
        private readonly Subject<PacketEntitySoundEffect> _onentity_sound_effect = new();
        private readonly Subject<PacketSoundEffect> _onsound_effect = new();
        private readonly Subject<PacketPlayerlistHeader> _onplayerlist_header = new();
        private readonly Subject<PacketCollect> _oncollect = new();
        private readonly Subject<PacketEntityTeleport> _onentity_teleport = new();
        private readonly Subject<PacketEntityEffect> _onentity_effect = new();
        private readonly Subject<PacketSelectAdvancementTab> _onselect_advancement_tab = new();
        private readonly Subject<PacketAcknowledgePlayerDigging> _onacknowledge_player_digging = new();

        public IObservable<PacketSpawnEntity> OnSpawnEntityPacket => _onspawn_entity;

        public IObservable<PacketSpawnEntityExperienceOrb> OnSpawnEntityExperienceOrbPacket =>
            _onspawn_entity_experience_orb;

        public IObservable<PacketSpawnEntityLiving> OnSpawnEntityLivingPacket => _onspawn_entity_living;
        public IObservable<PacketSpawnEntityPainting> OnSpawnEntityPaintingPacket => _onspawn_entity_painting;
        public IObservable<PacketNamedEntitySpawn> OnNamedEntitySpawnPacket => _onnamed_entity_spawn;
        public IObservable<PacketAnimation> OnAnimationPacket => _onanimation;
        public IObservable<PacketBlockBreakAnimation> OnBlockBreakAnimationPacket => _onblock_break_animation;
        public IObservable<PacketBlockAction> OnBlockActionPacket => _onblock_action;
        public IObservable<PacketBlockChange> OnBlockChangePacket => _onblock_change;
        public IObservable<PacketDifficulty> OnDifficultyPacket => _ondifficulty;
        public IObservable<PacketChat> OnChatPacket => _onchat;
        public IObservable<PacketTransaction> OnTransactionPacket => _ontransaction;
        public IObservable<PacketCloseWindow> OnCloseWindowPacket => _onclose_window;
        public IObservable<PacketOpenWindow> OnOpenWindowPacket => _onopen_window;
        public IObservable<PacketCraftProgressBar> OnCraftProgressBarPacket => _oncraft_progress_bar;
        public IObservable<PacketSetSlot> OnSetSlotPacket => _onset_slot;
        public IObservable<PacketSetCooldown> OnSetCooldownPacket => _onset_cooldown;
        public IObservable<PacketCustomPayload> OnCustomPayloadPacket => _oncustom_payload;
        public IObservable<PacketNamedSoundEffect> OnNamedSoundEffectPacket => _onnamed_sound_effect;
        public IObservable<PacketKickDisconnect> OnKickDisconnectPacket => _onkick_disconnect;
        public IObservable<PacketEntityStatus> OnEntityStatusPacket => _onentity_status;
        public IObservable<PacketUnloadChunk> OnUnloadChunkPacket => _onunload_chunk;
        public IObservable<PacketGameStateChange> OnGameStateChangePacket => _ongame_state_change;
        public IObservable<PacketOpenHorseWindow> OnOpenHorseWindowPacket => _onopen_horse_window;
        public IObservable<PacketKeepAlive> OnKeepAlivePacket => _onkeep_alive;
        public IObservable<PacketWorldEvent> OnWorldEventPacket => _onworld_event;
        public IObservable<PacketUpdateLight> OnUpdateLightPacket => _onupdate_light;
        public IObservable<PacketRelEntityMove> OnRelEntityMovePacket => _onrel_entity_move;
        public IObservable<PacketEntityMoveLook> OnEntityMoveLookPacket => _onentity_move_look;
        public IObservable<PacketEntityLook> OnEntityLookPacket => _onentity_look;
        public IObservable<PacketEntity> OnEntityPacket => _onentity;
        public IObservable<PacketVehicleMove> OnVehicleMovePacket => _onvehicle_move;
        public IObservable<PacketOpenBook> OnOpenBookPacket => _onopen_book;
        public IObservable<PacketOpenSignEntity> OnOpenSignEntityPacket => _onopen_sign_entity;
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
        public IObservable<PacketSpawnPosition> OnSpawnPositionPacket => _onspawn_position;
        public IObservable<PacketUpdateTime> OnUpdateTimePacket => _onupdate_time;
        public IObservable<PacketEntitySoundEffect> OnEntitySoundEffectPacket => _onentity_sound_effect;
        public IObservable<PacketSoundEffect> OnSoundEffectPacket => _onsound_effect;
        public IObservable<PacketPlayerlistHeader> OnPlayerlistHeaderPacket => _onplayerlist_header;
        public IObservable<PacketCollect> OnCollectPacket => _oncollect;
        public IObservable<PacketEntityTeleport> OnEntityTeleportPacket => _onentity_teleport;
        public IObservable<PacketEntityEffect> OnEntityEffectPacket => _onentity_effect;
        public IObservable<PacketSelectAdvancementTab> OnSelectAdvancementTabPacket => _onselect_advancement_tab;

        public IObservable<PacketAcknowledgePlayerDigging> OnAcknowledgePlayerDiggingPacket =>
            _onacknowledge_player_digging;

        public Task SendTeleportConfirm(int teleportId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x00);
            writer.WriteVarInt(teleportId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendQueryBlockNbt(int transactionId, Position location)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x01);
            writer.WriteVarInt(transactionId);
            writer.WritePosition(location);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetDifficulty(byte newDifficulty)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x02);
            writer.WriteUnsignedByte(newDifficulty);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEditBook(Slot? new_book, bool signing, int hand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0c);
            writer.WriteSlot(new_book);
            writer.WriteBoolean(signing);
            writer.WriteVarInt(hand);
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

        public Task SendUpdateCommandBlock(Position location, string command, int mode, byte flags)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x26);
            writer.WritePosition(location);
            writer.WriteString(command);
            writer.WriteVarInt(mode);
            writer.WriteUnsignedByte(flags);
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

        public Task SendUpdateStructureBlock(Position location, int action, int mode, string name, sbyte offset_x,
            sbyte offset_y, sbyte offset_z, sbyte size_x, sbyte size_y, sbyte size_z, int mirror, int rotation,
            string metadata, float integrity, long seed, byte flags)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2a);
            writer.WritePosition(location);
            writer.WriteVarInt(action);
            writer.WriteVarInt(mode);
            writer.WriteString(name);
            writer.WriteSignedByte(offset_x);
            writer.WriteSignedByte(offset_y);
            writer.WriteSignedByte(offset_z);
            writer.WriteSignedByte(size_x);
            writer.WriteSignedByte(size_y);
            writer.WriteSignedByte(size_z);
            writer.WriteVarInt(mirror);
            writer.WriteVarInt(rotation);
            writer.WriteString(metadata);
            writer.WriteFloat(integrity);
            writer.WriteVarLong(seed);
            writer.WriteUnsignedByte(flags);
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

        public Task SendWindowClick(byte windowId, short slot, sbyte mouseButton, short action, sbyte mode, Slot? item)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x09);
            writer.WriteUnsignedByte(windowId);
            writer.WriteSignedShort(slot);
            writer.WriteSignedByte(mouseButton);
            writer.WriteSignedShort(action);
            writer.WriteSignedByte(mode);
            writer.WriteSlot(item);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCloseWindow(byte windowId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0a);
            writer.WriteUnsignedByte(windowId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCustomPayload(string channel, byte[] data)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0b);
            writer.WriteString(channel);
            writer.WriteBuffer(data);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendGenerateStructure(Position location, int levels, bool keepJigsaws)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0f);
            writer.WritePosition(location);
            writer.WriteVarInt(levels);
            writer.WriteBoolean(keepJigsaws);
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

        public Task SendBlockDig(int status, Position location, sbyte face)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1b);
            writer.WriteVarInt(status);
            writer.WritePosition(location);
            writer.WriteSignedByte(face);
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

        public Task SendSetCreativeSlot(short slot, Slot? item)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x28);
            writer.WriteSignedShort(slot);
            writer.WriteSlot(item);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUpdateJigsawBlock(Position location, string name, string target, string pool, string finalState,
            string jointType)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x29);
            writer.WritePosition(location);
            writer.WriteString(name);
            writer.WriteString(target);
            writer.WriteString(pool);
            writer.WriteString(finalState);
            writer.WriteString(jointType);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUpdateSign(Position location, string text1, string text2, string text3, string text4)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2b);
            writer.WritePosition(location);
            writer.WriteString(text1);
            writer.WriteString(text2);
            writer.WriteString(text3);
            writer.WriteString(text4);
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

        public Task SendBlockPlace(int hand, Position location, int direction, float cursorX, float cursorY,
            float cursorZ, bool insideBlock)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2e);
            writer.WriteVarInt(hand);
            writer.WritePosition(location);
            writer.WriteVarInt(direction);
            writer.WriteFloat(cursorX);
            writer.WriteFloat(cursorY);
            writer.WriteFloat(cursorZ);
            writer.WriteBoolean(insideBlock);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUseItem(int hand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2f);
            writer.WriteVarInt(hand);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        protected override void OnPacketReceived(InputPacket packet)
        {
            switch (packet.Id)
            {
                case 0x00:
                    if (_onspawn_entity.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        Guid objectUUID = reader.ReadUUID();
                        int type = reader.ReadVarInt();
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        sbyte pitch = reader.ReadSignedByte();
                        sbyte yaw = reader.ReadSignedByte();
                        int objectData = reader.ReadSignedInt();
                        short velocityX = reader.ReadSignedShort();
                        short velocityY = reader.ReadSignedShort();
                        short velocityZ = reader.ReadSignedShort();
                        _onspawn_entity.OnNext(new PacketSpawnEntity(entityId, objectUUID, type, x, y, z, pitch, yaw,
                            objectData, velocityX, velocityY, velocityZ));
                    }

                    break;
                case 0x01:
                    if (_onspawn_entity_experience_orb.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        short count = reader.ReadSignedShort();
                        _onspawn_entity_experience_orb.OnNext(
                            new PacketSpawnEntityExperienceOrb(entityId, x, y, z, count));
                    }

                    break;
                case 0x02:
                    if (_onspawn_entity_living.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        Guid entityUUID = reader.ReadUUID();
                        int type = reader.ReadVarInt();
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        sbyte yaw = reader.ReadSignedByte();
                        sbyte pitch = reader.ReadSignedByte();
                        sbyte headPitch = reader.ReadSignedByte();
                        short velocityX = reader.ReadSignedShort();
                        short velocityY = reader.ReadSignedShort();
                        short velocityZ = reader.ReadSignedShort();
                        _onspawn_entity_living.OnNext(new PacketSpawnEntityLiving(entityId, entityUUID, type, x, y, z,
                            yaw, pitch, headPitch, velocityX, velocityY, velocityZ));
                    }

                    break;
                case 0x03:
                    if (_onspawn_entity_painting.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        Guid entityUUID = reader.ReadUUID();
                        int title = reader.ReadVarInt();
                        Position location = reader.ReadPosition();
                        byte direction = reader.ReadUnsignedByte();
                        _onspawn_entity_painting.OnNext(new PacketSpawnEntityPainting(entityId, entityUUID, title,
                            location, direction));
                    }

                    break;
                case 0x04:
                    if (_onnamed_entity_spawn.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        Guid playerUUID = reader.ReadUUID();
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        sbyte yaw = reader.ReadSignedByte();
                        sbyte pitch = reader.ReadSignedByte();
                        _onnamed_entity_spawn.OnNext(new PacketNamedEntitySpawn(entityId, playerUUID, x, y, z, yaw,
                            pitch));
                    }

                    break;
                case 0x05:
                    if (_onanimation.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        byte animation = reader.ReadUnsignedByte();
                        _onanimation.OnNext(new PacketAnimation(entityId, animation));
                    }

                    break;
                case 0x08:
                    if (_onblock_break_animation.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        Position location = reader.ReadPosition();
                        sbyte destroyStage = reader.ReadSignedByte();
                        _onblock_break_animation.OnNext(new PacketBlockBreakAnimation(entityId, location,
                            destroyStage));
                    }

                    break;
                case 0x0a:
                    if (_onblock_action.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        Position location = reader.ReadPosition();
                        byte byte1 = reader.ReadUnsignedByte();
                        byte byte2 = reader.ReadUnsignedByte();
                        int blockId = reader.ReadVarInt();
                        _onblock_action.OnNext(new PacketBlockAction(location, byte1, byte2, blockId));
                    }

                    break;
                case 0x0b:
                    if (_onblock_change.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        Position location = reader.ReadPosition();
                        int type = reader.ReadVarInt();
                        _onblock_change.OnNext(new PacketBlockChange(location, type));
                    }

                    break;
                case 0x0d:
                    if (_ondifficulty.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        byte difficulty = reader.ReadUnsignedByte();
                        bool difficultyLocked = reader.ReadBoolean();
                        _ondifficulty.OnNext(new PacketDifficulty(difficulty, difficultyLocked));
                    }

                    break;
                case 0x0e:
                    if (_onchat.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string message = reader.ReadString();
                        sbyte position = reader.ReadSignedByte();
                        Guid sender = reader.ReadUUID();
                        _onchat.OnNext(new PacketChat(message, position, sender));
                    }

                    break;
                case 0x11:
                    if (_ontransaction.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        sbyte windowId = reader.ReadSignedByte();
                        short action = reader.ReadSignedShort();
                        bool accepted = reader.ReadBoolean();
                        _ontransaction.OnNext(new PacketTransaction(windowId, action, accepted));
                    }

                    break;
                case 0x12:
                    if (_onclose_window.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        byte windowId = reader.ReadUnsignedByte();
                        _onclose_window.OnNext(new PacketCloseWindow(windowId));
                    }

                    break;
                case 0x2d:
                    if (_onopen_window.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int windowId = reader.ReadVarInt();
                        int inventoryType = reader.ReadVarInt();
                        string windowTitle = reader.ReadString();
                        _onopen_window.OnNext(new PacketOpenWindow(windowId, inventoryType, windowTitle));
                    }

                    break;
                case 0x14:
                    if (_oncraft_progress_bar.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        byte windowId = reader.ReadUnsignedByte();
                        short property = reader.ReadSignedShort();
                        short value = reader.ReadSignedShort();
                        _oncraft_progress_bar.OnNext(new PacketCraftProgressBar(windowId, property, value));
                    }

                    break;
                case 0x15:
                    if (_onset_slot.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        sbyte windowId = reader.ReadSignedByte();
                        short slot = reader.ReadSignedShort();
                        Slot? item = reader.ReadSlot();
                        _onset_slot.OnNext(new PacketSetSlot(windowId, slot, item));
                    }

                    break;
                case 0x16:
                    if (_onset_cooldown.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int itemID = reader.ReadVarInt();
                        int cooldownTicks = reader.ReadVarInt();
                        _onset_cooldown.OnNext(new PacketSetCooldown(itemID, cooldownTicks));
                    }

                    break;
                case 0x17:
                    if (_oncustom_payload.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string channel = reader.ReadString();
                        byte[] data = reader.ReadRestBuffer();
                        _oncustom_payload.OnNext(new PacketCustomPayload(channel, data));
                    }

                    break;
                case 0x18:
                    if (_onnamed_sound_effect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string soundName = reader.ReadString();
                        int soundCategory = reader.ReadVarInt();
                        int x = reader.ReadSignedInt();
                        int y = reader.ReadSignedInt();
                        int z = reader.ReadSignedInt();
                        float volume = reader.ReadFloat();
                        float pitch = reader.ReadFloat();
                        _onnamed_sound_effect.OnNext(new PacketNamedSoundEffect(soundName, soundCategory, x, y, z,
                            volume, pitch));
                    }

                    break;
                case 0x19:
                    if (_onkick_disconnect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string reason = reader.ReadString();
                        _onkick_disconnect.OnNext(new PacketKickDisconnect(reason));
                    }

                    break;
                case 0x1a:
                    if (_onentity_status.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadSignedInt();
                        sbyte entityStatus = reader.ReadSignedByte();
                        _onentity_status.OnNext(new PacketEntityStatus(entityId, entityStatus));
                    }

                    break;
                case 0x1c:
                    if (_onunload_chunk.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int chunkX = reader.ReadSignedInt();
                        int chunkZ = reader.ReadSignedInt();
                        _onunload_chunk.OnNext(new PacketUnloadChunk(chunkX, chunkZ));
                    }

                    break;
                case 0x1d:
                    if (_ongame_state_change.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        byte reason = reader.ReadUnsignedByte();
                        float gameMode = reader.ReadFloat();
                        _ongame_state_change.OnNext(new PacketGameStateChange(reason, gameMode));
                    }

                    break;
                case 0x1e:
                    if (_onopen_horse_window.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        byte windowId = reader.ReadUnsignedByte();
                        int nbSlots = reader.ReadVarInt();
                        int entityId = reader.ReadSignedInt();
                        _onopen_horse_window.OnNext(new PacketOpenHorseWindow(windowId, nbSlots, entityId));
                    }

                    break;
                case 0x1f:
                    if (_onkeep_alive.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        long keepAliveId = reader.ReadSignedLong();
                        _onkeep_alive.OnNext(new PacketKeepAlive(keepAliveId));
                    }

                    break;
                case 0x21:
                    if (_onworld_event.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int effectId = reader.ReadSignedInt();
                        Position location = reader.ReadPosition();
                        int data = reader.ReadSignedInt();
                        bool global = reader.ReadBoolean();
                        _onworld_event.OnNext(new PacketWorldEvent(effectId, location, data, global));
                    }

                    break;
                case 0x23:
                    if (_onupdate_light.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int chunkX = reader.ReadVarInt();
                        int chunkZ = reader.ReadVarInt();
                        bool trustEdges = reader.ReadBoolean();
                        int skyLightMask = reader.ReadVarInt();
                        int blockLightMask = reader.ReadVarInt();
                        int emptySkyLightMask = reader.ReadVarInt();
                        int emptyBlockLightMask = reader.ReadVarInt();
                        byte[] data = reader.ReadRestBuffer();
                        _onupdate_light.OnNext(new PacketUpdateLight(chunkX, chunkZ, trustEdges, skyLightMask,
                            blockLightMask, emptySkyLightMask, emptyBlockLightMask, data));
                    }

                    break;
                case 0x27:
                    if (_onrel_entity_move.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        short dX = reader.ReadSignedShort();
                        short dY = reader.ReadSignedShort();
                        short dZ = reader.ReadSignedShort();
                        bool onGround = reader.ReadBoolean();
                        _onrel_entity_move.OnNext(new PacketRelEntityMove(entityId, dX, dY, dZ, onGround));
                    }

                    break;
                case 0x28:
                    if (_onentity_move_look.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        short dX = reader.ReadSignedShort();
                        short dY = reader.ReadSignedShort();
                        short dZ = reader.ReadSignedShort();
                        sbyte yaw = reader.ReadSignedByte();
                        sbyte pitch = reader.ReadSignedByte();
                        bool onGround = reader.ReadBoolean();
                        _onentity_move_look.OnNext(new PacketEntityMoveLook(entityId, dX, dY, dZ, yaw, pitch,
                            onGround));
                    }

                    break;
                case 0x29:
                    if (_onentity_look.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        sbyte yaw = reader.ReadSignedByte();
                        sbyte pitch = reader.ReadSignedByte();
                        bool onGround = reader.ReadBoolean();
                        _onentity_look.OnNext(new PacketEntityLook(entityId, yaw, pitch, onGround));
                    }

                    break;
                case 0x2a:
                    if (_onentity.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        _onentity.OnNext(new PacketEntity(entityId));
                    }

                    break;
                case 0x2b:
                    if (_onvehicle_move.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        float yaw = reader.ReadFloat();
                        float pitch = reader.ReadFloat();
                        _onvehicle_move.OnNext(new PacketVehicleMove(x, y, z, yaw, pitch));
                    }

                    break;
                case 0x2c:
                    if (_onopen_book.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int hand = reader.ReadVarInt();
                        _onopen_book.OnNext(new PacketOpenBook(hand));
                    }

                    break;
                case 0x2e:
                    if (_onopen_sign_entity.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        Position location = reader.ReadPosition();
                        _onopen_sign_entity.OnNext(new PacketOpenSignEntity(location));
                    }

                    break;
                case 0x2f:
                    if (_oncraft_recipe_response.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        sbyte windowId = reader.ReadSignedByte();
                        string recipe = reader.ReadString();
                        _oncraft_recipe_response.OnNext(new PacketCraftRecipeResponse(windowId, recipe));
                    }

                    break;
                case 0x30:
                    if (_onabilities.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        sbyte flags = reader.ReadSignedByte();
                        float flyingSpeed = reader.ReadFloat();
                        float walkingSpeed = reader.ReadFloat();
                        _onabilities.OnNext(new PacketAbilities(flags, flyingSpeed, walkingSpeed));
                    }

                    break;
                case 0x34:
                    if (_onposition.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        float yaw = reader.ReadFloat();
                        float pitch = reader.ReadFloat();
                        sbyte flags = reader.ReadSignedByte();
                        int teleportId = reader.ReadVarInt();
                        _onposition.OnNext(new PacketPosition(x, y, z, yaw, pitch, flags, teleportId));
                    }

                    break;
                case 0x36:
                    if (_onentity_destroy.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        var tempArrayLength_0_0 = reader.ReadVarInt();
                        var tempArray_0_0 = new int[tempArrayLength_0_0];
                        for (int i_0_0 = 0; i_0_0 < tempArrayLength_0_0; i_0_0++)
                        {
                            var for_item_0_0 = reader.ReadVarInt();
                            tempArray_0_0[i_0_0] = for_item_0_0;
                        }

                        int[] entityIds = tempArray_0_0;
                        _onentity_destroy.OnNext(new PacketEntityDestroy(entityIds));
                    }

                    break;
                case 0x37:
                    if (_onremove_entity_effect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        sbyte effectId = reader.ReadSignedByte();
                        _onremove_entity_effect.OnNext(new PacketRemoveEntityEffect(entityId, effectId));
                    }

                    break;
                case 0x38:
                    if (_onresource_pack_send.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string url = reader.ReadString();
                        string hash = reader.ReadString();
                        _onresource_pack_send.OnNext(new PacketResourcePackSend(url, hash));
                    }

                    break;
                case 0x3a:
                    if (_onentity_head_rotation.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        sbyte headYaw = reader.ReadSignedByte();
                        _onentity_head_rotation.OnNext(new PacketEntityHeadRotation(entityId, headYaw));
                    }

                    break;
                case 0x3e:
                    if (_oncamera.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int cameraId = reader.ReadVarInt();
                        _oncamera.OnNext(new PacketCamera(cameraId));
                    }

                    break;
                case 0x3f:
                    if (_onheld_item_slot.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        sbyte slot = reader.ReadSignedByte();
                        _onheld_item_slot.OnNext(new PacketHeldItemSlot(slot));
                    }

                    break;
                case 0x40:
                    if (_onupdate_view_position.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int chunkX = reader.ReadVarInt();
                        int chunkZ = reader.ReadVarInt();
                        _onupdate_view_position.OnNext(new PacketUpdateViewPosition(chunkX, chunkZ));
                    }

                    break;
                case 0x41:
                    if (_onupdate_view_distance.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int viewDistance = reader.ReadVarInt();
                        _onupdate_view_distance.OnNext(new PacketUpdateViewDistance(viewDistance));
                    }

                    break;
                case 0x43:
                    if (_onscoreboard_display_objective.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        sbyte position = reader.ReadSignedByte();
                        string name = reader.ReadString();
                        _onscoreboard_display_objective.OnNext(new PacketScoreboardDisplayObjective(position, name));
                    }

                    break;
                case 0x45:
                    if (_onattach_entity.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadSignedInt();
                        int vehicleId = reader.ReadSignedInt();
                        _onattach_entity.OnNext(new PacketAttachEntity(entityId, vehicleId));
                    }

                    break;
                case 0x46:
                    if (_onentity_velocity.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        short velocityX = reader.ReadSignedShort();
                        short velocityY = reader.ReadSignedShort();
                        short velocityZ = reader.ReadSignedShort();
                        _onentity_velocity.OnNext(new PacketEntityVelocity(entityId, velocityX, velocityY, velocityZ));
                    }

                    break;
                case 0x48:
                    if (_onexperience.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        float experienceBar = reader.ReadFloat();
                        int level = reader.ReadVarInt();
                        int totalExperience = reader.ReadVarInt();
                        _onexperience.OnNext(new PacketExperience(experienceBar, level, totalExperience));
                    }

                    break;
                case 0x49:
                    if (_onupdate_health.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        float health = reader.ReadFloat();
                        int food = reader.ReadVarInt();
                        float foodSaturation = reader.ReadFloat();
                        _onupdate_health.OnNext(new PacketUpdateHealth(health, food, foodSaturation));
                    }

                    break;
                case 0x4b:
                    if (_onset_passengers.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        var tempArrayLength_1_0 = reader.ReadVarInt();
                        var tempArray_1_0 = new int[tempArrayLength_1_0];
                        for (int i_1_0 = 0; i_1_0 < tempArrayLength_1_0; i_1_0++)
                        {
                            var for_item_1_0 = reader.ReadVarInt();
                            tempArray_1_0[i_1_0] = for_item_1_0;
                        }

                        int[] passengers = tempArray_1_0;
                        _onset_passengers.OnNext(new PacketSetPassengers(entityId, passengers));
                    }

                    break;
                case 0x42:
                    if (_onspawn_position.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        Position location = reader.ReadPosition();
                        _onspawn_position.OnNext(new PacketSpawnPosition(location));
                    }

                    break;
                case 0x4e:
                    if (_onupdate_time.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        long age = reader.ReadSignedLong();
                        long time = reader.ReadSignedLong();
                        _onupdate_time.OnNext(new PacketUpdateTime(age, time));
                    }

                    break;
                case 0x50:
                    if (_onentity_sound_effect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int soundId = reader.ReadVarInt();
                        int soundCategory = reader.ReadVarInt();
                        int entityId = reader.ReadVarInt();
                        float volume = reader.ReadFloat();
                        float pitch = reader.ReadFloat();
                        _onentity_sound_effect.OnNext(new PacketEntitySoundEffect(soundId, soundCategory, entityId,
                            volume, pitch));
                    }

                    break;
                case 0x51:
                    if (_onsound_effect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int soundId = reader.ReadVarInt();
                        int soundCategory = reader.ReadVarInt();
                        int x = reader.ReadSignedInt();
                        int y = reader.ReadSignedInt();
                        int z = reader.ReadSignedInt();
                        float volume = reader.ReadFloat();
                        float pitch = reader.ReadFloat();
                        _onsound_effect.OnNext(new PacketSoundEffect(soundId, soundCategory, x, y, z, volume, pitch));
                    }

                    break;
                case 0x53:
                    if (_onplayerlist_header.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string header = reader.ReadString();
                        string footer = reader.ReadString();
                        _onplayerlist_header.OnNext(new PacketPlayerlistHeader(header, footer));
                    }

                    break;
                case 0x55:
                    if (_oncollect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int collectedEntityId = reader.ReadVarInt();
                        int collectorEntityId = reader.ReadVarInt();
                        int pickupItemCount = reader.ReadVarInt();
                        _oncollect.OnNext(new PacketCollect(collectedEntityId, collectorEntityId, pickupItemCount));
                    }

                    break;
                case 0x56:
                    if (_onentity_teleport.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        double x = reader.ReadDouble();
                        double y = reader.ReadDouble();
                        double z = reader.ReadDouble();
                        sbyte yaw = reader.ReadSignedByte();
                        sbyte pitch = reader.ReadSignedByte();
                        bool onGround = reader.ReadBoolean();
                        _onentity_teleport.OnNext(new PacketEntityTeleport(entityId, x, y, z, yaw, pitch, onGround));
                    }

                    break;
                case 0x59:
                    if (_onentity_effect.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        int entityId = reader.ReadVarInt();
                        sbyte effectId = reader.ReadSignedByte();
                        sbyte amplifier = reader.ReadSignedByte();
                        int duration = reader.ReadVarInt();
                        sbyte hideParticles = reader.ReadSignedByte();
                        _onentity_effect.OnNext(new PacketEntityEffect(entityId, effectId, amplifier, duration,
                            hideParticles));
                    }

                    break;
                case 0x3c:
                    if (_onselect_advancement_tab.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        string? id = null;
                        if (reader.ReadBoolean())
                        {
                            id = reader.ReadString();
                        }

                        _onselect_advancement_tab.OnNext(new PacketSelectAdvancementTab(id));
                    }

                    break;
                case 0x07:
                    if (_onacknowledge_player_digging.HasObservers)
                    {
                        scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                        Position location = reader.ReadPosition();
                        int block = reader.ReadVarInt();
                        int status = reader.ReadVarInt();
                        bool successful = reader.ReadBoolean();
                        _onacknowledge_player_digging.OnNext(
                            new PacketAcknowledgePlayerDigging(location, block, status, successful));
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

    public class PacketSpawnEntityPainting
    {
        public PacketSpawnEntityPainting(int entityId, Guid entityUUID, int title, Position location, byte direction)
        {
            EntityId = entityId;
            EntityUUID = entityUUID;
            Title = title;
            Location = location;
            Direction = direction;
        }

        public int EntityId { get; internal set; }
        public Guid EntityUUID { get; internal set; }
        public int Title { get; internal set; }
        public Position Location { get; internal set; }
        public byte Direction { get; internal set; }
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

    public class PacketBlockBreakAnimation
    {
        public PacketBlockBreakAnimation(int entityId, Position location, sbyte destroyStage)
        {
            EntityId = entityId;
            Location = location;
            DestroyStage = destroyStage;
        }

        public int EntityId { get; internal set; }
        public Position Location { get; internal set; }
        public sbyte DestroyStage { get; internal set; }
    }

    public class PacketBlockAction
    {
        public PacketBlockAction(Position location, byte byte1, byte byte2, int blockId)
        {
            Location = location;
            Byte1 = byte1;
            Byte2 = byte2;
            BlockId = blockId;
        }

        public Position Location { get; internal set; }
        public byte Byte1 { get; internal set; }
        public byte Byte2 { get; internal set; }
        public int BlockId { get; internal set; }
    }

    public class PacketBlockChange
    {
        public PacketBlockChange(Position location, int type)
        {
            Location = location;
            Type = type;
        }

        public Position Location { get; internal set; }
        public int Type { get; internal set; }
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

    public class PacketSetSlot
    {
        public PacketSetSlot(sbyte windowId, short slot, Slot? item)
        {
            WindowId = windowId;
            Slot = slot;
            Item = item;
        }

        public sbyte WindowId { get; internal set; }
        public short Slot { get; internal set; }
        public Slot? Item { get; internal set; }
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

    public class PacketCustomPayload
    {
        public PacketCustomPayload(string channel, byte[] data)
        {
            Channel = channel;
            Data = data;
        }

        public string Channel { get; internal set; }
        public byte[] Data { get; internal set; }
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

    public class PacketWorldEvent
    {
        public PacketWorldEvent(int effectId, Position location, int data, bool global)
        {
            EffectId = effectId;
            Location = location;
            Data = data;
            Global = global;
        }

        public int EffectId { get; internal set; }
        public Position Location { get; internal set; }
        public int Data { get; internal set; }
        public bool Global { get; internal set; }
    }

    public class PacketUpdateLight
    {
        public PacketUpdateLight(int chunkX, int chunkZ, bool trustEdges, int skyLightMask, int blockLightMask,
            int emptySkyLightMask, int emptyBlockLightMask, byte[] data)
        {
            ChunkX = chunkX;
            ChunkZ = chunkZ;
            TrustEdges = trustEdges;
            SkyLightMask = skyLightMask;
            BlockLightMask = blockLightMask;
            EmptySkyLightMask = emptySkyLightMask;
            EmptyBlockLightMask = emptyBlockLightMask;
            Data = data;
        }

        public int ChunkX { get; internal set; }
        public int ChunkZ { get; internal set; }
        public bool TrustEdges { get; internal set; }
        public int SkyLightMask { get; internal set; }
        public int BlockLightMask { get; internal set; }
        public int EmptySkyLightMask { get; internal set; }
        public int EmptyBlockLightMask { get; internal set; }
        public byte[] Data { get; internal set; }
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

    public class PacketOpenSignEntity
    {
        public PacketOpenSignEntity(Position location)
        {
            Location = location;
        }

        public Position Location { get; internal set; }
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

    public class PacketSpawnPosition
    {
        public PacketSpawnPosition(Position location)
        {
            Location = location;
        }

        public Position Location { get; internal set; }
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

    public class PacketAcknowledgePlayerDigging
    {
        public PacketAcknowledgePlayerDigging(Position location, int block, int status, bool successful)
        {
            Location = location;
            Block = block;
            Status = status;
            Successful = successful;
        }

        public Position Location { get; internal set; }
        public int Block { get; internal set; }
        public int Status { get; internal set; }
        public bool Successful { get; internal set; }
    }
}