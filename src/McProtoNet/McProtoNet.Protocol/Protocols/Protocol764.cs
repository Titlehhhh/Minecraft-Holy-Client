using System.Reactive.Subjects;
using McProtoNet.Abstractions;
using McProtoNet.NBT;
using McProtoNet.Protocol;
using McProtoNet.Serialization;

namespace McProtoNet.Protocol764;

public sealed class Protocol_764 : ProtocolBase
{
    private readonly Subject<PacketAbilities> _onabilities = new();
    private readonly Subject<PacketAcknowledgePlayerDigging> _onacknowledge_player_digging = new();
    private readonly Subject<PacketActionBar> _onaction_bar = new();
    private readonly Subject<PacketAnimation> _onanimation = new();
    private readonly Subject<PacketAttachEntity> _onattach_entity = new();
    private readonly Subject<PacketBlockAction> _onblock_action = new();
    private readonly Subject<PacketBlockBreakAnimation> _onblock_break_animation = new();
    private readonly Subject<PacketBlockChange> _onblock_change = new();
    private readonly Subject<PacketCamera> _oncamera = new();
    private readonly Subject<PacketChatSuggestions> _onchat_suggestions = new();
    private readonly Subject<PacketChunkBatchFinished> _onchunk_batch_finished = new();
    private readonly Subject<PacketChunkBatchStart> _onchunk_batch_start = new();
    private readonly Subject<PacketClearTitles> _onclear_titles = new();
    private readonly Subject<PacketCloseWindow> _onclose_window = new();
    private readonly Subject<PacketCollect> _oncollect = new();
    private readonly Subject<PacketCraftProgressBar> _oncraft_progress_bar = new();
    private readonly Subject<PacketCraftRecipeResponse> _oncraft_recipe_response = new();
    private readonly Subject<PacketCustomPayload> _oncustom_payload = new();
    private readonly Subject<PacketDeathCombatEvent> _ondeath_combat_event = new();
    private readonly Subject<PacketDifficulty> _ondifficulty = new();
    private readonly Subject<PacketEndCombatEvent> _onend_combat_event = new();
    private readonly Subject<PacketEnterCombatEvent> _onenter_combat_event = new();
    private readonly Subject<PacketEntityDestroy> _onentity_destroy = new();
    private readonly Subject<PacketEntityHeadRotation> _onentity_head_rotation = new();
    private readonly Subject<PacketEntityLook> _onentity_look = new();
    private readonly Subject<PacketEntityMoveLook> _onentity_move_look = new();
    private readonly Subject<PacketEntityStatus> _onentity_status = new();
    private readonly Subject<PacketEntityTeleport> _onentity_teleport = new();
    private readonly Subject<PacketEntityVelocity> _onentity_velocity = new();
    private readonly Subject<PacketExperience> _onexperience = new();
    private readonly Subject<PacketGameStateChange> _ongame_state_change = new();
    private readonly Subject<PacketHeldItemSlot> _onheld_item_slot = new();
    private readonly Subject<PacketHurtAnimation> _onhurt_animation = new();
    private readonly Subject<PacketInitializeWorldBorder> _oninitialize_world_border = new();
    private readonly Subject<PacketKeepAlive> _onkeep_alive = new();
    private readonly Subject<PacketKickDisconnect> _onkick_disconnect = new();
    private readonly Subject<PacketNbtQueryResponse> _onnbt_query_response = new();
    private readonly Subject<PacketOpenBook> _onopen_book = new();
    private readonly Subject<PacketOpenHorseWindow> _onopen_horse_window = new();
    private readonly Subject<PacketOpenSignEntity> _onopen_sign_entity = new();
    private readonly Subject<PacketOpenWindow> _onopen_window = new();
    private readonly Subject<PacketPing> _onping = new();
    private readonly Subject<PacketPingResponse> _onping_response = new();
    private readonly Subject<PacketPlayerlistHeader> _onplayerlist_header = new();
    private readonly Subject<PacketPosition> _onposition = new();
    private readonly Subject<PacketProfilelessChat> _onprofileless_chat = new();
    private readonly Subject<PacketRelEntityMove> _onrel_entity_move = new();
    private readonly Subject<PacketRemoveEntityEffect> _onremove_entity_effect = new();
    private readonly Subject<PacketResourcePackSend> _onresource_pack_send = new();
    private readonly Subject<PacketScoreboardDisplayObjective> _onscoreboard_display_objective = new();
    private readonly Subject<PacketSelectAdvancementTab> _onselect_advancement_tab = new();
    private readonly Subject<PacketServerData> _onserver_data = new();
    private readonly Subject<PacketSetCooldown> _onset_cooldown = new();
    private readonly Subject<PacketSetPassengers> _onset_passengers = new();
    private readonly Subject<PacketSetSlot> _onset_slot = new();
    private readonly Subject<PacketSetTitleSubtitle> _onset_title_subtitle = new();
    private readonly Subject<PacketSetTitleText> _onset_title_text = new();
    private readonly Subject<PacketSetTitleTime> _onset_title_time = new();
    private readonly Subject<PacketSimulationDistance> _onsimulation_distance = new();

    private readonly Subject<PacketSpawnEntity> _onspawn_entity = new();
    private readonly Subject<PacketSpawnEntityExperienceOrb> _onspawn_entity_experience_orb = new();
    private readonly Subject<PacketSpawnPosition> _onspawn_position = new();
    private readonly Subject<PacketStartConfiguration> _onstart_configuration = new();
    private readonly Subject<PacketSystemChat> _onsystem_chat = new();
    private readonly Subject<PacketTileEntityData> _ontile_entity_data = new();
    private readonly Subject<PacketUnloadChunk> _onunload_chunk = new();
    private readonly Subject<PacketUpdateHealth> _onupdate_health = new();
    private readonly Subject<PacketUpdateTime> _onupdate_time = new();
    private readonly Subject<PacketUpdateViewDistance> _onupdate_view_distance = new();
    private readonly Subject<PacketUpdateViewPosition> _onupdate_view_position = new();
    private readonly Subject<PacketVehicleMove> _onvehicle_move = new();
    private readonly Subject<PacketWorldBorderCenter> _onworld_border_center = new();
    private readonly Subject<PacketWorldBorderLerpSize> _onworld_border_lerp_size = new();
    private readonly Subject<PacketWorldBorderSize> _onworld_border_size = new();
    private readonly Subject<PacketWorldBorderWarningDelay> _onworld_border_warning_delay = new();
    private readonly Subject<PacketWorldBorderWarningReach> _onworld_border_warning_reach = new();
    private readonly Subject<PacketWorldEvent> _onworld_event = new();

