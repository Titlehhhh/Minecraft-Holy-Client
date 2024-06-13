using McProtoNet.Serialization;
using McProtoNet.Protocol;
using McProtoNet.Abstractions;
using System.Reactive.Subjects;

namespace McProtoNet.Protocol763
{
    public sealed class Protocol_763 : ProtocolBase
    {
        public Protocol_763(IPacketBroker client) : base(client)
        {
        }

        private readonly Subject<PacketSpawnEntity> _onspawn_entity = new();
        private readonly Subject<PacketSpawnEntityExperienceOrb> _onspawn_entity_experience_orb = new();
        private readonly Subject<PacketNamedEntitySpawn> _onnamed_entity_spawn = new();
        private readonly Subject<PacketAnimation> _onanimation = new();
        private readonly Subject<PacketDifficulty> _ondifficulty = new();
        private readonly Subject<PacketCloseWindow> _onclose_window = new();
        private readonly Subject<PacketOpenWindow> _onopen_window = new();
        private readonly Subject<PacketCraftProgressBar> _oncraft_progress_bar = new();
        private readonly Subject<PacketSetCooldown> _onset_cooldown = new();
        private readonly Subject<PacketChatSuggestions> _onchat_suggestions = new();
        private readonly Subject<PacketKickDisconnect> _onkick_disconnect = new();
        private readonly Subject<PacketProfilelessChat> _onprofileless_chat = new();
        private readonly Subject<PacketEntityStatus> _onentity_status = new();
        private readonly Subject<PacketUnloadChunk> _onunload_chunk = new();
        private readonly Subject<PacketGameStateChange> _ongame_state_change = new();
        private readonly Subject<PacketOpenHorseWindow> _onopen_horse_window = new();
        private readonly Subject<PacketKeepAlive> _onkeep_alive = new();
        private readonly Subject<PacketRelEntityMove> _onrel_entity_move = new();
        private readonly Subject<PacketEntityMoveLook> _onentity_move_look = new();
        private readonly Subject<PacketEntityLook> _onentity_look = new();
        private readonly Subject<PacketVehicleMove> _onvehicle_move = new();
        private readonly Subject<PacketOpenBook> _onopen_book = new();
        private readonly Subject<PacketCraftRecipeResponse> _oncraft_recipe_response = new();
        private readonly Subject<PacketAbilities> _onabilities = new();
        private readonly Subject<PacketEndCombatEvent> _onend_combat_event = new();
        private readonly Subject<PacketEnterCombatEvent> _onenter_combat_event = new();
        private readonly Subject<PacketDeathCombatEvent> _ondeath_combat_event = new();
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
        private readonly Subject<PacketSystemChat> _onsystem_chat = new();
        private readonly Subject<PacketPlayerlistHeader> _onplayerlist_header = new();
        private readonly Subject<PacketCollect> _oncollect = new();
        private readonly Subject<PacketEntityTeleport> _onentity_teleport = new();
        private readonly Subject<PacketFeatureFlags> _onfeature_flags = new();
        private readonly Subject<PacketSelectAdvancementTab> _onselect_advancement_tab = new();
        private readonly Subject<PacketServerData> _onserver_data = new();
        private readonly Subject<PacketAcknowledgePlayerDigging> _onacknowledge_player_digging = new();
        private readonly Subject<PacketClearTitles> _onclear_titles = new();
        private readonly Subject<PacketInitializeWorldBorder> _oninitialize_world_border = new();
        private readonly Subject<PacketActionBar> _onaction_bar = new();
        private readonly Subject<PacketWorldBorderCenter> _onworld_border_center = new();
        private readonly Subject<PacketWorldBorderLerpSize> _onworld_border_lerp_size = new();
        private readonly Subject<PacketWorldBorderSize> _onworld_border_size = new();
        private readonly Subject<PacketWorldBorderWarningDelay> _onworld_border_warning_delay = new();
        private readonly Subject<PacketWorldBorderWarningReach> _onworld_border_warning_reach = new();
        private readonly Subject<PacketPing> _onping = new();
        private readonly Subject<PacketSetTitleSubtitle> _onset_title_subtitle = new();
        private readonly Subject<PacketSetTitleText> _onset_title_text = new();
        private readonly Subject<PacketSetTitleTime> _onset_title_time = new();
        private readonly Subject<PacketSimulationDistance> _onsimulation_distance = new();
        private readonly Subject<PacketHurtAnimation> _onhurt_animation = new();