    public Protocol_764(IPacketBroker client) : base(client)
    {
        SupportedVersion = 764;
    }

    public IObservable<PacketSpawnEntity> OnSpawnEntityPacket => _onspawn_entity;

    public IObservable<PacketSpawnEntityExperienceOrb> OnSpawnEntityExperienceOrbPacket =>
        _onspawn_entity_experience_orb;

    public IObservable<PacketAnimation> OnAnimationPacket => _onanimation;
    public IObservable<PacketBlockBreakAnimation> OnBlockBreakAnimationPacket => _onblock_break_animation;
    public IObservable<PacketTileEntityData> OnTileEntityDataPacket => _ontile_entity_data;
    public IObservable<PacketBlockAction> OnBlockActionPacket => _onblock_action;
    public IObservable<PacketBlockChange> OnBlockChangePacket => _onblock_change;
    public IObservable<PacketDifficulty> OnDifficultyPacket => _ondifficulty;
    public IObservable<PacketChunkBatchFinished> OnChunkBatchFinishedPacket => _onchunk_batch_finished;
    public IObservable<PacketChunkBatchStart> OnChunkBatchStartPacket => _onchunk_batch_start;
    public IObservable<PacketNbtQueryResponse> OnNbtQueryResponsePacket => _onnbt_query_response;
    public IObservable<PacketCloseWindow> OnCloseWindowPacket => _onclose_window;
    public IObservable<PacketOpenWindow> OnOpenWindowPacket => _onopen_window;
    public IObservable<PacketCraftProgressBar> OnCraftProgressBarPacket => _oncraft_progress_bar;
    public IObservable<PacketSetSlot> OnSetSlotPacket => _onset_slot;
    public IObservable<PacketSetCooldown> OnSetCooldownPacket => _onset_cooldown;
    public IObservable<PacketChatSuggestions> OnChatSuggestionsPacket => _onchat_suggestions;
    public IObservable<PacketCustomPayload> OnCustomPayloadPacket => _oncustom_payload;
    public IObservable<PacketKickDisconnect> OnKickDisconnectPacket => _onkick_disconnect;
    public IObservable<PacketProfilelessChat> OnProfilelessChatPacket => _onprofileless_chat;
    public IObservable<PacketEntityStatus> OnEntityStatusPacket => _onentity_status;
    public IObservable<PacketUnloadChunk> OnUnloadChunkPacket => _onunload_chunk;
    public IObservable<PacketGameStateChange> OnGameStateChangePacket => _ongame_state_change;
    public IObservable<PacketOpenHorseWindow> OnOpenHorseWindowPacket => _onopen_horse_window;
    public IObservable<PacketKeepAlive> OnKeepAlivePacket => _onkeep_alive;
    public IObservable<PacketWorldEvent> OnWorldEventPacket => _onworld_event;
    public IObservable<PacketRelEntityMove> OnRelEntityMovePacket => _onrel_entity_move;
    public IObservable<PacketEntityMoveLook> OnEntityMoveLookPacket => _onentity_move_look;
    public IObservable<PacketEntityLook> OnEntityLookPacket => _onentity_look;
    public IObservable<PacketVehicleMove> OnVehicleMovePacket => _onvehicle_move;
    public IObservable<PacketOpenBook> OnOpenBookPacket => _onopen_book;
    public IObservable<PacketOpenSignEntity> OnOpenSignEntityPacket => _onopen_sign_entity;
    public IObservable<PacketCraftRecipeResponse> OnCraftRecipeResponsePacket => _oncraft_recipe_response;
    public IObservable<PacketAbilities> OnAbilitiesPacket => _onabilities;
    public IObservable<PacketEndCombatEvent> OnEndCombatEventPacket => _onend_combat_event;
    public IObservable<PacketEnterCombatEvent> OnEnterCombatEventPacket => _onenter_combat_event;
    public IObservable<PacketDeathCombatEvent> OnDeathCombatEventPacket => _ondeath_combat_event;
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
    public IObservable<PacketSystemChat> OnSystemChatPacket => _onsystem_chat;
    public IObservable<PacketPlayerlistHeader> OnPlayerlistHeaderPacket => _onplayerlist_header;
    public IObservable<PacketCollect> OnCollectPacket => _oncollect;
    public IObservable<PacketEntityTeleport> OnEntityTeleportPacket => _onentity_teleport;
    public IObservable<PacketSelectAdvancementTab> OnSelectAdvancementTabPacket => _onselect_advancement_tab;
    public IObservable<PacketServerData> OnServerDataPacket => _onserver_data;

    public IObservable<PacketAcknowledgePlayerDigging> OnAcknowledgePlayerDiggingPacket =>
        _onacknowledge_player_digging;

    public IObservable<PacketClearTitles> OnClearTitlesPacket => _onclear_titles;
    public IObservable<PacketInitializeWorldBorder> OnInitializeWorldBorderPacket => _oninitialize_world_border;
    public IObservable<PacketActionBar> OnActionBarPacket => _onaction_bar;
    public IObservable<PacketWorldBorderCenter> OnWorldBorderCenterPacket => _onworld_border_center;
    public IObservable<PacketWorldBorderLerpSize> OnWorldBorderLerpSizePacket => _onworld_border_lerp_size;
    public IObservable<PacketWorldBorderSize> OnWorldBorderSizePacket => _onworld_border_size;

    public IObservable<PacketWorldBorderWarningDelay> OnWorldBorderWarningDelayPacket =>
        _onworld_border_warning_delay;

    public IObservable<PacketWorldBorderWarningReach> OnWorldBorderWarningReachPacket =>
        _onworld_border_warning_reach;

    public IObservable<PacketPing> OnPingPacket => _onping;
    public IObservable<PacketPingResponse> OnPingResponsePacket => _onping_response;
    public IObservable<PacketSetTitleSubtitle> OnSetTitleSubtitlePacket => _onset_title_subtitle;
    public IObservable<PacketSetTitleText> OnSetTitleTextPacket => _onset_title_text;
    public IObservable<PacketSetTitleTime> OnSetTitleTimePacket => _onset_title_time;
    public IObservable<PacketSimulationDistance> OnSimulationDistancePacket => _onsimulation_distance;
    public IObservable<PacketHurtAnimation> OnHurtAnimationPacket => _onhurt_animation;
    public IObservable<PacketStartConfiguration> OnStartConfigurationPacket => _onstart_configuration;

    public ValueTask SendTeleportConfirm(int teleportId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x00);
            writer.WriteVarInt(teleportId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendQueryBlockNbt(int transactionId, Position location)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x01);
            writer.WriteVarInt(transactionId);
            writer.WritePosition(location);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendChatMessage(string message, long timestamp, long salt, byte[]? signature, int offset,
        byte[] acknowledged)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
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
                writer.WriteBuffer(signature.AsSpan(0, 256));
            }

            writer.WriteVarInt(offset);
            writer.WriteVarInt(3);
            writer.WriteBuffer(acknowledged.AsSpan(0, 3));
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSetDifficulty(byte newDifficulty)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x02);
            writer.WriteUnsignedByte(newDifficulty);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendMessageAcknowledgement(int count)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x03);
            writer.WriteVarInt(count);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendEditBook(int hand, string[] pages, string? title)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x10);
            writer.WriteVarInt(hand);
            writer.WriteVarInt(pages.Length);
            for (var i_0 = 0; i_0 < pages.Length; i_0++)
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

            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendQueryEntityNbt(int transactionId, int entityId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x11);
            writer.WriteVarInt(transactionId);
            writer.WriteVarInt(entityId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendPickItem(int slot)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x1c);
            writer.WriteVarInt(slot);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendNameItem(string name)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x26);
            writer.WriteString(name);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSelectTrade(int slot)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x29);
            writer.WriteVarInt(slot);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSetBeaconEffect(int? primaryEffect, int? secondaryEffect)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x2a);
            if (primaryEffect is null)
            {
                writer.WriteBoolean(false);
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteVarInt(primaryEffect);
            }

            if (secondaryEffect is null)
            {
                writer.WriteBoolean(false);
            }
            else
            {
                writer.WriteBoolean(true);
                writer.WriteVarInt(secondaryEffect);
            }

            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendUpdateCommandBlock(Position location, string command, int mode, byte flags)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x2c);
            writer.WritePosition(location);
            writer.WriteString(command);
            writer.WriteVarInt(mode);
            writer.WriteUnsignedByte(flags);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendUpdateCommandBlockMinecart(int entityId, string command, bool trackOutput)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x2d);
            writer.WriteVarInt(entityId);
            writer.WriteString(command);
            writer.WriteBoolean(trackOutput);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendUpdateStructureBlock(Position location, int action, int mode, string name, sbyte offsetX,
        sbyte offsetY, sbyte offsetZ, sbyte sizeX, sbyte sizeY, sbyte sizeZ, int mirror, int rotation,
        string metadata, float integrity, int seed, byte flags)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x30);
            writer.WritePosition(location);
            writer.WriteVarInt(action);
            writer.WriteVarInt(mode);
            writer.WriteString(name);
            writer.WriteSignedByte(offsetX);
            writer.WriteSignedByte(offsetY);
            writer.WriteSignedByte(offsetZ);
            writer.WriteSignedByte(sizeX);
            writer.WriteSignedByte(sizeY);
            writer.WriteSignedByte(sizeZ);
            writer.WriteVarInt(mirror);
            writer.WriteVarInt(rotation);
            writer.WriteString(metadata);
            writer.WriteFloat(integrity);
            writer.WriteVarInt(seed);
            writer.WriteUnsignedByte(flags);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendTabComplete(int transactionId, string text)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x0a);
            writer.WriteVarInt(transactionId);
            writer.WriteString(text);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendClientCommand(int actionId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x08);
            writer.WriteVarInt(actionId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSettings(string locale, sbyte viewDistance, int chatFlags, bool chatColors, byte skinParts,
        int mainHand, bool enableTextFiltering, bool enableServerListing)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x09);
            writer.WriteString(locale);
            writer.WriteSignedByte(viewDistance);
            writer.WriteVarInt(chatFlags);
            writer.WriteBoolean(chatColors);
            writer.WriteUnsignedByte(skinParts);
            writer.WriteVarInt(mainHand);
            writer.WriteBoolean(enableTextFiltering);
            writer.WriteBoolean(enableServerListing);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendEnchantItem(sbyte windowId, sbyte enchantment)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x0c);
            writer.WriteSignedByte(windowId);
            writer.WriteSignedByte(enchantment);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendCloseWindow(byte windowId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x0e);
            writer.WriteUnsignedByte(windowId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendCustomPayload(string channel, byte[] data)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x0f);
            writer.WriteString(channel);
            writer.WriteBuffer(data);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendGenerateStructure(Position location, int levels, bool keepJigsaws)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x13);
            writer.WritePosition(location);
            writer.WriteVarInt(levels);
            writer.WriteBoolean(keepJigsaws);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendKeepAlive(long keepAliveId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x14);
            writer.WriteSignedLong(keepAliveId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendLockDifficulty(bool locked)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x15);
            writer.WriteBoolean(locked);
            return SendPacketCore(writer.GetWrittenMemory());
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
            writer.WriteVarInt(0x16);
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

    public ValueTask SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x17);
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
            writer.WriteVarInt(0x18);
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

    public ValueTask SendFlying(bool onGround)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x19);
            writer.WriteBoolean(onGround);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendVehicleMove(double x, double y, double z, float yaw, float pitch)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x1a);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSteerBoat(bool leftPaddle, bool rightPaddle)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x1b);
            writer.WriteBoolean(leftPaddle);
            writer.WriteBoolean(rightPaddle);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendCraftRecipeRequest(sbyte windowId, string recipe, bool makeAll)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x1e);
            writer.WriteSignedByte(windowId);
            writer.WriteString(recipe);
            writer.WriteBoolean(makeAll);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendAbilities(sbyte flags)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x1f);
            writer.WriteSignedByte(flags);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendBlockDig(int status, Position location, sbyte face, int sequence)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x20);
            writer.WriteVarInt(status);
            writer.WritePosition(location);
            writer.WriteSignedByte(face);
            writer.WriteVarInt(sequence);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendEntityAction(int entityId, int actionId, int jumpBoost)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x21);
            writer.WriteVarInt(entityId);
            writer.WriteVarInt(actionId);
            writer.WriteVarInt(jumpBoost);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSteerVehicle(float sideways, float forward, byte jump)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x22);
            writer.WriteFloat(sideways);
            writer.WriteFloat(forward);
            writer.WriteUnsignedByte(jump);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendDisplayedRecipe(string recipeId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x25);
            writer.WriteString(recipeId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendRecipeBook(int bookId, bool bookOpen, bool filterActive)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x24);
            writer.WriteVarInt(bookId);
            writer.WriteBoolean(bookOpen);
            writer.WriteBoolean(filterActive);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendResourcePackReceive(int result)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x27);
            writer.WriteVarInt(result);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendHeldItemSlot(short slotId)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x2b);
            writer.WriteSignedShort(slotId);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSetCreativeSlot(short slot, Slot? item)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x2e);
            writer.WriteSignedShort(slot);
            writer.WriteSlot(item);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendUpdateJigsawBlock(Position location, string name, string target, string pool,
        string finalState, string jointType)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x2f);
            writer.WritePosition(location);
            writer.WriteString(name);
            writer.WriteString(target);
            writer.WriteString(pool);
            writer.WriteString(finalState);
            writer.WriteString(jointType);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendUpdateSign(Position location, bool isFrontText, string text1, string text2, string text3,
        string text4)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x31);
            writer.WritePosition(location);
            writer.WriteBoolean(isFrontText);
            writer.WriteString(text1);
            writer.WriteString(text2);
            writer.WriteString(text3);
            writer.WriteString(text4);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendArmAnimation(int hand)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x32);
            writer.WriteVarInt(hand);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendSpectate(Guid target)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x33);
            writer.WriteUUID(target);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendBlockPlace(int hand, Position location, int direction, float cursorX, float cursorY,
        float cursorZ, bool insideBlock, int sequence)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x34);
            writer.WriteVarInt(hand);
            writer.WritePosition(location);
            writer.WriteVarInt(direction);
            writer.WriteFloat(cursorX);
            writer.WriteFloat(cursorY);
            writer.WriteFloat(cursorZ);
            writer.WriteBoolean(insideBlock);
            writer.WriteVarInt(sequence);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendUseItem(int hand, int sequence)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x35);
            writer.WriteVarInt(hand);
            writer.WriteVarInt(sequence);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendPong(int id)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x23);
            writer.WriteSignedInt(id);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendChatSessionUpdate(Guid sessionUUID, long expireTime, byte[] publicKey, byte[] signature)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x06);
            writer.WriteUUID(sessionUUID);
            writer.WriteSignedLong(expireTime);
            writer.WriteVarInt(publicKey.Length);
            writer.WriteBuffer(publicKey);
            writer.WriteVarInt(signature.Length);
            writer.WriteBuffer(signature);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendChunkBatchReceived(float chunksPerTick)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x07);
            writer.WriteFloat(chunksPerTick);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendConfigurationAcknowledged()
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x0b);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    public ValueTask SendPingRequest(long id)
    {
        scoped var writer = new MinecraftPrimitiveSpanWriter();
        try
        {
            writer.WriteVarInt(0x1d);
            writer.WriteSignedLong(id);
            return SendPacketCore(writer.GetWrittenMemory());
        }
        catch
        {
            writer.Dispose();
            throw;
        }
    }

    protected override void OnPacketReceived(InputPacket packet)
    {
        switch (packet.Id)
        {
            case 0x01:
                if (_onspawn_entity.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var objectUUID = reader.ReadUUID();
                    var type = reader.ReadVarInt();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var pitch = reader.ReadSignedByte();
                    var yaw = reader.ReadSignedByte();
                    var headPitch = reader.ReadSignedByte();
                    var objectData = reader.ReadVarInt();
                    var velocityX = reader.ReadSignedShort();
                    var velocityY = reader.ReadSignedShort();
                    var velocityZ = reader.ReadSignedShort();
                    _onspawn_entity.OnNext(new PacketSpawnEntity(entityId, objectUUID, type, x, y, z, pitch, yaw,
                        headPitch, objectData, velocityX, velocityY, velocityZ));
                }

                break;
            case 0x02:
                if (_onspawn_entity_experience_orb.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var count = reader.ReadSignedShort();
                    _onspawn_entity_experience_orb.OnNext(
                        new PacketSpawnEntityExperienceOrb(entityId, x, y, z, count));
                }

                break;
            case 0x03:
                if (_onanimation.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var animation = reader.ReadUnsignedByte();
                    _onanimation.OnNext(new PacketAnimation(entityId, animation));
                }

                break;
            case 0x06:
                if (_onblock_break_animation.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var location = reader.ReadPosition();
                    var destroyStage = reader.ReadSignedByte();
                    _onblock_break_animation.OnNext(new PacketBlockBreakAnimation(entityId, location,
                        destroyStage));
                }

                break;
            case 0x07:
                if (_ontile_entity_data.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var location = reader.ReadPosition();
                    var action = reader.ReadVarInt();
                    var nbtData = reader.ReadOptionalNbt(false);
                    _ontile_entity_data.OnNext(new PacketTileEntityData(location, action, nbtData));
                }

                break;
            case 0x08:
                if (_onblock_action.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var location = reader.ReadPosition();
                    var byte1 = reader.ReadUnsignedByte();
                    var byte2 = reader.ReadUnsignedByte();
                    var blockId = reader.ReadVarInt();
                    _onblock_action.OnNext(new PacketBlockAction(location, byte1, byte2, blockId));
                }

                break;
            case 0x09:
                if (_onblock_change.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var location = reader.ReadPosition();
                    var type = reader.ReadVarInt();
                    _onblock_change.OnNext(new PacketBlockChange(location, type));
                }

                break;
            case 0x0b:
                if (_ondifficulty.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var difficulty = reader.ReadUnsignedByte();
                    var difficultyLocked = reader.ReadBoolean();
                    _ondifficulty.OnNext(new PacketDifficulty(difficulty, difficultyLocked));
                }

                break;
            case 0x0c:
                if (_onchunk_batch_finished.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var batchSize = reader.ReadVarInt();
                    _onchunk_batch_finished.OnNext(new PacketChunkBatchFinished(batchSize));
                }

                break;
            case 0x0d:
                if (_onchunk_batch_start.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    _onchunk_batch_start.OnNext(new PacketChunkBatchStart());
                }

                break;
            case 0x69:
                if (_onnbt_query_response.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var transactionId = reader.ReadVarInt();
                    var nbt = reader.ReadOptionalNbt(false);
                    _onnbt_query_response.OnNext(new PacketNbtQueryResponse(transactionId, nbt));
                }

                break;
            case 0x12:
                if (_onclose_window.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    _onclose_window.OnNext(new PacketCloseWindow(windowId));
                }

                break;
            case 0x31:
                if (_onopen_window.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var windowId = reader.ReadVarInt();
                    var inventoryType = reader.ReadVarInt();
                    var windowTitle = reader.ReadString();
                    _onopen_window.OnNext(new PacketOpenWindow(windowId, inventoryType, windowTitle));
                }

                break;
            case 0x14:
                if (_oncraft_progress_bar.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    var property = reader.ReadSignedShort();
                    var value = reader.ReadSignedShort();
                    _oncraft_progress_bar.OnNext(new PacketCraftProgressBar(windowId, property, value));
                }

                break;
            case 0x15:
                if (_onset_slot.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var windowId = reader.ReadSignedByte();
                    var stateId = reader.ReadVarInt();
                    var slot = reader.ReadSignedShort();
                    var item = reader.ReadSlot(ProtocolVersion);
                    _onset_slot.OnNext(new PacketSetSlot(windowId, stateId, slot, item));
                }

                break;
            case 0x16:
                if (_onset_cooldown.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var itemID = reader.ReadVarInt();
                    var cooldownTicks = reader.ReadVarInt();
                    _onset_cooldown.OnNext(new PacketSetCooldown(itemID, cooldownTicks));
                }

                break;
            case 0x17:
                if (_onchat_suggestions.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var action = reader.ReadVarInt();
                    var tempArrayLength_1_0 = reader.ReadVarInt();
                    var tempArray_1_0 = new string[tempArrayLength_1_0];
                    for (var i_1_0 = 0; i_1_0 < tempArrayLength_1_0; i_1_0++)
                    {
                        var for_item_1_0 = reader.ReadString();
                        tempArray_1_0[i_1_0] = for_item_1_0;
                    }

                    var entries = tempArray_1_0;
                    _onchat_suggestions.OnNext(new PacketChatSuggestions(action, entries));
                }

                break;
            case 0x18:
                if (_oncustom_payload.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var channel = reader.ReadString();
                    var data = reader.ReadRestBuffer();
                    _oncustom_payload.OnNext(new PacketCustomPayload(channel, data));
                }

                break;
            case 0x1b:
                if (_onkick_disconnect.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var reason = reader.ReadString();
                    _onkick_disconnect.OnNext(new PacketKickDisconnect(reason));
                }

                break;
            case 0x1c:
                if (_onprofileless_chat.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var message = reader.ReadString();
                    var type = reader.ReadVarInt();
                    var name = reader.ReadString();
                    string? target = null;
                    if (reader.ReadBoolean()) target = reader.ReadString();

                    _onprofileless_chat.OnNext(new PacketProfilelessChat(message, type, name, target));
                }

                break;
            case 0x1d:
                if (_onentity_status.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadSignedInt();
                    var entityStatus = reader.ReadSignedByte();
                    _onentity_status.OnNext(new PacketEntityStatus(entityId, entityStatus));
                }

                break;
            case 0x1f:
                if (_onunload_chunk.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var chunkZ = reader.ReadSignedInt();
                    var chunkX = reader.ReadSignedInt();
                    _onunload_chunk.OnNext(new PacketUnloadChunk(chunkZ, chunkX));
                }

                break;
            case 0x20:
                if (_ongame_state_change.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var reason = reader.ReadUnsignedByte();
                    var gameMode = reader.ReadFloat();
                    _ongame_state_change.OnNext(new PacketGameStateChange(reason, gameMode));
                }

                break;
            case 0x21:
                if (_onopen_horse_window.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    var nbSlots = reader.ReadVarInt();
                    var entityId = reader.ReadSignedInt();
                    _onopen_horse_window.OnNext(new PacketOpenHorseWindow(windowId, nbSlots, entityId));
                }

                break;
            case 0x24:
                if (_onkeep_alive.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var keepAliveId = reader.ReadSignedLong();
                    _onkeep_alive.OnNext(new PacketKeepAlive(keepAliveId));
                }

                break;
            case 0x26:
                if (_onworld_event.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var effectId = reader.ReadSignedInt();
                    var location = reader.ReadPosition();
                    var data = reader.ReadSignedInt();
                    var global = reader.ReadBoolean();
                    _onworld_event.OnNext(new PacketWorldEvent(effectId, location, data, global));
                }

                break;
            case 0x2c:
                if (_onrel_entity_move.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var dX = reader.ReadSignedShort();
                    var dY = reader.ReadSignedShort();
                    var dZ = reader.ReadSignedShort();
                    var onGround = reader.ReadBoolean();
                    _onrel_entity_move.OnNext(new PacketRelEntityMove(entityId, dX, dY, dZ, onGround));
                }

                break;
            case 0x2d:
                if (_onentity_move_look.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var dX = reader.ReadSignedShort();
                    var dY = reader.ReadSignedShort();
                    var dZ = reader.ReadSignedShort();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var onGround = reader.ReadBoolean();
                    _onentity_move_look.OnNext(new PacketEntityMoveLook(entityId, dX, dY, dZ, yaw, pitch,
                        onGround));
                }

                break;
            case 0x2e:
                if (_onentity_look.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var onGround = reader.ReadBoolean();
                    _onentity_look.OnNext(new PacketEntityLook(entityId, yaw, pitch, onGround));
                }

                break;
            case 0x2f:
                if (_onvehicle_move.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var x = reader.ReadDouble();
                    var y = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var yaw = reader.ReadFloat();
                    var pitch = reader.ReadFloat();
                    _onvehicle_move.OnNext(new PacketVehicleMove(x, y, z, yaw, pitch));
                }

                break;
            case 0x30:
                if (_onopen_book.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var hand = reader.ReadVarInt();
                    _onopen_book.OnNext(new PacketOpenBook(hand));
                }

                break;
            case 0x32:
                if (_onopen_sign_entity.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var location = reader.ReadPosition();
                    var isFrontText = reader.ReadBoolean();
                    _onopen_sign_entity.OnNext(new PacketOpenSignEntity(location, isFrontText));
                }

                break;
            case 0x35:
                if (_oncraft_recipe_response.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var windowId = reader.ReadSignedByte();
                    var recipe = reader.ReadString();
                    _oncraft_recipe_response.OnNext(new PacketCraftRecipeResponse(windowId, recipe));
                }

                break;
            case 0x36:
                if (_onabilities.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var flags = reader.ReadSignedByte();
                    var flyingSpeed = reader.ReadFloat();
                    var walkingSpeed = reader.ReadFloat();
                    _onabilities.OnNext(new PacketAbilities(flags, flyingSpeed, walkingSpeed));
                }

                break;
            case 0x38:
                if (_onend_combat_event.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var duration = reader.ReadVarInt();
                    _onend_combat_event.OnNext(new PacketEndCombatEvent(duration));
                }

                break;
            case 0x39:
                if (_onenter_combat_event.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    _onenter_combat_event.OnNext(new PacketEnterCombatEvent());
                }

                break;
            case 0x3a:
                if (_ondeath_combat_event.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var playerId = reader.ReadVarInt();
                    var message = reader.ReadString();
                    _ondeath_combat_event.OnNext(new PacketDeathCombatEvent(playerId, message));
                }

                break;
            case 0x3e:
                if (_onposition.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
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
            case 0x40:
                if (_onentity_destroy.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var tempArrayLength_0_0 = reader.ReadVarInt();
                    var tempArray_0_0 = new int[tempArrayLength_0_0];
                    for (var i_0_0 = 0; i_0_0 < tempArrayLength_0_0; i_0_0++)
                    {
                        var for_item_0_0 = reader.ReadVarInt();
                        tempArray_0_0[i_0_0] = for_item_0_0;
                    }

                    var entityIds = tempArray_0_0;
                    _onentity_destroy.OnNext(new PacketEntityDestroy(entityIds));
                }

                break;
            case 0x41:
                if (_onremove_entity_effect.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var effectId = reader.ReadVarInt();
                    _onremove_entity_effect.OnNext(new PacketRemoveEntityEffect(entityId, effectId));
                }

                break;
            case 0x42:
                if (_onresource_pack_send.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var url = reader.ReadString();
                    var hash = reader.ReadString();
                    var forced = reader.ReadBoolean();
                    string? promptMessage = null;
                    if (reader.ReadBoolean()) promptMessage = reader.ReadString();

                    _onresource_pack_send.OnNext(new PacketResourcePackSend(url, hash, forced, promptMessage));
                }

                break;
            case 0x44:
                if (_onentity_head_rotation.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var headYaw = reader.ReadSignedByte();
                    _onentity_head_rotation.OnNext(new PacketEntityHeadRotation(entityId, headYaw));
                }

                break;
            case 0x4e:
                if (_oncamera.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var cameraId = reader.ReadVarInt();
                    _oncamera.OnNext(new PacketCamera(cameraId));
                }

                break;
            case 0x4f:
                if (_onheld_item_slot.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var slot = reader.ReadSignedByte();
                    _onheld_item_slot.OnNext(new PacketHeldItemSlot(slot));
                }

                break;
            case 0x50:
                if (_onupdate_view_position.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var chunkX = reader.ReadVarInt();
                    var chunkZ = reader.ReadVarInt();
                    _onupdate_view_position.OnNext(new PacketUpdateViewPosition(chunkX, chunkZ));
                }

                break;
            case 0x51:
                if (_onupdate_view_distance.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var viewDistance = reader.ReadVarInt();
                    _onupdate_view_distance.OnNext(new PacketUpdateViewDistance(viewDistance));
                }

                break;
            case 0x53:
                if (_onscoreboard_display_objective.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var position = reader.ReadVarInt();
                    var name = reader.ReadString();
                    _onscoreboard_display_objective.OnNext(new PacketScoreboardDisplayObjective(position, name));
                }

                break;
            case 0x55:
                if (_onattach_entity.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadSignedInt();
                    var vehicleId = reader.ReadSignedInt();
                    _onattach_entity.OnNext(new PacketAttachEntity(entityId, vehicleId));
                }

                break;
            case 0x56:
                if (_onentity_velocity.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var velocityX = reader.ReadSignedShort();
                    var velocityY = reader.ReadSignedShort();
                    var velocityZ = reader.ReadSignedShort();
                    _onentity_velocity.OnNext(new PacketEntityVelocity(entityId, velocityX, velocityY, velocityZ));
                }

                break;
            case 0x58:
                if (_onexperience.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var experienceBar = reader.ReadFloat();
                    var level = reader.ReadVarInt();
                    var totalExperience = reader.ReadVarInt();
                    _onexperience.OnNext(new PacketExperience(experienceBar, level, totalExperience));
                }

                break;
            case 0x59:
                if (_onupdate_health.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var health = reader.ReadFloat();
                    var food = reader.ReadVarInt();
                    var foodSaturation = reader.ReadFloat();
                    _onupdate_health.OnNext(new PacketUpdateHealth(health, food, foodSaturation));
                }

                break;
            case 0x5b:
                if (_onset_passengers.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var tempArrayLength_1_0 = reader.ReadVarInt();
                    var tempArray_1_0 = new int[tempArrayLength_1_0];
                    for (var i_1_0 = 0; i_1_0 < tempArrayLength_1_0; i_1_0++)
                    {
                        var for_item_1_0 = reader.ReadVarInt();
                        tempArray_1_0[i_1_0] = for_item_1_0;
                    }

                    var passengers = tempArray_1_0;
                    _onset_passengers.OnNext(new PacketSetPassengers(entityId, passengers));
                }

                break;
            case 0x52:
                if (_onspawn_position.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var location = reader.ReadPosition();
                    var angle = reader.ReadFloat();
                    _onspawn_position.OnNext(new PacketSpawnPosition(location, angle));
                }

                break;
            case 0x60:
                if (_onupdate_time.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var age = reader.ReadSignedLong();
                    var time = reader.ReadSignedLong();
                    _onupdate_time.OnNext(new PacketUpdateTime(age, time));
                }

                break;
            case 0x67:
                if (_onsystem_chat.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var content = reader.ReadString();
                    var isActionBar = reader.ReadBoolean();
                    _onsystem_chat.OnNext(new PacketSystemChat(content, isActionBar));
                }

                break;
            case 0x68:
                if (_onplayerlist_header.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var header = reader.ReadString();
                    var footer = reader.ReadString();
                    _onplayerlist_header.OnNext(new PacketPlayerlistHeader(header, footer));
                }

                break;
            case 0x6a:
                if (_oncollect.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var collectedEntityId = reader.ReadVarInt();
                    var collectorEntityId = reader.ReadVarInt();
                    var pickupItemCount = reader.ReadVarInt();
                    _oncollect.OnNext(new PacketCollect(collectedEntityId, collectorEntityId, pickupItemCount));
                }

                break;
            case 0x6b:
                if (_onentity_teleport.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
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
            case 0x46:
                if (_onselect_advancement_tab.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    string? id = null;
                    if (reader.ReadBoolean()) id = reader.ReadString();

                    _onselect_advancement_tab.OnNext(new PacketSelectAdvancementTab(id));
                }

                break;
            case 0x47:
                if (_onserver_data.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var motd = reader.ReadString();
                    byte[]? iconBytes = null;
                    if (reader.ReadBoolean())
                    {
                        var tempArrayLength_1_0 = reader.ReadVarInt();
                        iconBytes = reader.ReadBuffer(tempArrayLength_1_0);
                    }

                    var enforcesSecureChat = reader.ReadBoolean();
                    _onserver_data.OnNext(new PacketServerData(motd, iconBytes, enforcesSecureChat));
                }

                break;
            case 0x05:
                if (_onacknowledge_player_digging.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var sequenceId = reader.ReadVarInt();
                    _onacknowledge_player_digging.OnNext(new PacketAcknowledgePlayerDigging(sequenceId));
                }

                break;
            case 0x0f:
                if (_onclear_titles.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var reset = reader.ReadBoolean();
                    _onclear_titles.OnNext(new PacketClearTitles(reset));
                }

                break;
            case 0x23:
                if (_oninitialize_world_border.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var x = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var oldDiameter = reader.ReadDouble();
                    var newDiameter = reader.ReadDouble();
                    var speed = reader.ReadVarInt();
                    var portalTeleportBoundary = reader.ReadVarInt();
                    var warningBlocks = reader.ReadVarInt();
                    var warningTime = reader.ReadVarInt();
                    _oninitialize_world_border.OnNext(new PacketInitializeWorldBorder(x, z, oldDiameter,
                        newDiameter, speed, portalTeleportBoundary, warningBlocks, warningTime));
                }

                break;
            case 0x48:
                if (_onaction_bar.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var text = reader.ReadString();
                    _onaction_bar.OnNext(new PacketActionBar(text));
                }

                break;
            case 0x49:
                if (_onworld_border_center.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var x = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    _onworld_border_center.OnNext(new PacketWorldBorderCenter(x, z));
                }

                break;
            case 0x4a:
                if (_onworld_border_lerp_size.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var oldDiameter = reader.ReadDouble();
                    var newDiameter = reader.ReadDouble();
                    var speed = reader.ReadVarInt();
                    _onworld_border_lerp_size.OnNext(new PacketWorldBorderLerpSize(oldDiameter, newDiameter,
                        speed));
                }

                break;
            case 0x4b:
                if (_onworld_border_size.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var diameter = reader.ReadDouble();
                    _onworld_border_size.OnNext(new PacketWorldBorderSize(diameter));
                }

                break;
            case 0x4c:
                if (_onworld_border_warning_delay.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var warningTime = reader.ReadVarInt();
                    _onworld_border_warning_delay.OnNext(new PacketWorldBorderWarningDelay(warningTime));
                }

                break;
            case 0x4d:
                if (_onworld_border_warning_reach.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var warningBlocks = reader.ReadVarInt();
                    _onworld_border_warning_reach.OnNext(new PacketWorldBorderWarningReach(warningBlocks));
                }

                break;
            case 0x33:
                if (_onping.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var id = reader.ReadSignedInt();
                    _onping.OnNext(new PacketPing(id));
                }

                break;
            case 0x34:
                if (_onping_response.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var id = reader.ReadSignedLong();
                    _onping_response.OnNext(new PacketPingResponse(id));
                }

                break;
            case 0x5f:
                if (_onset_title_subtitle.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var text = reader.ReadString();
                    _onset_title_subtitle.OnNext(new PacketSetTitleSubtitle(text));
                }

                break;
            case 0x61:
                if (_onset_title_text.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var text = reader.ReadString();
                    _onset_title_text.OnNext(new PacketSetTitleText(text));
                }

                break;
            case 0x62:
                if (_onset_title_time.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var fadeIn = reader.ReadSignedInt();
                    var stay = reader.ReadSignedInt();
                    var fadeOut = reader.ReadSignedInt();
                    _onset_title_time.OnNext(new PacketSetTitleTime(fadeIn, stay, fadeOut));
                }

                break;
            case 0x5e:
                if (_onsimulation_distance.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var distance = reader.ReadVarInt();
                    _onsimulation_distance.OnNext(new PacketSimulationDistance(distance));
                }

                break;
            case 0x22:
                if (_onhurt_animation.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var yaw = reader.ReadFloat();
                    _onhurt_animation.OnNext(new PacketHurtAnimation(entityId, yaw));
                }

                break;
            case 0x65:
                if (_onstart_configuration.HasObservers)
                {
                    scoped var reader = new MinecraftPrimitiveSpanReader(packet.Data);
                    _onstart_configuration.OnNext(new PacketStartConfiguration());
                }

                break;
        }
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

public class PacketTileEntityData
{
    public PacketTileEntityData(Position location, int action, NbtTag? nbtData)
    {
        Location = location;
        Action = action;
        NbtData = nbtData;
    }

    public Position Location { get; internal set; }
    public int Action { get; internal set; }
    public NbtTag? NbtData { get; internal set; }
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

public class PacketChunkBatchFinished
{
    public PacketChunkBatchFinished(int batchSize)
    {
        BatchSize = batchSize;
    }

    public int BatchSize { get; internal set; }
}

public class PacketChunkBatchStart
{
}

public class PacketNbtQueryResponse
{
    public PacketNbtQueryResponse(int transactionId, NbtTag? nbt)
    {
        TransactionId = transactionId;
        Nbt = nbt;
    }

    public int TransactionId { get; internal set; }
    public NbtTag? Nbt { get; internal set; }
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
    public PacketSetSlot(sbyte windowId, int stateId, short slot, Slot? item)
    {
        WindowId = windowId;
        StateId = stateId;
        Slot = slot;
        Item = item;
    }

    public sbyte WindowId { get; internal set; }
    public int StateId { get; internal set; }
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

public class PacketChatSuggestions
{
    public PacketChatSuggestions(int action, string[] entries)
    {
        Action = action;
        Entries = entries;
    }

    public int Action { get; internal set; }
    public string[] Entries { get; internal set; }
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

public class PacketKickDisconnect
{
    public PacketKickDisconnect(string reason)
    {
        Reason = reason;
    }

    public string Reason { get; internal set; }
}

public class PacketProfilelessChat
{
    public PacketProfilelessChat(string message, int type, string name, string? target)
    {
        Message = message;
        Type = type;
        Name = name;
        Target = target;
    }

    public string Message { get; internal set; }
    public int Type { get; internal set; }
    public string Name { get; internal set; }
    public string? Target { get; internal set; }
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
    public PacketUnloadChunk(int chunkZ, int chunkX)
    {
        ChunkZ = chunkZ;
        ChunkX = chunkX;
    }

    public int ChunkZ { get; internal set; }
    public int ChunkX { get; internal set; }
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
    public PacketOpenSignEntity(Position location, bool isFrontText)
    {
        Location = location;
        IsFrontText = isFrontText;
    }

    public Position Location { get; internal set; }
    public bool IsFrontText { get; internal set; }
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

public class PacketEndCombatEvent
{
    public PacketEndCombatEvent(int duration)
    {
        Duration = duration;
    }

    public int Duration { get; internal set; }
}

public class PacketEnterCombatEvent
{
}

public class PacketDeathCombatEvent
{
    public PacketDeathCombatEvent(int playerId, string message)
    {
        PlayerId = playerId;
        Message = message;
    }

    public int PlayerId { get; internal set; }
    public string Message { get; internal set; }
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
    public PacketRemoveEntityEffect(int entityId, int effectId)
    {
        EntityId = entityId;
        EffectId = effectId;
    }

    public int EntityId { get; internal set; }
    public int EffectId { get; internal set; }
}

public class PacketResourcePackSend
{
    public PacketResourcePackSend(string url, string hash, bool forced, string? promptMessage)
    {
        Url = url;
        Hash = hash;
        Forced = forced;
        PromptMessage = promptMessage;
    }

    public string Url { get; internal set; }
    public string Hash { get; internal set; }
    public bool Forced { get; internal set; }
    public string? PromptMessage { get; internal set; }
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
    public PacketScoreboardDisplayObjective(int position, string name)
    {
        Position = position;
        Name = name;
    }

    public int Position { get; internal set; }
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
    public PacketSpawnPosition(Position location, float angle)
    {
        Location = location;
        Angle = angle;
    }

    public Position Location { get; internal set; }
    public float Angle { get; internal set; }
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

public class PacketSystemChat
{
    public PacketSystemChat(string content, bool isActionBar)
    {
        Content = content;
        IsActionBar = isActionBar;
    }

    public string Content { get; internal set; }
    public bool IsActionBar { get; internal set; }
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

public class PacketSelectAdvancementTab
{
    public PacketSelectAdvancementTab(string? id)
    {
        Id = id;
    }

    public string? Id { get; internal set; }
}

public class PacketServerData
{
    public PacketServerData(string motd, byte[]? iconBytes, bool enforcesSecureChat)
    {
        Motd = motd;
        IconBytes = iconBytes;
        EnforcesSecureChat = enforcesSecureChat;
    }

    public string Motd { get; internal set; }
    public byte[]? IconBytes { get; internal set; }
    public bool EnforcesSecureChat { get; internal set; }
}

public class PacketAcknowledgePlayerDigging
{
    public PacketAcknowledgePlayerDigging(int sequenceId)
    {
        SequenceId = sequenceId;
    }

    public int SequenceId { get; internal set; }
}

public class PacketClearTitles
{
    public PacketClearTitles(bool reset)
    {
        Reset = reset;
    }

    public bool Reset { get; internal set; }
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

    public double X { get; internal set; }
    public double Z { get; internal set; }
    public double OldDiameter { get; internal set; }
    public double NewDiameter { get; internal set; }
    public int Speed { get; internal set; }
    public int PortalTeleportBoundary { get; internal set; }
    public int WarningBlocks { get; internal set; }
    public int WarningTime { get; internal set; }
}

public class PacketActionBar
{
    public PacketActionBar(string text)
    {
        Text = text;
    }

    public string Text { get; internal set; }
}

public class PacketWorldBorderCenter
{
    public PacketWorldBorderCenter(double x, double z)
    {
        X = x;
        Z = z;
    }

    public double X { get; internal set; }
    public double Z { get; internal set; }
}

public class PacketWorldBorderLerpSize
{
    public PacketWorldBorderLerpSize(double oldDiameter, double newDiameter, int speed)
    {
        OldDiameter = oldDiameter;
        NewDiameter = newDiameter;
        Speed = speed;
    }

    public double OldDiameter { get; internal set; }
    public double NewDiameter { get; internal set; }
    public int Speed { get; internal set; }
}

public class PacketWorldBorderSize
{
    public PacketWorldBorderSize(double diameter)
    {
        Diameter = diameter;
    }

    public double Diameter { get; internal set; }
}

public class PacketWorldBorderWarningDelay
{
    public PacketWorldBorderWarningDelay(int warningTime)
    {
        WarningTime = warningTime;
    }

    public int WarningTime { get; internal set; }
}

public class PacketWorldBorderWarningReach
{
    public PacketWorldBorderWarningReach(int warningBlocks)
    {
        WarningBlocks = warningBlocks;
    }

    public int WarningBlocks { get; internal set; }
}

public class PacketPing
{
    public PacketPing(int id)
    {
        Id = id;
    }

    public int Id { get; internal set; }
}

public class PacketPingResponse
{
    public PacketPingResponse(long id)
    {
        Id = id;
    }

    public long Id { get; internal set; }
}

public class PacketSetTitleSubtitle
{
    public PacketSetTitleSubtitle(string text)
    {
        Text = text;
    }

    public string Text { get; internal set; }
}

public class PacketSetTitleText
{
    public PacketSetTitleText(string text)
    {
        Text = text;
    }

    public string Text { get; internal set; }
}

public class PacketSetTitleTime
{
    public PacketSetTitleTime(int fadeIn, int stay, int fadeOut)
    {
        FadeIn = fadeIn;
        Stay = stay;
        FadeOut = fadeOut;
    }

    public int FadeIn { get; internal set; }
    public int Stay { get; internal set; }
    public int FadeOut { get; internal set; }
}

public class PacketSimulationDistance
{
    public PacketSimulationDistance(int distance)
    {
        Distance = distance;
    }

    public int Distance { get; internal set; }
}

public class PacketHurtAnimation
{
    public PacketHurtAnimation(int entityId, float yaw)
    {
        EntityId = entityId;
        Yaw = yaw;
    }

    public int EntityId { get; internal set; }
    public float Yaw { get; internal set; }
}

public class PacketStartConfiguration
{
}