        public IObservable<PacketSpawnEntity> OnSpawnEntityPacket => _onspawn_entity;

        public IObservable<PacketSpawnEntityExperienceOrb> OnSpawnEntityExperienceOrbPacket =>
            _onspawn_entity_experience_orb;

        public IObservable<PacketNamedEntitySpawn> OnNamedEntitySpawnPacket => _onnamed_entity_spawn;
        public IObservable<PacketAnimation> OnAnimationPacket => _onanimation;
        public IObservable<PacketDifficulty> OnDifficultyPacket => _ondifficulty;
        public IObservable<PacketCloseWindow> OnCloseWindowPacket => _onclose_window;
        public IObservable<PacketOpenWindow> OnOpenWindowPacket => _onopen_window;
        public IObservable<PacketCraftProgressBar> OnCraftProgressBarPacket => _oncraft_progress_bar;
        public IObservable<PacketSetCooldown> OnSetCooldownPacket => _onset_cooldown;
        public IObservable<PacketChatSuggestions> OnChatSuggestionsPacket => _onchat_suggestions;
        public IObservable<PacketKickDisconnect> OnKickDisconnectPacket => _onkick_disconnect;
        public IObservable<PacketProfilelessChat> OnProfilelessChatPacket => _onprofileless_chat;
        public IObservable<PacketEntityStatus> OnEntityStatusPacket => _onentity_status;
        public IObservable<PacketUnloadChunk> OnUnloadChunkPacket => _onunload_chunk;
        public IObservable<PacketGameStateChange> OnGameStateChangePacket => _ongame_state_change;
        public IObservable<PacketOpenHorseWindow> OnOpenHorseWindowPacket => _onopen_horse_window;
        public IObservable<PacketKeepAlive> OnKeepAlivePacket => _onkeep_alive;
        public IObservable<PacketRelEntityMove> OnRelEntityMovePacket => _onrel_entity_move;
        public IObservable<PacketEntityMoveLook> OnEntityMoveLookPacket => _onentity_move_look;
        public IObservable<PacketEntityLook> OnEntityLookPacket => _onentity_look;
        public IObservable<PacketVehicleMove> OnVehicleMovePacket => _onvehicle_move;
        public IObservable<PacketOpenBook> OnOpenBookPacket => _onopen_book;
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
        public IObservable<PacketUpdateTime> OnUpdateTimePacket => _onupdate_time;
        public IObservable<PacketSystemChat> OnSystemChatPacket => _onsystem_chat;
        public IObservable<PacketPlayerlistHeader> OnPlayerlistHeaderPacket => _onplayerlist_header;
        public IObservable<PacketCollect> OnCollectPacket => _oncollect;
        public IObservable<PacketEntityTeleport> OnEntityTeleportPacket => _onentity_teleport;
        public IObservable<PacketFeatureFlags> OnFeatureFlagsPacket => _onfeature_flags;
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
        public IObservable<PacketSetTitleSubtitle> OnSetTitleSubtitlePacket => _onset_title_subtitle;
        public IObservable<PacketSetTitleText> OnSetTitleTextPacket => _onset_title_text;
        public IObservable<PacketSetTitleTime> OnSetTitleTimePacket => _onset_title_time;
        public IObservable<PacketSimulationDistance> OnSimulationDistancePacket => _onsimulation_distance;
        public IObservable<PacketHurtAnimation> OnHurtAnimationPacket => _onhurt_animation;

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
            writer.WriteVarInt(0x0e);
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
            writer.WriteVarInt(0x0f);
            writer.WriteVarInt(transactionId);
            writer.WriteVarInt(entityId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPickItem(int slot)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1a);
            writer.WriteVarInt(slot);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendNameItem(string name)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x23);
            writer.WriteString(name);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSelectTrade(int slot)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x26);
            writer.WriteVarInt(slot);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSetBeaconEffect(int? primary_effect, int? secondary_effect)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x27);
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
            writer.WriteVarInt(0x2a);
            writer.WriteVarInt(entityId);
            writer.WriteString(command);
            writer.WriteBoolean(track_output);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendTabComplete(int transactionId, string text)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x09);
            writer.WriteVarInt(transactionId);
            writer.WriteString(text);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendClientCommand(int actionId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x07);
            writer.WriteVarInt(actionId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSettings(string locale, sbyte viewDistance, int chatFlags, bool chatColors, byte skinParts,
            int mainHand, bool enableTextFiltering, bool enableServerListing)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x08);
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
            writer.WriteVarInt(0x0a);
            writer.WriteSignedByte(windowId);
            writer.WriteSignedByte(enchantment);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCloseWindow(byte windowId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x0c);
            writer.WriteUnsignedByte(windowId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendKeepAlive(long keepAliveId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x12);
            writer.WriteSignedLong(keepAliveId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendLockDifficulty(bool locked)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x13);
            writer.WriteBoolean(locked);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPosition(double x, double y, double z, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x14);
            writer.WriteDouble(x);
            writer.WriteDouble(y);
            writer.WriteDouble(z);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPositionLook(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x15);
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
            writer.WriteVarInt(0x16);
            writer.WriteFloat(yaw);
            writer.WriteFloat(pitch);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendFlying(bool onGround)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x17);
            writer.WriteBoolean(onGround);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendVehicleMove(double x, double y, double z, float yaw, float pitch)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x18);
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
            writer.WriteVarInt(0x19);
            writer.WriteBoolean(leftPaddle);
            writer.WriteBoolean(rightPaddle);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendCraftRecipeRequest(sbyte windowId, string recipe, bool makeAll)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1b);
            writer.WriteSignedByte(windowId);
            writer.WriteString(recipe);
            writer.WriteBoolean(makeAll);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendAbilities(sbyte flags)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1c);
            writer.WriteSignedByte(flags);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendEntityAction(int entityId, int actionId, int jumpBoost)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1e);
            writer.WriteVarInt(entityId);
            writer.WriteVarInt(actionId);
            writer.WriteVarInt(jumpBoost);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSteerVehicle(float sideways, float forward, byte jump)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x1f);
            writer.WriteFloat(sideways);
            writer.WriteFloat(forward);
            writer.WriteUnsignedByte(jump);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendDisplayedRecipe(string recipeId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x22);
            writer.WriteString(recipeId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendRecipeBook(int bookId, bool bookOpen, bool filterActive)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x21);
            writer.WriteVarInt(bookId);
            writer.WriteBoolean(bookOpen);
            writer.WriteBoolean(filterActive);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendResourcePackReceive(int result)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x24);
            writer.WriteVarInt(result);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendHeldItemSlot(short slotId)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x28);
            writer.WriteSignedShort(slotId);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendArmAnimation(int hand)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x2f);
            writer.WriteVarInt(hand);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendSpectate(Guid target)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x30);
            writer.WriteUUID(target);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendUseItem(int hand, int sequence)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x32);
            writer.WriteVarInt(hand);
            writer.WriteVarInt(sequence);
            return base.SendPacketCore(writer.GetWrittenMemory());
        }

        public Task SendPong(int id)
        {
            scoped var writer = new MinecraftPrimitiveWriterSlim();
            writer.WriteVarInt(0x20);
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

        public override void OnPacketReceived(InputPacket packet)
        {
            switch (packet.Id)
            {
                case 0x01:
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
                case 0x03:
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
                case 0x04:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var animation = reader.ReadUnsignedByte();
                    _onanimation.OnNext(new PacketAnimation(entityId, animation));
                }
                    break;
                case 0x0c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var difficulty = reader.ReadUnsignedByte();
                    var difficultyLocked = reader.ReadBoolean();
                    _ondifficulty.OnNext(new PacketDifficulty(difficulty, difficultyLocked));
                }
                    break;
                case 0x11:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    _onclose_window.OnNext(new PacketCloseWindow(windowId));
                }
                    break;
                case 0x30:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadVarInt();
                    var inventoryType = reader.ReadVarInt();
                    var windowTitle = reader.ReadString();
                    _onopen_window.OnNext(new PacketOpenWindow(windowId, inventoryType, windowTitle));
                }
                    break;
                case 0x13:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    var property = reader.ReadSignedShort();
                    var value = reader.ReadSignedShort();
                    _oncraft_progress_bar.OnNext(new PacketCraftProgressBar(windowId, property, value));
                }
                    break;
                case 0x15:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var itemID = reader.ReadVarInt();
                    var cooldownTicks = reader.ReadVarInt();
                    _onset_cooldown.OnNext(new PacketSetCooldown(itemID, cooldownTicks));
                }
                    break;
                case 0x16:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var action = reader.ReadVarInt();
                    var tempArrayLength_0 = reader.ReadVarInt();
                    var tempArray_0 = new string[tempArrayLength_0];
                    for (int i_0 = 0; i_0 < tempArrayLength_0; i_0++)
                    {
                        var for_item_0 = reader.ReadString();
                        tempArray_0[i_0] = for_item_0;
                    }

                    var entries = tempArray_0;
                    _onchat_suggestions.OnNext(new PacketChatSuggestions(action, entries));
                }
                    break;
                case 0x1a:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var reason = reader.ReadString();
                    _onkick_disconnect.OnNext(new PacketKickDisconnect(reason));
                }
                    break;
                case 0x1b:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var message = reader.ReadString();
                    var type = reader.ReadVarInt();
                    var name = reader.ReadString();
                    if (!reader.ReadBoolean())
                    {
                        var target = null;
                    }
                    else
                    {
                        var target = reader.ReadString();
                    }

                    _onprofileless_chat.OnNext(new PacketProfilelessChat(message, type, name, target));
                }
                    break;
                case 0x1c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadSignedInt();
                    var entityStatus = reader.ReadSignedByte();
                    _onentity_status.OnNext(new PacketEntityStatus(entityId, entityStatus));
                }
                    break;
                case 0x1e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var chunkX = reader.ReadSignedInt();
                    var chunkZ = reader.ReadSignedInt();
                    _onunload_chunk.OnNext(new PacketUnloadChunk(chunkX, chunkZ));
                }
                    break;
                case 0x1f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var reason = reader.ReadUnsignedByte();
                    var gameMode = reader.ReadFloat();
                    _ongame_state_change.OnNext(new PacketGameStateChange(reason, gameMode));
                }
                    break;
                case 0x20:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadUnsignedByte();
                    var nbSlots = reader.ReadVarInt();
                    var entityId = reader.ReadSignedInt();
                    _onopen_horse_window.OnNext(new PacketOpenHorseWindow(windowId, nbSlots, entityId));
                }
                    break;
                case 0x23:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var keepAliveId = reader.ReadSignedLong();
                    _onkeep_alive.OnNext(new PacketKeepAlive(keepAliveId));
                }
                    break;
                case 0x2b:
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
                case 0x2c:
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
                case 0x2d:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var yaw = reader.ReadSignedByte();
                    var pitch = reader.ReadSignedByte();
                    var onGround = reader.ReadBoolean();
                    _onentity_look.OnNext(new PacketEntityLook(entityId, yaw, pitch, onGround));
                }
                    break;
                case 0x2e:
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
                case 0x2f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var hand = reader.ReadVarInt();
                    _onopen_book.OnNext(new PacketOpenBook(hand));
                }
                    break;
                case 0x33:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var windowId = reader.ReadSignedByte();
                    var recipe = reader.ReadString();
                    _oncraft_recipe_response.OnNext(new PacketCraftRecipeResponse(windowId, recipe));
                }
                    break;
                case 0x34:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var flags = reader.ReadSignedByte();
                    var flyingSpeed = reader.ReadFloat();
                    var walkingSpeed = reader.ReadFloat();
                    _onabilities.OnNext(new PacketAbilities(flags, flyingSpeed, walkingSpeed));
                }
                    break;
                case 0x36:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var duration = reader.ReadVarInt();
                    _onend_combat_event.OnNext(new PacketEndCombatEvent(duration));
                }
                    break;
                case 0x37:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    _onenter_combat_event.OnNext(new PacketEnterCombatEvent());
                }
                    break;
                case 0x38:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var playerId = reader.ReadVarInt();
                    var message = reader.ReadString();
                    _ondeath_combat_event.OnNext(new PacketDeathCombatEvent(playerId, message));
                }
                    break;
                case 0x3c:
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
                case 0x3e:
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
                case 0x3f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var effectId = reader.ReadVarInt();
                    _onremove_entity_effect.OnNext(new PacketRemoveEntityEffect(entityId, effectId));
                }
                    break;
                case 0x40:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var url = reader.ReadString();
                    var hash = reader.ReadString();
                    var forced = reader.ReadBoolean();
                    if (!reader.ReadBoolean())
                    {
                        var promptMessage = null;
                    }
                    else
                    {
                        var promptMessage = reader.ReadString();
                    }

                    _onresource_pack_send.OnNext(new PacketResourcePackSend(url, hash, forced, promptMessage));
                }
                    break;
                case 0x42:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var headYaw = reader.ReadSignedByte();
                    _onentity_head_rotation.OnNext(new PacketEntityHeadRotation(entityId, headYaw));
                }
                    break;
                case 0x4c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var cameraId = reader.ReadVarInt();
                    _oncamera.OnNext(new PacketCamera(cameraId));
                }
                    break;
                case 0x4d:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var slot = reader.ReadSignedByte();
                    _onheld_item_slot.OnNext(new PacketHeldItemSlot(slot));
                }
                    break;
                case 0x4e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var chunkX = reader.ReadVarInt();
                    var chunkZ = reader.ReadVarInt();
                    _onupdate_view_position.OnNext(new PacketUpdateViewPosition(chunkX, chunkZ));
                }
                    break;
                case 0x4f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var viewDistance = reader.ReadVarInt();
                    _onupdate_view_distance.OnNext(new PacketUpdateViewDistance(viewDistance));
                }
                    break;
                case 0x51:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var position = reader.ReadSignedByte();
                    var name = reader.ReadString();
                    _onscoreboard_display_objective.OnNext(new PacketScoreboardDisplayObjective(position, name));
                }
                    break;
                case 0x53:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadSignedInt();
                    var vehicleId = reader.ReadSignedInt();
                    _onattach_entity.OnNext(new PacketAttachEntity(entityId, vehicleId));
                }
                    break;
                case 0x54:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var velocityX = reader.ReadSignedShort();
                    var velocityY = reader.ReadSignedShort();
                    var velocityZ = reader.ReadSignedShort();
                    _onentity_velocity.OnNext(new PacketEntityVelocity(entityId, velocityX, velocityY, velocityZ));
                }
                    break;
                case 0x56:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var experienceBar = reader.ReadFloat();
                    var totalExperience = reader.ReadVarInt();
                    var level = reader.ReadVarInt();
                    _onexperience.OnNext(new PacketExperience(experienceBar, totalExperience, level));
                }
                    break;
                case 0x57:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var health = reader.ReadFloat();
                    var food = reader.ReadVarInt();
                    var foodSaturation = reader.ReadFloat();
                    _onupdate_health.OnNext(new PacketUpdateHealth(health, food, foodSaturation));
                }
                    break;
                case 0x59:
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
                case 0x5e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var age = reader.ReadSignedLong();
                    var time = reader.ReadSignedLong();
                    _onupdate_time.OnNext(new PacketUpdateTime(age, time));
                }
                    break;
                case 0x64:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var content = reader.ReadString();
                    var isActionBar = reader.ReadBoolean();
                    _onsystem_chat.OnNext(new PacketSystemChat(content, isActionBar));
                }
                    break;
                case 0x65:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var header = reader.ReadString();
                    var footer = reader.ReadString();
                    _onplayerlist_header.OnNext(new PacketPlayerlistHeader(header, footer));
                }
                    break;
                case 0x67:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var collectedEntityId = reader.ReadVarInt();
                    var collectorEntityId = reader.ReadVarInt();
                    var pickupItemCount = reader.ReadVarInt();
                    _oncollect.OnNext(new PacketCollect(collectedEntityId, collectorEntityId, pickupItemCount));
                }
                    break;
                case 0x68:
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
                case 0x6b:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var tempArrayLength_0 = reader.ReadVarInt();
                    var tempArray_0 = new string[tempArrayLength_0];
                    for (int i_0 = 0; i_0 < tempArrayLength_0; i_0++)
                    {
                        var for_item_0 = reader.ReadString();
                        tempArray_0[i_0] = for_item_0;
                    }

                    var features = tempArray_0;
                    _onfeature_flags.OnNext(new PacketFeatureFlags(features));
                }
                    break;
                case 0x44:
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
                case 0x45:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var motd = reader.ReadString();
                    if (!reader.ReadBoolean())
                    {
                        var iconBytes = null;
                    }
                    else
                    {
                        var tempArrayLength_1 = reader.ReadVarInt();
                        var iconBytes = reader.ReadBuffer(tempArrayLength_1);
                    }

                    var enforcesSecureChat = reader.ReadBoolean();
                    _onserver_data.OnNext(new PacketServerData(motd, iconBytes, enforcesSecureChat));
                }
                    break;
                case 0x06:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var sequenceId = reader.ReadVarInt();
                    _onacknowledge_player_digging.OnNext(new PacketAcknowledgePlayerDigging(sequenceId));
                }
                    break;
                case 0x0e:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var reset = reader.ReadBoolean();
                    _onclear_titles.OnNext(new PacketClearTitles(reset));
                }
                    break;
                case 0x22:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var x = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    var oldDiameter = reader.ReadDouble();
                    var newDiameter = reader.ReadDouble();
                    var speed = reader.ReadVarInt();
                    var portalTeleportBoundary = reader.ReadVarInt();
                    var warningBlocks = reader.ReadVarInt();
                    var warningTime = reader.ReadVarInt();
                    _oninitialize_world_border.OnNext(new PacketInitializeWorldBorder(x, z, oldDiameter, newDiameter,
                        speed, portalTeleportBoundary, warningBlocks, warningTime));
                }
                    break;
                case 0x46:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var text = reader.ReadString();
                    _onaction_bar.OnNext(new PacketActionBar(text));
                }
                    break;
                case 0x47:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var x = reader.ReadDouble();
                    var z = reader.ReadDouble();
                    _onworld_border_center.OnNext(new PacketWorldBorderCenter(x, z));
                }
                    break;
                case 0x48:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var oldDiameter = reader.ReadDouble();
                    var newDiameter = reader.ReadDouble();
                    var speed = reader.ReadVarInt();
                    _onworld_border_lerp_size.OnNext(new PacketWorldBorderLerpSize(oldDiameter, newDiameter, speed));
                }
                    break;
                case 0x49:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var diameter = reader.ReadDouble();
                    _onworld_border_size.OnNext(new PacketWorldBorderSize(diameter));
                }
                    break;
                case 0x4a:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var warningTime = reader.ReadVarInt();
                    _onworld_border_warning_delay.OnNext(new PacketWorldBorderWarningDelay(warningTime));
                }
                    break;
                case 0x4b:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var warningBlocks = reader.ReadVarInt();
                    _onworld_border_warning_reach.OnNext(new PacketWorldBorderWarningReach(warningBlocks));
                }
                    break;
                case 0x32:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var id = reader.ReadSignedInt();
                    _onping.OnNext(new PacketPing(id));
                }
                    break;
                case 0x5d:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var text = reader.ReadString();
                    _onset_title_subtitle.OnNext(new PacketSetTitleSubtitle(text));
                }
                    break;
                case 0x5f:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var text = reader.ReadString();
                    _onset_title_text.OnNext(new PacketSetTitleText(text));
                }
                    break;
                case 0x60:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var fadeIn = reader.ReadSignedInt();
                    var stay = reader.ReadSignedInt();
                    var fadeOut = reader.ReadSignedInt();
                    _onset_title_time.OnNext(new PacketSetTitleTime(fadeIn, stay, fadeOut));
                }
                    break;
                case 0x5c:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var distance = reader.ReadVarInt();
                    _onsimulation_distance.OnNext(new PacketSimulationDistance(distance));
                }
                    break;
                case 0x21:
                {
                    scoped var reader = new MinecraftPrimitiveReaderSlim(packet.Data);
                    var entityId = reader.ReadVarInt();
                    var yaw = reader.ReadFloat();
                    _onhurt_animation.OnNext(new PacketHurtAnimation(entityId, yaw));
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
        public PacketEnterCombatEvent()
        {
        }
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
        public PacketExperience(float experienceBar, int totalExperience, int level)
        {
            ExperienceBar = experienceBar;
            TotalExperience = totalExperience;
            Level = level;
        }

        public float ExperienceBar { get; internal set; }
        public int TotalExperience { get; internal set; }
        public int Level { get; internal set; }
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

    public class PacketFeatureFlags
    {
        public PacketFeatureFlags(string[] features)
        {
            Features = features;
        }

        public string[] Features { get; internal set; }
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
}